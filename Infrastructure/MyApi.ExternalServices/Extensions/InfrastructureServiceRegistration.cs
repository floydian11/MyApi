using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MyApi.Application.Services.OuterServices.FileStorage;
using MyApi.Application.Services.OuterServices.Identity;
using MyApi.Application.Services.OuterServices.Identity.Hash;
using MyApi.Application.Services.OuterServices.Identity.JWT;
using MyApi.ExternalServices.FileStorage;
using MyApi.ExternalServices.Hash;
using MyApi.ExternalServices.Identity;
using MyApi.ExternalServices.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.ExternalServices.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string webRootPath) // dikkat: string path alıyor
        {
            var uploadRoot = Path.Combine(webRootPath, configuration.GetValue<string>("FileStorage:RootPath") ?? "uploads");
            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            services.AddScoped<IFileStorage>(provider => new LocalFileStorage(uploadRoot));

            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);
            // nameof kullanmak daha güvenli

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IHashService, HashService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IClaimService, ClaimService>();
            services.AddScoped<IRoleService, RoleService>();

            return services;
        }
    }
}
