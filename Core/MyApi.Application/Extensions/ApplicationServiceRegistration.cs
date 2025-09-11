
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.DTOs.Order;
using MyApi.Application.DTOs.Product;
using MyApi.Application.Mapping;
using MyApi.Application.Mapping.Account;
using MyApi.Application.Services.Concrete;
using MyApi.Application.Services.OuterServices.FileStorage; // AddValidatorsFromAssemblyContaining için yeterliusing Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Validators;
namespace MyApi.Application.Extensions
{
    public static class ApplicationServiceRegistration 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ProductService).Assembly;

            // -----------------------------
            // 1️⃣ Service’leri otomatik register et
            // -----------------------------
            var types = applicationAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service") && !t.IsGenericType);

            foreach (var impl in types)
            {
                var iface = impl.GetInterfaces().FirstOrDefault(i => i.Name == "I" + impl.Name);
                if (iface != null)
                    services.AddScoped(iface, impl);
            }

            // -----------------------------
            // 2️⃣ Validator’ları register et
            // -----------------------------
            services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            services.AddTransient<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
            services.AddTransient<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
            services.AddTransient<IValidator<OrderUpdateDto>, OrderUpdateDtoValidator>();

            // -----------------------------
            // 3️⃣ AutoMapper profillerini ekle
            // -----------------------------
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<OrderProfile>();
                cfg.AddProfile<AccountProfile>();
            });

            return services;
        }
    }
}

