using AutoMapper;
using MyApi.Application.DTOs.ExternalServices.Account;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Mapping.ExternalServices.Account
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            CreateMap<RegisterDto, AppUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())           // Guid Identity tarafından atanacak
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Password Identity tarafından yönetiliyor
                .ForMember(dest => dest.TCKNHash, opt => opt.Ignore())    // Hash servis ile set edilecek
                .ForMember(dest => dest.TCKNSalt, opt => opt.Ignore())    // Hash servis ile set edilecek
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            // AppUser -> UserResponseDto
            CreateMap<AppUser, UserResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())   // Manuel olarak set edilecek
                .ForMember(dest => dest.Claims, opt => opt.Ignore()); // Manuel olarak set edilecek

            // AppUser -> UserLoginResponseDto
            CreateMap<AppUser, UserLoginResponseDto>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Claims, opt => opt.Ignore());
        }
    }
}
