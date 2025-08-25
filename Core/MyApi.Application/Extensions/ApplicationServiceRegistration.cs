using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Extensions
{
    public static class ApplicationServiceRegistration 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            // Service  eşleştirmeleri
            //services.AddScoped<IProductRepositorty, ProductRepository>();
            //services.AddScoped<IOrderRepository, OrderRepository>();
            //services.AddScoped<ICategoryRepository, CategoryRepository>();
            //yukarıdai eşleştirmeler yerine aşağıdaki gibi otomatik yapacağız. 

            // Reflection ile tüm repository interface ve implementasyonlarını ekle
            var applicationAssembly = typeof(MyApi.Application.Services.Concrete.ProductService).Assembly;

            var types = applicationAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service") && !t.IsGenericType);

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

