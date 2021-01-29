using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace Expenses.Core.Utilities
{
    public static class Hash
    {
        public static string Password(string password)
        {
            byte[] salt = new byte[128 / 8];

            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
        }
    }
}
