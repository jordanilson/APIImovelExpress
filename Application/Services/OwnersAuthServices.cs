using Application.Interfaaces;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class OwnersAuthServices : IOwnersAuthServices
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPropertiesServices _propertiesServices;

    public OwnersAuthServices(UserManager<ApplicationUser> userManager, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IPropertiesServices propertiesServices)
    {
        _userManager = userManager;
        _configuration = configuration;
        _httpContextAccessor = httpContextAccessor;
        _propertiesServices = propertiesServices;
    }

    public async Task<OwnersDTO> GetOwnersAsync()
    {
        var userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);

        if (string.IsNullOrEmpty(userId))
        {
            throw new SecurityTokenException("Token inválido ou usuário não autenticado");
        }

        var userProperties = await _propertiesServices.GetPropertiesAsync(userId);

        var ownersDTO = new OwnersDTO
        {
            Id = userId,
            Properties = userProperties
        };

        return ownersDTO;
    }

    public async Task<RegisterOwnersDTO> GetOwnersAsyncById()
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

        var ownersDTO = new RegisterOwnersDTO
        {
            Id = userById.Id,
            Email = userById.Email,
            Name = userById.Name
        };

        return ownersDTO;
    }

    public async Task<LoginOwnersDTO> LoginOwnersAsync(LoginOwnersDTO loginOwnersDTO)
    {
        var owner = await _userManager.FindByEmailAsync(loginOwnersDTO.Email);

        if (owner == null || !await _userManager.CheckPasswordAsync(owner, loginOwnersDTO.Password))
        {
            return null;
        }

        var authorizeRolesLogin = await _userManager.IsInRoleAsync(owner, "Owners");

        if (!authorizeRolesLogin)
        {
            return null;
        }

        var LoginOwnersDetails = new LoginOwnersDTO
        {
            Email = loginOwnersDTO.Email,
        };

        var token = GenerateJwtToken(owner);
        LoginOwnersDetails.Token = token;

        return LoginOwnersDetails;
    }

    public async Task<RegisterOwnersDTO> RegisterOwnersAsync(RegisterOwnersDTO registerOwnersDTO)
    {
        var existingUserByEmail = await _userManager.FindByEmailAsync(registerOwnersDTO.Email);

        if (existingUserByEmail != null)
        {
            throw new Exception("Este email já está sendo usado por outro usuário.");
        }

        var usuario = new ApplicationUser
        {
            UserName = registerOwnersDTO.Email,
            Email = registerOwnersDTO.Email,
            Name = registerOwnersDTO.Name,
        };

        var result = await _userManager.CreateAsync(usuario, registerOwnersDTO.Password);

        if (result.Succeeded)
        {
            await _userManager.UpdateAsync(usuario);
            await _userManager.AddToRoleAsync(usuario, "Owners");
            var RegisterOwnersDetails = new RegisterOwnersDTO
            {
                Id = usuario.UserName,
                Email = usuario.Email,
                Name = usuario.Name,
            };

            var token = GenerateJwtToken(usuario);
            RegisterOwnersDetails.Token = token;

            return RegisterOwnersDetails;
        }

        throw new Exception("Erro ao registrar");
    }

    private string GenerateJwtToken(ApplicationUser usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id),
                new Claim(ClaimTypes.Role, "Owners")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
