
using AutoMapper;
using FluentValidation; // AddValidatorsFromAssemblyContaining için
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MyApi.Api.Services;
using MyApi.Application.Services.Abstract;
using MyApi.Application.Services.Abstract.Account;
using MyApi.Application.Validators;
using MyApi.Domain.Entities.Identity; // Validator class’larının namespace’i

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

            // AccountService concrete
            services.AddScoped<IAccountService>(provider =>
                new AccountService(
                    provider.GetRequiredService<UserManager<AppUser>>(),
                    provider.GetRequiredService<SignInManager<AppUser>>(),
                    provider.GetRequiredService<RoleManager<AppRole>>(),
                    provider.GetRequiredService<IMapper>(),
                    provider.GetRequiredService<IHashService>() //bunu eklemeden önce hata vrdi. çünkü artık servis ctor  5 parametreli. 
                ));

            return services;
        }
    }
}
