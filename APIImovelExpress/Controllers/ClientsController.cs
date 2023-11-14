using Application.DTOs;
using Application.Interfaaces;
using Application.Interfaces;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIImovelExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowRequest")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientsAuthServices _clientsAuthServices;

        public ClientsController(IClientsAuthServices clientsAuthServices)
        {
            _clientsAuthServices = clientsAuthServices;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Clients")]
        [HttpGet("User")]
        public async Task<ActionResult> GetClientsUser()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var userOwners = await _clientsAuthServices.GetClientsAsync();
            return Ok(userOwners);

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] ClientsRegisterDTO clientsRegisterDTO)
        {
            try
            {
                var result = await _clientsAuthServices.ClientsRegisterAsync(clientsRegisterDTO);
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Erro ao registrar o cliente.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] ClientsLoginDTO clientsLoginDTO)
        {
            try
            {
                var result = await _clientsAuthServices.ClientsLoginAsync(clientsLoginDTO);
                if (result != null)
                {
                    return Ok(result);
                }

                return Unauthorized("Credenciais inválidas.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }


    }
}
