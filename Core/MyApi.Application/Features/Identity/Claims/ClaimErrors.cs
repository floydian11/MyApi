using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Claims
{
    public class ClaimErrors
    {
        public static readonly Error AlreadyExists = new(
           "Claim.AlreadyExists",// KISIM 1: Hata Kodu (Error Code)Bu bizim Hata Kodumuzdur (Error Code)
           "Bu yetki adı zaten kullanılıyor.",// KISIM 2: Varsayılan Mesaj (Default Message
           ErrorType.Conflict);// KISIM 3: Hata Tipi (Error Type)

        public static readonly Error UserAlreadyInClaim = new(
          "Claim.UserAlreadyInClaim",// KISIM 1: Hata Kodu (Error Code)Bu bizim Hata Kodumuzdur (Error Code)
          "Bu yetki kullanıcıda zaten kullanılıyor.",// KISIM 2: Varsayılan Mesaj (Default Message
          ErrorType.Conflict);// KISIM 3: Hata Tipi (Error Type)

        public static readonly Error AssignFailed = new (
            "Claim.AssignFailed",
            "Yetki kullanıcıya atanırken bir hata oluştu.",
            ErrorType.Failure);

        public static readonly Error RemoveFailed = new(
            "Claim.AssignFailed",
            "Yetki kaldırılırken bir hata oluştu.",
            ErrorType.Failure);

        public static readonly Error NotAssigned = new(
            "Claim.NotAssigned",
            "Kullanıcı bu yetkiye sahip değil.",
            ErrorType.Failure);

        public static readonly Error NotFound = new(
            "Claim.NotFound",
            "Yetki bulunamadı.",
            ErrorType.NotFound);
    }
}
