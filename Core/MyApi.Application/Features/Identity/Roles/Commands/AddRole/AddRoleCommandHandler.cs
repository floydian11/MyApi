using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Features.Identity.Users;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Roles.Commands.AddRole
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public AddRoleCommandHandler(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Result<RoleDto>> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            if (await _roleManager.RoleExistsAsync(request.Name))
                return Result.Failure<RoleDto>(RoleErrors.AlreadyExists);

            //var newRole = new AppRole
            //{
            //    Name = request.Name,
            //    Description = request.Description
            //};
            //yukarıdaki manual mapleme yerine kullanıyoruz. ikisi de kullanılabilir. 
            var newRole = _mapper.Map<AppRole>(request);

            var identityResult = await _roleManager.CreateAsync(newRole);

            if (!identityResult.Succeeded)
            {
                return Result.Failure<RoleDto>(RoleErrors.CreationFailed);
            }

            // 5. İşlem BAŞARILIYSA, en başta oluşturduğumuz 'newRole' nesnesini DTO'ya map'le.
            var roleDto = _mapper.Map<RoleDto>(newRole);

            
            return Result.Success(roleDto);
        }
    }
}
