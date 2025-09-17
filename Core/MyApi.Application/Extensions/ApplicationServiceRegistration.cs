
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Common;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Services.Concrete;
using System.Reflection;
namespace MyApi.Application.Extensions
{
    public static class ApplicationServiceRegistration 
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var applicationAssembly = typeof(ProductService).Assembly;


            // AutoMapper'ı en temel ve açık haliyle kaydediyoruz.
            services.AddAutoMapper(cfg =>
            {
                // AutoMapper konfigürasyonuna (cfg), Application katmanındaki
                // Assembly'yi tarayıp içindeki tüm Profile'ları bulmasını söylüyoruz.
                // typeof(UserProfile).Assembly, Application projesini referans almanın en güvenli yoludur.
                cfg.AddMaps(typeof(UserProfile).Assembly);
            });

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
            // BU SATIRI KONTROL ET:
            // Bu satır, MediatR'a "tüm istekleri önce benim ValidationBehavior'ımdan geçir"
            // diyen satırdır. Eğer bu satır eksikse, validasyon pipeline'ı hiç çalışmaz.

            // 1. ADIM: Tüm validator'ları sisteme tanıtır.
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // 2. ADIM: MediatR'a, bu validator'ları çalıştıracak olan ValidationBehavior'ı kullanmasını söyler.
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));




            //services.AddTransient<IValidator<ProductCreateDto>, ProductCreateDtoValidator>();
            //services.AddTransient<IValidator<ProductUpdateDto>, ProductUpdateDtoValidator>();
            //services.AddTransient<IValidator<OrderCreateDto>, OrderCreateDtoValidator>();
            //services.AddTransient<IValidator<OrderUpdateDto>, OrderUpdateDtoValidator>();

            // -----------------------------
            // 3️⃣ AutoMapper profillerini ekle
            // -----------------------------
            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddProfile<ProductProfile>();
            //    cfg.AddProfile<CategoryProfile>();
            //    cfg.AddProfile<OrderProfile>();
            //    cfg.AddProfile<AccountProfile>();
            //});

            return services;
        }
    }
}

