using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Api.Controllers.Base;
using MyApi.Application.DTOs.ARServices.Category;
using MyApi.Application.Features.Identity.Users.Commands.RegisterUser;
using MyApi.Application.Features.Identity.Users.Queries.GetUserById;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ApiControllerBase // ControllerBase yerine ApiControllerBase'den miras al
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
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
        public async Task<IActionResult> GetUserById(GetUSerByIdQuery query)
        {
            var result = await _mediator.Send(query);
            return HandleResult(result);
        }
    }
}
