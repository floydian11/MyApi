using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Api.Controllers.Base;
using MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate;
using MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplate;
using MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplateStatusCommand;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Claims.Queries.GetAllClaimTemplates;
using MyApi.Application.Features.Identity.Claims.Queries.GetClaimTemplateById;

namespace MyApi.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ClaimTemplatesController : ApiControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ClaimTemplatesController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        /// <summary>
        /// Sistemde tanımlı tüm yetki şablonlarını listeler.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllClaimTemplatesQuery());
            return HandleResult(result);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip tek bir yetki şablonunu getirir.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetClaimTemplateByIdQuery(id));
            return HandleResult(result);
        }

        /// <summary>
        /// Sisteme yeni bir yetki şablonu ekler.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClaimTemplateDto dto)
        {
            var command = _mapper.Map<CreateClaimTemplateCommand>(dto);
            var result = await _mediator.Send(command);

            return result.IsSuccess
                ? CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value)
                : HandleResult(result);
        }

        /// <summary>
        /// Mevcut bir yetki şablonunu günceller.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateClaimTemplateDto dto)
        {
            var command = _mapper.Map<UpdateClaimTemplateCommand>(dto) with { Id = id };
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }

        /// <summary>
        /// Bir yetki şablonunu pasif hale getirir (Soft Delete).
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // "Silme" işlemi için, IsActive durumunu 'false' yapan Command'i gönderiyoruz.
            var command = new UpdateClaimTemplateStatusCommand(id, IsActive: false);
            var result = await _mediator.Send(command);
            return HandleResult(result); // Başarılıysa 204 NoContent döner
        }

        /// <summary>
        /// Pasif bir yetki şablonunu aktif hale getirir.
        /// </summary>
        [HttpPatch("{id}/activate")]
        public async Task<IActionResult> Activate([FromRoute] Guid id)
        {
            var command = new UpdateClaimTemplateStatusCommand(id, IsActive: true);
            var result = await _mediator.Send(command);
            return HandleResult(result);
        }
    }
}