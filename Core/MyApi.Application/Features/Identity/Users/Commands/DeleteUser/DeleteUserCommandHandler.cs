using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Users.Commands.UpdateUser;
using MyApi.Application.Results;
using MyApi.Application.Results.Eski;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Result = MyApi.Application.Results.Result;

namespace MyApi.Application.Features.Identity.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result>
    {
        private readonly UserManager<AppUser> _userManager;

        public DeleteUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
                return Result.Failure(UserErrors.NotFound);

            var result = await _userManager.DeleteAsync(user);
            if(!result.Succeeded)  
                return Result.Failure(UserErrors.NotFound);
            return Result.Success();
        }
    }
}
