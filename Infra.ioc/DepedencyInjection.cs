using Application.Interfaaces;
using Application.Interfaces;
using Application.Services;
using Azure.Storage.Blobs;
using Domain.Interfaces;
using Domain.Models;
using Infra.data.Context;
using Infra.data.Repositorys;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;


namespace Infra.IoC
{
    public static class DependencyInjection
    {
        public static async Task<IServiceCollection> AddInfraInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(opt =>
            {
                var strBD = configuration.GetConnectionString("DefaultConnection");
                opt.UseSqlServer(strBD);
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IPropertiesRepository, PropertiesRepository>();
            services.AddScoped<IPropertiesServices, PropertiesServices>();
            services.AddScoped<IOwnersAuthServices, OwnersAuthServices>();
            services.AddScoped<IClientsAuthServices, ClientsAuthServices>();
            services.AddScoped<IReservatioService, ReservationServices>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            services.AddScoped<RegisterOwners>();
            services.AddScoped<LoginOwners>();
            services.AddScoped<ClientsLogin>();
            services.AddScoped<ClientsRegister>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Jwt:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new OpenApiInfo { Title = "ImovelExpress" });
                s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Informe:'Bearer' [espaço] e o seu Token."
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                              Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Owners", policy => policy.RequireClaim(ClaimTypes.Role, "Owners"));
                options.AddPolicy("Clients", policy => policy.RequireClaim(ClaimTypes.Role, "Clients"));
            });

            await CreateRoles(services.BuildServiceProvider());

         
            return services;
        }

        private static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            IdentityResult roleResult;

            foreach (var roleName in new[] { "Owners", "Clients" })
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));

                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Erro ao criar role: " + roleName);
                    }
                }
            }
        }

    }
}
