using MyApi.Application.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Features.Identity.Users
{
    public static class UserErrors
    {
        public static readonly Error AlreadyExists = new(
            "User.AlreadyExists",// KISIM 1: Hata Kodu (Error Code)Bu bizim Hata Kodumuzdur (Error Code)
            "Bu kullanıcı adı zaten kullanılıyor.",// KISIM 2: Varsayılan Mesaj (Default Message
            ErrorType.Conflict);// KISIM 3: Hata Tipi (Error Type)

        public static readonly Error EmailInUse = new(
            "User.EmailInUse",
            "Bu e-posta adresi zaten kullanılıyor.",
            ErrorType.Conflict);

        public static readonly Error CreationFailed = new(
            "User.CreationFailed",
            "Kullanıcı oluşturulamadı.",
            ErrorType.Failure);

        public static readonly Error NotFound = new(
            "User.NotFound",
            "Kullanıcı bulunamadı.",
            ErrorType.NotFound);

        public static readonly Error UpdateFailed = new(
            "User.UpdateFailed",
            "Kullanıcı bilgileri güncellenemedi.",
            ErrorType.Failure);

        public static readonly Error DeletionFailed = new(
            "User.DeletionFailed",
            "Kullanıcı silinirken bir hata oluştu.",
            ErrorType.Failure);

        public static readonly Error StatusChangeFailed = new(
           "User.StatusChangeFailed",
           "Kullanıcı durumu değiştirilirken bir hata oluştu.",
           ErrorType.Failure);

        public static readonly Error ListingFailed = new(
         "User.ListingFailed",
         "Kullanıcılar listelenirken bir hata oluştu.",
         ErrorType.Failure);

        public static readonly Error LoginFailed = new(
        "User.LoginFailed",
        "Kullanıcı adı ya da şifre hatalı.",
        ErrorType.Failure);

        public static readonly Error InvalidCredentials = new(
        "User.InvalidCredentials",
        "Kullanıcı adı ya da şifre hatalı.",
        ErrorType.Failure);

        public static readonly Error AccountNotActive = new(
       "User.AccountNotActive",
       "Kullanıcı hesabı aktif değil.",
       ErrorType.Failure);
    }
}
