using AutoMapper;
using MyApi.Application.Features.Identity.Authentication.Queries.LoginUser;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Authentication
{
    public class AuthenticationProfile : Profile
    {
        public AuthenticationProfile()
        {
            CreateMap<LoginDto, LoginQuery>();
            CreateMap<AppUser, UserLoginResponseDto>();

        }
    }
}
//"Missing type map configuration or unsupported mapping.
//\r\n\r\nMapping types:\r\nAppUser -> UserLoginResponseDto\r\nMyApi.Domain.Entities.Identity.AppUser ->
//MyApi.Application.Features.Identity.Users.DTOs.UserLoginResponseDto"