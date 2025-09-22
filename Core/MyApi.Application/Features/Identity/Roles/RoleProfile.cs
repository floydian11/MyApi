using AutoMapper;
using MyApi.Application.Features.Identity.Roles.Commands.AddRole;
using MyApi.Application.Features.Identity.Roles.Commands.UpdateRole;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            // Mevcut harita: Entity -> DTO
            CreateMap<AppRole, RoleDto>();

            // YENİ EKLENEN HARİTA: Command -> Entity
            // Property isimleri (Name, Description) birebir aynı olduğu için
            // ek bir konfigürasyona gerek yoktur.
            CreateMap<AddRoleCommand, AppRole>();
            CreateMap<UpdateRoleCommand, AppRole>();
        }
    }
}
