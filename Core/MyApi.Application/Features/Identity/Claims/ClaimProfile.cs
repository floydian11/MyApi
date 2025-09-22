using AutoMapper;
using MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate;
using MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplate;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Claims.Queries.GetAllClaimTemplates;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims
{
    public class ClaimTemplateProfile : Profile
    {
        public ClaimTemplateProfile()
        {
            // Entity -> DTO
            CreateMap<ClaimTemplate, ClaimTemplateDto>();

            // DTO -> Command
            CreateMap<CreateClaimTemplateDto, CreateClaimTemplateCommand>();
            CreateMap<UpdateClaimTemplateDto, UpdateClaimTemplateCommand>();

            // Command -> Entity
            CreateMap<CreateClaimTemplateCommand, ClaimTemplate>();
            CreateMap<UpdateClaimTemplateCommand, ClaimTemplate>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // Güncellerken ID'yi map'leme

           
        }
    }
}
