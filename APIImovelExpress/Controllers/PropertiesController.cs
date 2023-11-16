using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Interfaaces;
using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowRequest")]

    public class PropertiesController : ControllerBase
    {
        private readonly IPropertiesServices _propertiesServices;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PropertiesController(IPropertiesServices propertiesServices, IConfiguration configuration, IMapper mapper)
        {
            _propertiesServices = propertiesServices;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<PropertiesDTO>>> GetProperties()
        {
            return await _propertiesServices.GetPropertiesAsyncAll();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PropertiesDTO>> GetPropertyById(string id)
        {
            if(id == null)
            {
                return NotFound($"Propetário com {id} não encotrado!");
            }

            var propertiesBById = await _propertiesServices.GetPropertiesAsyncById(id);
            return Ok(propertiesBById);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpPost]
        public async Task<ActionResult<PropertiesDTO>> PostProperties([FromForm] PropertiesDTO propertiesDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                const string connectionStringName = "AzureStorage:ConnectionString";
                const string containerNameName = "AzureStorage:ContainerName";

                var connectionString = _configuration[connectionStringName];
                var containerName = _configuration[containerNameName];

                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                {
                    return BadRequest("Configuração incorreta.");
                }

                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (!await blobContainerClient.ExistsAsync())
                {
                    await blobContainerClient.CreateAsync();
                }

                var blobFileName = Guid.NewGuid().ToString() + Path.GetExtension(propertiesDTO.Photo.FileName);
                var blobClient = blobContainerClient.GetBlobClient(blobFileName);

                using (var stream = propertiesDTO.Photo.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, true);
                }

                propertiesDTO.ImgUrl = blobClient.Uri.ToString();
                var property = new PropertiesDTO
                {
                    City = propertiesDTO.City,
                    State = propertiesDTO.State,
                    Price = propertiesDTO.Price,
                    Availability = propertiesDTO.Availability,
                    Amenities = propertiesDTO.Amenities,
                    Name = propertiesDTO.Name,
                    ImgUrl = blobClient.Uri.ToString(),
                    Description = propertiesDTO.Description,
                    OwnersId = userId
                };

                await _propertiesServices.CreatePropertiesAsyncById(property);
                return Ok(propertiesDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno no servidor: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpPut]
        public async Task<ActionResult<PropertiesDTO>> UpdateProperties([FromForm] PropertiesDTO propertiesDTO)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null || userId != propertiesDTO.OwnersId)
                {
                    return Unauthorized("Usuário não autorizado para atualizar esta propriedade.");
                }

                const string connectionStringName = "AzureStorage:ConnectionString";
                const string containerNameName = "AzureStorage:ContainerName";

                var connectionString = _configuration[connectionStringName];
                var containerName = _configuration[containerNameName];

                if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(containerName))
                {
                    return BadRequest("Configuração incorreta.");
                }

                var blobServiceClient = new BlobServiceClient(connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (!string.IsNullOrEmpty(propertiesDTO.ImgUrl) && propertiesDTO.Photo != null)
                {
                    try
                    {
                        var existingBlobUri = new Uri(propertiesDTO.ImgUrl);
                        var existingBlobName = Path.GetFileName(existingBlobUri.LocalPath);
                        var existingBlobClient = blobContainerClient.GetBlobClient(existingBlobName);

                        await existingBlobClient.DeleteIfExistsAsync();
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "Erro ao excluir imagem existente: " + ex.Message);
                    }
                }
                
                if (propertiesDTO.Photo != null)
                {
                    try
                    {
                        var blobFileName = Guid.NewGuid().ToString() + Path.GetExtension(propertiesDTO.Photo.FileName);

                        using (var stream = propertiesDTO.Photo.OpenReadStream())
                        {
                            var blobClient = blobContainerClient.GetBlobClient(blobFileName);
                            await blobClient.UploadAsync(stream, true);
                            propertiesDTO.ImgUrl = blobClient.Uri.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, "Erro ao fazer upload da nova imagem: " + ex.Message);
                    }
                }

                var updatedProperties = await _propertiesServices.UpdatePropertiesAsync(propertiesDTO);

                if (updatedProperties == null)
                {
                    return NotFound("Propriedade não encontrada ou usuário não autorizado.");
                }

                return Ok(_mapper.Map<PropertiesDTO>(updatedProperties));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro interno no servidor: " + ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<PropertiesDTO>> DeleteProperties(string id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var deletedProperty = await _propertiesServices.DeletePropertiesAsyncById(id);
            if (deletedProperty == null)
            {
                return NotFound("Propriedade não encontrada.");
            }

            return Ok(deletedProperty);
        }
    }
}
