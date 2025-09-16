using MyApi.Application.Services.OuterServices.Identity.Hash;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApi.ExternalServices.Hash
{
    public class HashService : IHashService
    {
        public (byte[] Hash, byte[] Salt) HashValue(string value)
        {
            var salt = new byte[16];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(value),
                salt,
                iterations: 100_000,
                HashAlgorithmName.SHA256,
                32);

            return (hash, salt);
        }

        public bool VerifyHash(string value, byte[] salt, byte[] hash)
        {
            var computedHash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(value),
            salt,
            iterations: 100_000,
            HashAlgorithmName.SHA256,
            32);

            return computedHash.SequenceEqual(hash);
        }
    }
}
