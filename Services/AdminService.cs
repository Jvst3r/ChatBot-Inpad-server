using ChatBotInpadserver.Data.DataBase;
using ChatBotInpadserver.Data.DTOs.WebClientDTOs;
using ChatBotInpadServer.Data.Models;
using ChatBotInpadServer.Services;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Payments;

namespace ChatBot_Inpad_server.Services
{
    public class AdminService
    {
        private readonly AppDbContext db;
        private readonly PasswordHasherService hasher;
        public AdminService(AppDbContext _db, PasswordHasherService _hasher)
        {
            db = _db;
            hasher = _hasher;
        }

        public async Task<LoginAdminResponse> LoginAsync(string Email, string Password)
        {
            try
            {
                if (Email == null || Password == null)
                    return new LoginAdminResponse() { Success = false, Message = "Не указана почта или пароль!" };

                Admin admin = await db.Admins.FirstOrDefaultAsync(a => a.Email == Email);

                if (admin == null)
                    return new LoginAdminResponse() { Success = false, Message = "Указанный Email не зарегистрирован!" };

                if (!hasher.CheckPassword(Password, admin.PasswordHash))
                    return new LoginAdminResponse() { Success = false, Message = "Неверный пароль" };

                admin.LastLoginAt = DateTime.UtcNow;
                await db.SaveChangesAsync();
                return new LoginAdminResponse() { Success = true, Message ="Успешный вход. Добро пожаловать!", Email = Email };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ex.Message\n ОШИБКА АВТОРИЗАЦИИ ПОЛЬЗОВАТЕЛЯ");
                return new LoginAdminResponse() { Success = false, Message = "Возникла непредвиденная ошибка" };
            }

            
        }

        public async Task<Admin?> GetAdminByEmailAsync(string email)
        {
            return await db.Admins
                .FirstOrDefaultAsync(a => a.Email == email);
        }

        public async Task<bool> ChangePasswordAsync(string email, string oldPassword, string newPassword)
        {
            var admin = await GetAdminByEmailAsync(email);
            if (admin == null || !hasher.CheckPassword(oldPassword, admin.PasswordHash))
                return false;

            admin.PasswordHash = hasher.GetHash(newPassword);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Admin> CreateAdminAsync(string email, string password)
        {
            var admin = new Admin
            {
                Email = email,
                PasswordHash = hasher.GetHash(password),
                LastLoginAt = null
            };

            db.Admins.Add(admin);
            await db.SaveChangesAsync();
            return admin;
        }

        public async Task<bool> AdminExistsAsync(string email)
                                                => await db.Admins.AnyAsync(a => a.Email == email);
        
        public async Task<List<Admin>> GetAllAdminsAsync()
                                                => await db.Admins.ToListAsync();
        
    }
}
