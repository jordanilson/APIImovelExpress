using Application.Interfaaces;
using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace YourProjectNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowRequest")]
    public class OwnersController : ControllerBase
    {
        private readonly IOwnersAuthServices _ownersAuthServices;
        private readonly IPropertiesServices _propertiesServices;

        public OwnersController(IOwnersAuthServices ownersAuthServices, IPropertiesServices propertiesServices)
        {
            _ownersAuthServices = ownersAuthServices;
            _propertiesServices = propertiesServices;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpGet]
        public async Task<ActionResult> GetOwners()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }

            var properties = await _propertiesServices.GetPropertiesAsync(userId);
            var userProperties = properties.Where(p => p.OwnersId == userId).ToList();
            return Ok(userProperties);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Owners")]
        [HttpGet("User")]
        public async Task<ActionResult> GetOwnersById()
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return Unauthorized();
            }
            var userOwners = await _ownersAuthServices.GetOwnersAsyncById();
            return Ok(userOwners);

        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterOwnersDTO registerOwnersDTO)
        {
            try
            {
                var result = await _ownersAuthServices.RegisterOwnersAsync(registerOwnersDTO);
                if (result != null)
                {
                    return Ok(result);
                }

                return BadRequest("Erro ao registrar o proprietário.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginOwnersDTO loginOwnersDTO)
        {
            try
            {
                var result = await _ownersAuthServices.LoginOwnersAsync(loginOwnersDTO);
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
