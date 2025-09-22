using AutoMapper;
using MediatR;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplate
{
    public class UpdateClaimTemplateCommandHandler : IRequestHandler<UpdateClaimTemplateCommand, Result<ClaimTemplateDto>>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository; // Generic repo veya özel repo
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateClaimTemplateCommandHandler(IClaimTemplateRepository claimTemplateRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimTemplateRepository = claimTemplateRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ClaimTemplateDto>> Handle(UpdateClaimTemplateCommand request, CancellationToken ct)
        {
            var claimTemplate = await _claimTemplateRepository.GetByIdAsync(request.Id);
            if (claimTemplate == null)
                return Result.Failure<ClaimTemplateDto>(ClaimErrors.NotFound);

            var exists = await _claimTemplateRepository.AnyAsync(c =>
                c.Type == request.Type &&
                c.Value == request.Value &&
                c.Id != request.Id); // <-- Kendisi hariç başka bir kayıtta var mı?

            if (exists)
                return Result.Failure<ClaimTemplateDto>(ClaimErrors.AlreadyExists);

            _mapper.Map(request, claimTemplate); // Var olanı güncelle

            _claimTemplateRepository.Update(claimTemplate);
            await _unitOfWork.CommitAsync();

            var dto = _mapper.Map<ClaimTemplateDto>(claimTemplate);
            return Result.Success(dto);
        }
    }
}
