using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetAllClaimTemplates
{
    public class GetAllClaimTemplatesQueryHandler : IRequestHandler<GetAllClaimTemplatesQuery, Result<List<ClaimTemplateDto>>>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository;

        public GetAllClaimTemplatesQueryHandler(IClaimTemplateRepository claimTemplateRepository)
        {
            _claimTemplateRepository = claimTemplateRepository;
        }

        public async Task<Result<List<ClaimTemplateDto>>> Handle(GetAllClaimTemplatesQuery request, CancellationToken cancellationToken)
        {
            // 1. Veritabanı sorgusunu bir IQueryable olarak al (bu aşamada veritabanına henüz gidilmez).
            var claimsQueryable = _claimTemplateRepository.GetAllAsNoTracking();

            // 2. Veritabanına "Sadece ihtiyacım olan alanları seçerek yeni bir ClaimTemplateDto oluştur"
            //    komutunu veriyoruz. Bu işleme "Projection" denir.
            var claimDtos = await claimsQueryable
                .Select(ct => new ClaimTemplateDto( // DTO'muz record olduğu için bu kısa kullanımı yapabiliriz
                    ct.Id,
                    ct.Type,
                    ct.Value,
                    ct.Description,
                    ct.IsActive
                ))
                // 3. Sorguyu veritabanında ASENKRON olarak çalıştır ve sonuçları doğrudan bir listeye çevir.
                .ToListAsync(cancellationToken);

            // 4. Artık elimizde doğrudan DTO listesi var. Task.FromResult'a gerek yok.
            return Result.Success(claimDtos);
        }
    }
}
