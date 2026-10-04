using Microsoft.EntityFrameworkCore;
using PolyOmnia.Models;
using System.Security.Cryptography;
using System.Text;

namespace PolyOmnia.Repository
{
    public class PasswordHasher : IPasswordHasher
    {
        public (string Hash, string Salt) HashPassword(string password)
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
            string salt = Convert.ToHexString(saltBytes);

            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + password);
            string hash = Convert.ToHexString(SHA256.HashData(passwordBytes));
            return (hash, salt);
        }

        public bool VerifyPassword(string password, string hash, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + password);
            string _hash = Convert.ToHexString(SHA256.HashData(passwordBytes));
            return _hash == hash;
        }
    }
}