﻿using System.Security.Cryptography;
using CarRentalAPI.Abstractions;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CarRentalAPI.Services.PasswordServices
{
    public class Hmacsha256PasswordService : IPasswordService
    {
        public (string hash, string salt) HashPassword(string password)
        {
            byte[] saltBytes = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            string salt = Convert.ToBase64String(saltBytes);
            string hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
            return (hash, salt);
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            try
            {
                string computedHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: password,
                    salt: Convert.FromBase64String(salt),
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8));

                return hash == computedHash;
            }
            catch (ArgumentNullException e)
            {
                return false;
            }
        }
    }
}
