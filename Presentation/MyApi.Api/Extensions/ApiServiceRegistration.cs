
using FluentValidation; // AddValidatorsFromAssemblyContaining için
using Microsoft.Extensions.DependencyInjection;
using MyApi.Application.Validators.Category; // Validator class’larının namespace’i

namespace MyApi.Api.Extensions
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddApiValidators(this IServiceCollection services)
        {
            var assembly = typeof(CategoryCreateDtoValidator).Assembly;

            // IValidator implementasyonlarını tarayıp ekle
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsAbstract && typeof(IValidator).IsAssignableFrom(type))
                {
                    services.AddScoped(typeof(IValidator), type);
                }
            }

            return services;
        }
    }
}
