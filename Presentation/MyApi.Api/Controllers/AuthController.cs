using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Api.Controllers.Base;
using MyApi.Application.Features.Identity.Authentication.Commands.Logout;
using MyApi.Application.Features.Identity.Authentication.Commands.RefreshToken;
using MyApi.Application.Features.Identity.Authentication.Queries.LoginUser;
using MyApi.Application.Features.Identity.Users.DTOs;

//using MyApi.Application.Services.Abstract.Account;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        public AuthController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("logout")]
        //[Authorize] // Sadece giriş yapmış bir kullanıcı kendi oturumunu sonlandırabilir.
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command)
        {
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            // 1. API Kontratı olan DTO'yu, Application katmanının iş emri olan Query'ye çevir.
            var query = _mapper.Map<LoginQuery>(dto);

            // 2. Query'yi MediatR'a gönder.
            var result = await _mediator.Send(query);

            // 3. Dönen sonucu standart HandleResult metodumuzla HTTP cevabına dönüştür.
            return HandleResult(result);
        }
    }
}
