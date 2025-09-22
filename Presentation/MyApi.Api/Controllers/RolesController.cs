using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Api.Controllers.Base;
using MyApi.Application.Features.Identity.Roles.Commands.AddRole;
using MyApi.Application.Features.Identity.Roles.Commands.DeleteRole;
using MyApi.Application.Features.Identity.Roles.Commands.UpdateRole;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Features.Identity.Roles.Queries.GetRoles.MyApi.Application.Features.Identity.Roles.Queries.GetAllRoles;
using MyApi.Application.Features.Identity.Roles.Queries.GetUserRoles;
using MyApi.Application.Features.Identity.Roles.Queries.GetUsersByRole;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ApiControllerBase // ControllerBase yerine ApiControllerBase'den miras al
    {
        private readonly IMediator _mediator;

        public RolesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("test")] // Bu metodun yolu: /api/roles/test
        [Authorize(Roles = "Admin")]
        public IActionResult TestEndpoint()
        {
            // BREAKPOINT'İ BU SATIRA KOY
            return Ok("Roles Controller çalışıyor!");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllRoles()
        {
            var query = new GetAllRolesQuery();
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpGet("by-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserRoles([FromRoute] Guid userId)
        {
            var query = new GetUserRolesQuery(userId);
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleDto dto)
        {
            // DTO'yu Command'e çeviriyoruz.
            var command = new AddRoleCommand(dto.Name, dto.Description);
            var result = await _mediator.Send(command);
            // Başarılı olursa, HandleResult bunu 201 Created'a çevirebilir (ileride).
            // Şimdilik Ok(result.Value) dönecek.
            return HandleResult(result);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRole([FromRoute] Guid id, [FromBody] UpdateRoleDto dto)
        {
            var command = new UpdateRoleCommand(id, dto.Name, dto.Description);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            var command = new DeleteRoleCommand(id);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpGet("{roleName}/users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsersByRole([FromRoute] string roleName)
        {
            var result = await _mediator.Send(new GetUsersByRoleQuery(roleName));
            return HandleResult(result);
        }
    }
}
