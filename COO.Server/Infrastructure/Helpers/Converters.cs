namespace COO.Server.Infrastructure.Helpers
{
    using System;
    using System.Linq;
    using BC = BCrypt.Net.BCrypt;

    public static class Converters
    {
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string CreateHashPassword(string password)
            => BC.HashPassword(password);

        public static bool CheckPassword(string password, string passwordHash)
            => BC.Verify(password, passwordHash);
    }
}
