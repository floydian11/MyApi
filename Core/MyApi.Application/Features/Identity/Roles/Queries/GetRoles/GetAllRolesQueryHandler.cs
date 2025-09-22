using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Features.Identity.Roles.Queries.GetRoles.MyApi.Application.Features.Identity.Roles.Queries.GetAllRoles;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Queries.GetRoles
{
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<List<RoleDto>>>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public GetAllRolesQueryHandler(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Result<List<RoleDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            // 1. RoleManager üzerinden tüm rolleri veritabanından çek.
            var roles = await _roleManager.Roles.ToListAsync(cancellationToken);

            // 2. Çekilen AppRole listesini, RoleDto listesine AutoMapper ile map'le.
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            // 3. Başarılı sonucu ve DTO listesini dön.
            // Eğer hiç rol yoksa, boş bir liste döner, bu bir hata değildir.
            return Result.Success(roleDtos);
        }
    }
}
