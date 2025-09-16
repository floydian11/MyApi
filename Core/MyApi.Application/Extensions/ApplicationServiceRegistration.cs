
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.DTOs.ARServices.Order;
using MyApi.Application.DTOs.ARServices.Product;
using MyApi.Application.Mapping.ARServices;
using MyApi.Application.Mapping.ExternalServices.Account;
using MyApi.Application.Services.Concrete;
using MyApi.Application.Services.OuterServices.FileStorage; // AddValidatorsFromAssemblyContaining için yeterliusing Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Validators;
using System.Reflection;
namespace MyApi.Application.Extensions
{
    public static class ApplicationServiceRegistration 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ProductService).Assembly;

            // MediatR'ı Application katmanındaki tüm Handler'ları ve diğer bileşenleri
            // bulacak şekilde otomatik olarak kaydet.
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


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

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            //services.AddTransient<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
            //services.AddTransient<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
            //services.AddTransient<IValidator<OrderUpdateDto>, OrderUpdateDtoValidator>();

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

