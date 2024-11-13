using System.Security.Cryptography;

namespace ProductTrackingAPI.Utils
{
    public class AppPasswordHasher
    {
        public static byte[] salt = Convert.FromBase64String("yvhJaico6px25ucO8+pt5Q==");

        public static string HashPassword(string password)
        {
             // Generate a 16-byte salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            return Convert.ToBase64String(pbkdf2.GetBytes(32)); // Generate a 32-byte hash
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100_000, HashAlgorithmName.SHA256);
            var hash = pbkdf2.GetBytes(32);
            Console.WriteLine($"{Convert.ToBase64String(hash)} == {hashedPassword}");
            return Convert.ToBase64String(hash) == hashedPassword;
        }
    }
}
