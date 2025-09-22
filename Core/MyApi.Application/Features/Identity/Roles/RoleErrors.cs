using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Roles
{
    public class RoleErrors
    {
        public static readonly Error AlreadyExists = new(
           "Role.AlreadyExists",// KISIM 1: Hata Kodu (Error Code)Bu bizim Hata Kodumuzdur (Error Code)
           "Bu rol adı zaten kullanılıyor.",// KISIM 2: Varsayılan Mesaj (Default Message
           ErrorType.Conflict);// KISIM 3: Hata Tipi (Error Type)
             
        public static readonly Error CreationFailed = new(
            "Role.CreationFailed",
            "Rol oluşturulamadı.",
            ErrorType.Failure);

        //public static readonly Error NotFound = new(
        //    "Role.NotFound",
        //    "Rol bulunamadı.",
        //    ErrorType.NotFound);

        public static readonly Error UpdateFailed = new(
            "Role.UpdateFailed",
            "Rol bilgileri güncellenemedi.",
            ErrorType.Failure);

        //public static readonly Error DeletionFailed = new(
        //    "Role.DeletionFailed",
        //    "Rol silinirken bir hata oluştu.",
        //    ErrorType.Failure);

        public static readonly Error UserAlreadyInRole = new(
           "Role.UserAlreadyInRole",
           "Kullanıcı zaten bu role sahip.",
           ErrorType.Conflict);

        public static readonly Error AssignFailed = new(
            "Role.AssignFailed",
            "Kullanıcıya rol atanırken bir hata oluştu.",
            ErrorType.Failure);

        public static readonly Error UserNotInRole = new(
            "Role.UserNotInRole",
            "Bu rol ilgili kullanıcıya atanmamış.",
            ErrorType.Failure);

        public static readonly Error RemovalFailed = new(
            "Role.RemovalFailed",
            "Atanmış rol silinirken bir hata oluştu.",
            ErrorType.Failure);

        public static readonly Error RoleInUse = new(
           "Role.RoleInUse",
           "Bu rol bir veya daha fazla kullanıcıya atanmış olduğu için silinemez.",
           ErrorType.Failure);

        public static readonly Error HasClaims = new(
           "Role.HasClaims",
           "Bu rol, yetkiler içerdiği için silinemez. Önce yetkileri kaldırın.",
           ErrorType.Failure);

        // Statik hatalar 'readonly field' olarak kalabilir.
        public static readonly Error NotFound = new("Role.NotFound", "Rol bulunamadı.", ErrorType.NotFound);
        public static readonly Error DeletionFailed = new("Role.DeletionFailed", "Rol silinirken bir hata oluştu.", ErrorType.Failure);

        // Dinamik hatalar ise, parametre alan bir 'static method' olur.
        public static Error InUseByUser(string userNames) => new Error(
            "Role.InUseByUser",
            "Bu rol, '{0}' kullanıcısı/kullanıcıları tarafından kullanıldığı için silinemez.", // <-- Mesaj şablonu
            ErrorType.Conflict,
            userNames); // <-- Dinamik parametre
    }
}
