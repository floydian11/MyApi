using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using MyApi.Application.Features.Identity.Roles.DTOs;
using MyApi.Application.Results;
using MyApi.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles.Commands.UpdateRole
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<RoleDto>>
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public UpdateRoleCommandHandler(RoleManager<AppRole> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Result<RoleDto>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            // 1. Güncellenecek rolü veritabanında ID'si ile bul.
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                // Eğer rol bulunamazsa, NotFound hatası dön.
                return Result.Failure<RoleDto>(RoleErrors.NotFound);
            }

            // 2. KRİTİK KONTROL: Yeni rol adı başka bir rol tarafından kullanılıyor mu?
            // Rolün adını değiştirmeye çalışıyorsak bu kontrolü yapmalıyız.
            var existingRoleWithName = await _roleManager.FindByNameAsync(request.Name);
            if (existingRoleWithName != null && existingRoleWithName.Id != request.Id)
            {
                // Eğer yeni isimde bir rol varsa VE bu rol bizim şu an güncellediğimiz rolden
                // farklı bir rol ise, bu bir çakışmadır (conflict).
                return Result.Failure<RoleDto>(RoleErrors.AlreadyExists);
            }

            // 3. Değişiklikleri, veritabanından çektiğimiz entity'e uygula.
            role.Name = request.Name;
            role.Description = request.Description;

            // Manuel atamalar yerine tek satırlık AutoMapper çağrısı. YUKARIDAKİ YA DA BU KULLANILABİLİR.
            // _mapper.Map(request, role);

            // 4. RoleManager ile güncelleme işlemini yap.
            var identityResult = await _roleManager.UpdateAsync(role);

            // 5. İşlemin başarılı olup olmadığını kontrol et.
            if (!identityResult.Succeeded)
            {
                return Result.Failure<RoleDto>(RoleErrors.UpdateFailed); // UserErrors'a benzer bir RoleErrors sınıfı olmalı
            }

            // 6. Başarılı ise, güncellenmiş 'role' nesnesini RoleDto'ya map'le.
            var updatedRoleDto = _mapper.Map<RoleDto>(role);

            // 7. Başarılı sonucu ve güncel DTO'yu dön.
            return Result.Success(updatedRoleDto);
        }
    }
}
