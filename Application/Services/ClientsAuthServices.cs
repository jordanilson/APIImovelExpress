using Application.DTOs;
using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ClientsAuthServices : IClientsAuthServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ClientsAuthServices(UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        public async Task<ClientsRegisterDTO> GetClientsAsync()
        {
            var requestingUserId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

            if (string.IsNullOrEmpty(requestingUserId))
            {
                throw new SecurityTokenException("Token inválido ou usuário não autenticado");
            }

            var userById = await _userManager.FindByIdAsync(requestingUserId);

            if (userById == null)
            {
                throw new Exception("Usuário não encontrado");
            }

            var RegisterClientsDTO = new ClientsRegisterDTO
            {
                Id = userById.Id,
                Email = userById.Email,
                Name = userById.Name
            };

            return RegisterClientsDTO;
        }

        public async Task<ClientsLoginDTO> ClientsLoginAsync(ClientsLoginDTO clientsLoginDTO)
        {
            var clients = await _userManager.FindByEmailAsync(clientsLoginDTO.Email);

            if (clients == null || !await _userManager.CheckPasswordAsync(clients, clientsLoginDTO.Password))
            {
                return null;
            }

            var authorizeRolesLogin = await _userManager.IsInRoleAsync(clients, "Clients");

            if (!authorizeRolesLogin)
            {
                return null;
            }

            var LoginClientsDetails = new ClientsLoginDTO
            {
                Email = clientsLoginDTO.Email,
            };
            var token = GenerateJwtToken(clients, "Clients");
            LoginClientsDetails.Token = token;

            return LoginClientsDetails;
        }

        public async Task<ClientsRegisterDTO> ClientsRegisterAsync(ClientsRegisterDTO clientsRegisterDTO)
        {
            var existingUserByEmail = await _userManager.FindByEmailAsync(clientsRegisterDTO.Email);

            if (existingUserByEmail != null)
            {
                throw new Exception("Este email já está sendo usado por outro usuário.");
            }

            var usuario = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString(),
                Email = clientsRegisterDTO.Email,
                Name = clientsRegisterDTO.Name,
            };

            var result = await _userManager.CreateAsync(usuario, clientsRegisterDTO.Password);

            if (result.Succeeded)
            {
                await _userManager.UpdateAsync(usuario);
                await _userManager.AddToRoleAsync(usuario, "Clients");
                var RegisterClientsDetails = new ClientsRegisterDTO
                {
                    Id = usuario.UserName,
                    Email = usuario.Email,
                    Name = usuario.Name,
                };

                var token = GenerateJwtToken(usuario, "Clients");
                RegisterClientsDetails.Token = token;

                return RegisterClientsDetails;
            }

            throw new Exception("Erro ao registrar");
        }

        private string GenerateJwtToken(ApplicationUser usuario, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
