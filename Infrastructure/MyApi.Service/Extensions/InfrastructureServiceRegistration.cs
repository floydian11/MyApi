using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Configuration.JWT;
using MyApi.Application.Services.OuterServices.FileStorage;
using MyApi.Infrastructure.FileStorage;
using MyApi.Infrastructure.JWT;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Infrastructure.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string webRootPath) // dikkat: string path alıyor
        {
            var uploadRoot = Path.Combine(webRootPath, "uploads");

            if (!Directory.Exists(uploadRoot))
                Directory.CreateDirectory(uploadRoot);

            services.AddScoped<IFileStorage>(provider =>
                new LocalFileStorage(uploadRoot));

            var jwtSection = configuration.GetSection("JwtSettings");
            services.Configure<JwtSettings>(jwtSection);
            // nameof kullanmak daha güvenli
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }
    }
}
