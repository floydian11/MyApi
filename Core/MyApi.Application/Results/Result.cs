using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.Application.Results
{
    // Veri döndürmeyen (void) işlemler için temel Result sınıfı
    // Veri döndürmeyen (void) işlemler için temel Result sınıfı
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;

        // Başarılı durumda Error null olur.
        public Error? Error { get; }

        // (A) public string? Message { get; } // YENİ EKLENEN PROPERTY - Eğer başarılı sonuçlara mesaj eklemek isterseniz kullanabilirsiniz.
        // (B)protected Result(bool isSuccess, Error? error, string? message = null) // Constructor'a message eklendi Eğer A'daki mesajı kullanırsak ctor böyle olacak
        //{
        //    // ... (constructor içindeki kontrol mantığı aynı)
        //    IsSuccess = isSuccess;
        //    Error = error;
        //    Message = message; // Değeri ata
        //}
        // Constructor'ı 'protected' yaparak new Result() kullanımını engelliyoruz.
        // Bunun yerine aşağıdaki Success() ve Failure() metodlarını kullanmaya zorluyoruz.
        protected Result(bool isSuccess, Error? error, string? message = null) // Constructor'a message eklendi
    {
        // ... (constructor içindeki kontrol mantığı aynı)
        IsSuccess = isSuccess;
        Error = error;
            //Message = message; // (C) eğer A ve B'deki mesaj yapısı kullanılırsa bu satır aktif olur.
        }

        protected Result(bool isSuccess, Error? error)
        {
            // Başarılı bir sonuçta Error olamaz, başarısız bir sonuçta Error olmalı.
            if (isSuccess && error is not null || !isSuccess && error is null)
            {
                throw new ArgumentException("Geçersiz işlem.", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        // Başarılı bir Result nesnesi oluşturmak için fabrika metodu.
        public static Result Success() => new(true, null);

        // Başarısız bir Result nesnesi oluşturmak için fabrika metodu.
        public static Result Failure(Error error) => new(false, error);

        // Generic versiyonlar için de fabrika metodları ekleyelim.
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, null);
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        //FABRIKA METOTLARI A'DAKİ MESAJ YAPISINI KULLANIRSA
        //    public static Result Success(string? message = null) => new(true, null, message);
        //    public static Result Failure(Error error) => new(false, error, error.Message);

        //    public static Result<TValue> Success<TValue>(TValue value, string? message = null) => new(value, true, null, message);
        //    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error, error.Message);
    }

    // Veri döndüren işlemler için generic Result sınıfı
    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        // Başarılı durumda veri (value) dolu olmalıdır.
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("Başarısız bir sonucun değeri olamaz.");

        protected internal Result(TValue? value, bool isSuccess, Error? error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        //protected internal Result(TValue? value, bool isSuccess, Error? error, string? message = null)
        //: base(isSuccess, error, message) MESAJ VERİLECEKSA A'daki GİBİ MESAJI BASE'E GÖNDER
    }
}
