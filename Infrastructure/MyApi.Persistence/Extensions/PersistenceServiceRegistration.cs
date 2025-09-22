using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories;
using MyApi.Persistence.Context;
using MyApi.Persistence.Implementations;
using MyApi.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Persistence.Extensions
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Connection string'i IConfiguration üzerinden al
            var connectionString = configuration.GetConnectionString("SqlServerConnectionString");

            // DbContext ekle
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString),
                ServiceLifetime.Scoped);

            // Repository eşleştirmeleri
            //services.AddScoped<IProductRepositorty, ProductRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //yukarıdai eşleştirmeler yerine aşağıdaki gibi otomatik yapacağız. 

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Generic repository
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // Reflection ile tüm repository interface ve implementasyonlarını ekle
            var persistenceAssembly = typeof(MyApi.Persistence.Repositories.ProductRepository).Assembly;

            var types = persistenceAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository") && !t.IsGenericType);

            foreach (var implementation in types)
            {
                var interfaceType = implementation.GetInterfaces()
                    .FirstOrDefault(i => i.Name == "I" + implementation.Name);
                if (interfaceType != null)
                {
                    services.AddScoped(interfaceType, implementation);
                }
            }

            return services;
        }
    }
}
