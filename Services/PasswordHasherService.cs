using BCrypt.Net;

namespace ChatBotInpadServer.Services
{
    public class PasswordHasherService
    {
        public string GetHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool CheckPassword(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}