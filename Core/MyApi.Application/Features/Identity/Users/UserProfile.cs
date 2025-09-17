using AutoMapper;
using MyApi.Application.Features.Identity.Users.Commands.RegisterUser;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // 1. RegisterUserCommand -> AppUser Haritası (Handler içinde kullanılacak)
            // Bu, bizim hatamızı düzelten haritanın yeni ve doğru yeri.
            CreateMap<RegisterUserCommand, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.TCKNHash, opt => opt.Ignore())
                .ForMember(dest => dest.TCKNSalt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // CreatedAt'i SaveChanges'te dolduruyoruz.

            // 2. AppUser -> UserResponseDto Haritası (Handler içinde kullanılacak)
            CreateMap<AppUser, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Claims, opt => opt.Ignore());// Rolleri ve claimleri manuel dolduruyoruz.
        }
    }
}
