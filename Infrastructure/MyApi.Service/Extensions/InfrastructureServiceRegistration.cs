using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Services.OuterServices.FileStorage;
using MyApi.Infrastructure.FileStorage;
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

            return services;
        }
    }
}
