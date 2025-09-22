using MediatR;
using MyApi.Application.Interfaces;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Commands.UpdateClaimTemplateStatusCommand
{
    public class UpdateClaimTemplateStatusCommandHandler : IRequestHandler<UpdateClaimTemplateStatusCommand, Result>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository; // Generic repo veya özel repo
        private readonly IUnitOfWork _unitOfWork;

        public UpdateClaimTemplateStatusCommandHandler(IClaimTemplateRepository claimTemplateRepository, IUnitOfWork unitOfWork)
        {
            _claimTemplateRepository = claimTemplateRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(UpdateClaimTemplateStatusCommand request, CancellationToken ct)
        {
            var claimTemplate = await _claimTemplateRepository.GetByIdAsync(request.Id);
            if (claimTemplate == null)
                return Result.Failure(ClaimErrors.NotFound);

            claimTemplate.IsActive = request.IsActive;

            _claimTemplateRepository.Update(claimTemplate);
            await _unitOfWork.CommitAsync();

            return Result.Success();
        }
    }
}
