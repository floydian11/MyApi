using AutoMapper;
using Azure.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Api.Controllers.Base;
using MyApi.Application.DTOs.ARServices.Category;
using MyApi.Application.Features.Identity.Claims.Commands.AddClaimToUser;
using MyApi.Application.Features.Identity.Claims.Commands.RemoveClaimFromUser;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Roles.Commands.AssignRoleToUser;
using MyApi.Application.Features.Identity.Roles.Commands.RemoveRoleFromUser;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Features.Identity.Roles.Queries.GetUserRoles;
using MyApi.Application.Features.Identity.Roles.Queries.GetUsersByRole;
using MyApi.Application.Features.Identity.Users.Commands.ActivateUser;
using MyApi.Application.Features.Identity.Users.Commands.DeactivateUser;
using MyApi.Application.Features.Identity.Users.Commands.DeleteUser;
using MyApi.Application.Features.Identity.Users.Commands.RegisterUser;
using MyApi.Application.Features.Identity.Users.Commands.UpdateUser;
using MyApi.Application.Features.Identity.Users.DTOs;
using MyApi.Application.Features.Identity.Users.Queries.GetAllUsers;
using MyApi.Application.Features.Identity.Users.Queries.GetAllUsersForList;
using MyApi.Application.Features.Identity.Users.Queries.GetUserById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase // ControllerBase yerine ApiControllerBase'den miras al
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")] // Route'u api/users/register olarak belirliyoruz.
        // Dışarıdan gelen isteğin gövdesini (body) doğrudan RegisterUserCommand nesnesine map'liyoruz.
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            // Komutu MediatR'a gönderiyoruz. MediatR doğru Handler'ı bulup çalıştıracak.
            var result = await _mediator.Send(command);

            // Base class'ımızdaki yardımcı metodu kullanarak sonucu uygun HTTP cevabına dönüştürüyoruz.
            return HandleResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var query = new GetUserByIdQuery(id); // Query'yi burada oluştur
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("getallusers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery(); // Query'yi burada oluştur
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("getallusersforlist")]
        public async Task<IActionResult> GetAllUsersForList()
        {
            var query = new GetAllUsersForListQuery(); // Query'yi burada oluştur
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPatch("deactivate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeactivateUser([FromRoute] Guid id)
        {
            var command = new DeactivateUserCommand(id); // Command'i URL'deki id ile oluştur
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPatch("activate/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateUser([FromRoute] Guid id)
        {
            var command = new ActivateUserCommand(id); // Command'i URL'deki id ile oluştur
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
        {
            var command = new DeleteUserCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromRoute] Guid id, [FromBody] UpdateUserDto dto)
        {
            // MONTAJ İŞLEMİ:
            // Route'dan gelen 'id' + Body'den gelen 'dto' -> Tek bir 'command'
            var command = _mapper.Map<UpdateUserCommand>(dto) with { Id = id };

            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("by-user/{userId}")]
        [Authorize(Roles = "Admin")]
        //[Authorize]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetUserRolesQuery(userId));
            return HandleResult(result);
        }

        

        [HttpPost("{userId}/roles")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoleToUser([FromRoute] Guid userId, [FromBody] AssignRoleToUserDto dto)
        {
            var command = new AssignRoleToUserCommand(userId, dto.RoleName);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{userId}/roles/{roleName}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveRoleFromUser([FromRoute] Guid userId, [FromRoute] string roleName)
        {
            var command = new RemoveRoleFromUserCommand(userId, roleName);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("{userId}/claims")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddClaimToUser([FromRoute] Guid userId, [FromBody] AddClaimToUserDto dto)
        {
            var command = new AddClaimToUserCommand(userId, dto.ClaimType, dto.ClaimValue);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Belirtilen kullanıcının bir yetkisini (claim) kaldırır.
        /// </summary>
        [HttpDelete("{userId}/claims")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveClaimFromUser([FromRoute] Guid userId, [FromBody] RemoveClaimFromUserDto dto)
        {
            var command = new RemoveClaimFromUserCommand(userId, dto.ClaimType, dto.ClaimValue);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}
