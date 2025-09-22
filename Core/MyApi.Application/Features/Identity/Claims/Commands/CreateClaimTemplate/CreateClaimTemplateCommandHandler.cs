using AutoMapper;
using MediatR;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate
{
    public class CreateClaimTemplateCommandHandler : IRequestHandler<CreateClaimTemplateCommand, Result<ClaimTemplateDto>>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository; // Generic repo veya özel repo
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateClaimTemplateCommandHandler(IClaimTemplateRepository claimTemplateRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _claimTemplateRepository = claimTemplateRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<ClaimTemplateDto>> Handle(CreateClaimTemplateCommand request, CancellationToken ct)
        {
            // Zaten var mı diye kontrol et (Configuration'daki unique index'e ek olarak)
            var exists = await _claimTemplateRepository.AnyAsync(c => c.Type == request.Type && c.Value == request.Value);
            if (exists)
                return Result.Failure<ClaimTemplateDto>(ClaimErrors.AlreadyExists);

            var claimTemplate = _mapper.Map<ClaimTemplate>(request);

            await _claimTemplateRepository.AddAsync(claimTemplate);
            await _unitOfWork.CommitAsync();

            var dto = _mapper.Map<ClaimTemplateDto>(claimTemplate);
            return Result.Success(dto);
        }
    }
}
