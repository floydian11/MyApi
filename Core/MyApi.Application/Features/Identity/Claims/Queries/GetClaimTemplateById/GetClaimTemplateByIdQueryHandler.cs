using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyApi.Application.DTOs;
using MyApi.Application.Features.Identity.Claims;
using MyApi.Application.Features.Identity.Claims.Commands.CreateClaimTemplate;
using MyApi.Application.Features.Identity.Claims.DTOs;
using MyApi.Application.Features.Identity.Claims.Queries.GetAllClaimTemplates;
using MyApi.Application.Repositories.Claim;
using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims.Queries.GetClaimTemplateById
{
    public class GetClaimTemplateByIdQueryHandler : IRequestHandler<GetClaimTemplateByIdQuery, Result<ClaimTemplateDto>>
    {
        private readonly IClaimTemplateRepository _claimTemplateRepository;
        private readonly IMapper _mapper;
        
        public GetClaimTemplateByIdQueryHandler(IClaimTemplateRepository claimTemplateRepository, IMapper mapper)
        {
            _claimTemplateRepository = claimTemplateRepository;
            _mapper = mapper;
        }

        public async Task<Result<ClaimTemplateDto>> Handle(GetClaimTemplateByIdQuery request, CancellationToken cancellationToken)
        {
            // "Önce Sorguyu Hazırla, Sonra Çalıştır" prensibini uyguluyoruz.
            // Aşağıdaki ilk 3 satır veritabanına gitmez, sadece SQL sorgusunu hazırlar.
            //var claimTemplateDto = await _claimTemplateRepository BU DA AŞAĞIDAKİ GİBİ OLABİLİR
            //    .GetAllAsNoTracking() // 1. Değişiklikleri takip etme, çünkü bu salt okunur bir işlem.
            //    .Where(ct => ct.Id == request.Id) // 2. Gelen request'teki Id'ye göre filtrele.
            //    .Select(ct => new ClaimTemplateDto( // 3. Bulunan sonucu doğrudan DTO'muza yansıt (Project).
            //        ct.Id,
            //        ct.Type,
            //        ct.Value,
            //        ct.Description,
            //        ct.IsActive
            //    ))
                // 4. Hazırlanan bu sorguyu veritabanında asenkron olarak çalıştır ve
                //    ilk bulduğun (veya bulamazsan null olan) sonucu al.
                //.FirstOrDefaultAsync(cancellationToken);


            var claimTemplateDto = await _claimTemplateRepository
            .GetAllAsNoTracking() // IQueryable'ı al
            .Where(ct => ct.Id == request.Id) // Filtrele
            .ProjectTo<ClaimTemplateDto>(_mapper.ConfigurationProvider) // Doğrudan DTO'ya yansıt
            .FirstOrDefaultAsync(cancellationToken); // Sorguyu çalıştır

            // 5. Eğer sorgu sonucu null ise, yani o Id'ye sahip bir kayıt yoksa, NotFound hatası dön.
            if (claimTemplateDto is null) // '== null' ile aynı anlama gelir.
            {
                // ClaimErrors sınıfını oluşturup hataları orada merkezileştirmek en iyisidir.
                return Result.Failure<ClaimTemplateDto>(new Error("Claim.NotFound", "Belirtilen ID'ye sahip yetki tanımı bulunamadı.", ErrorType.NotFound));
            }

            // 6. Eğer kayıt bulunduysa, veritabanından zaten DTO formatında gelen nesneyi başarıyla dön.
            return Result.Success(claimTemplateDto);
        }
    }
}


// DAHA AZ VERİMLİ YÖNTEM
// 1. Önce TÜM entity'yi, tüm sütunlarıyla birlikte veritabanından çek.
//var claimTemplateEntity = await _claimTemplateRepository.GetByIdAsync(request.Id);
//if (claimTemplateEntity == null)
//{
//    return Result.Failure<ClaimTemplateDto>(ClaimErrors.NotFound);
//}

//// 2. Sonra bellekte AutoMapper ile bu entity'yi DTO'ya çevir.
//var claimTemplateDto = _mapper.Map<ClaimTemplateDto>(claimTemplateEntity);

//return Result.Success(claimTemplateDto);