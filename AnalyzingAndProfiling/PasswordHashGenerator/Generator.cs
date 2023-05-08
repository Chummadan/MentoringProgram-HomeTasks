using System.Security.Cryptography;
using System.Text;

namespace PasswordHashGenerator
{
    public class Generator
    {
        public static string GeneratePasswordHashUsingSalt(string passwordText, byte[] salt)
        {
            var hashLength = 20;
            var iterate = 10000;

            var pbkdf2 = new Rfc2898DeriveBytes(passwordText, salt, iterate);

            byte[] hash = pbkdf2.GetBytes(hashLength);
            pbkdf2.Dispose();

            byte[] hashBytes = new byte[16 + hashLength];

            Buffer.BlockCopy(salt, 0, hashBytes, 0, 16);
            Buffer.BlockCopy(hash, 0, hashBytes, 16, hashLength);

            var passwordHashBuilder = new StringBuilder(Convert.ToBase64String(hashBytes), 64);

            return passwordHashBuilder.ToString();
        }
    }
}
