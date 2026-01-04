// Services/UserService.cs
using ChatBotInpadserver.Data.DataBase;
using ChatBotInpadServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatBotInpadServer.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Находит или создает пользователя по платформе
        /// </summary>
        public async Task<User> GetOrCreateUserAsync(string platform, string platformId, string? username = null)
        {
            try
            {
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Platform == platform && u.PlatformId == platformId);

                if (existingUser != null)
                {
                    // Обновляем активность и имя
                    existingUser.LastActiveAt = DateTime.UtcNow;
                    if (!string.IsNullOrEmpty(username) && existingUser.UserName != username)
                    {
                        existingUser.UserName = username;
                    }

                    await _context.SaveChangesAsync();
                    return existingUser;
                }

                // Создаем нового пользователя
                var newUser = new User
                {
                    Platform = platform,
                    PlatformId = platformId,
                    UserName = username ?? "Unknown",
                    CreatedAt = DateTime.UtcNow,
                    LastActiveAt = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в UserService.GetOrCreateUserAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Получает пользователя по ID
        /// </summary>
        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        /// <summary>
        /// Получает все сообщения пользователя
        /// </summary>
        public async Task<List<ChatMessage>> GetUserMessagesAsync(int userId, int limit = 50)
        {
            return await _context.ChatMessages
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.CreatedAt)
                .Take(limit)
                .ToListAsync();
        }

        /// <summary>
        /// Сохраняет сообщение в чат
        /// </summary>
        public async Task<ChatMessage> SaveMessageAsync(
            string text,
            string platform,
            string platformId,
            bool isFromUser = true,
            int? knowledgeItemId = null)
        {
            try
            {
                var user = await GetOrCreateUserAsync(platform, platformId);

                var message = new ChatMessage
                {
                    TextMessage = text,
                    Platform = platform,
                    UserId = user.Id,
                    IsFromUser = isFromUser,
                    KnowledgeItemId = knowledgeItemId,
                    CreatedAt = DateTime.UtcNow
                };

                _context.ChatMessages.Add(message);
                await _context.SaveChangesAsync();

                return message;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в UserService.SaveMessageAsync: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Получает статистику пользователей
        /// </summary>
        public async Task<UserStats> GetUserStatsAsync()
        {
            var totalUsers = await _context.Users.CountAsync();
            var activeUsers = await _context.Users
                .Where(u => u.LastActiveAt >= DateTime.UtcNow.AddDays(-30))
                .CountAsync();
            var totalMessages = await _context.ChatMessages.CountAsync();

            var platforms = await _context.Users
                .GroupBy(u => u.Platform)
                .Select(g => new PlatformStats
                {
                    Platform = g.Key,
                    UserCount = g.Count()
                })
                .ToListAsync();

            return new UserStats
            {
                TotalUsers = totalUsers,
                ActiveUsers = activeUsers,
                TotalMessages = totalMessages,
                Platforms = platforms
            };
        }
    }

    public class UserStats
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalMessages { get; set; }
        public List<PlatformStats> Platforms { get; set; } = new();
    }

    public class PlatformStats
    {
        public string Platform { get; set; } = string.Empty;
        public int UserCount { get; set; }
    }
}