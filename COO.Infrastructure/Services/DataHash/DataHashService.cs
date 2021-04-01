using System;
using System.Security.Cryptography;
using System.Text;

namespace COO.Infrastructure.Services.DataHash
{
    public class DataHashService : IDataHashService
    {
        public string EncryptData(string data, string dataSalt)
        {
            var bytesToCrypt = Encoding.UTF8.GetBytes($"{data}{dataSalt}");

            return string.Concat(Array.ConvertAll(new SHA256Managed().ComputeHash(bytesToCrypt, 0, bytesToCrypt.Length), x => x.ToString("X2")));
        }

        public bool IsCorrectDataHash(string data, string dataHash, string salt)
        {
            return !string.IsNullOrEmpty(data) && !string.IsNullOrEmpty(dataHash) && !string.IsNullOrEmpty(salt)
                && string.Equals(dataHash, EncryptData(data, salt), StringComparison.Ordinal);
        }

        public string GenerateSalt()
        {
            return Guid.NewGuid().ToString("D");
        }
    }
}
