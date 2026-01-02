using BCrypt.Net;

namespace ChatBotInpadServer.Services
{
    public class PasswordHasherService
    {
        public static string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool CheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}