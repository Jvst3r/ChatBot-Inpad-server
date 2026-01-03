using Microsoft.EntityFrameworkCore;
using ChatBotInpadServer.Data.Models;
using ChatBotInpadServer.Services;

namespace ChatBotInpadserver.Data.DataBase
{

    //Контекст базы данных
    //Здесь написано какие таблицы будут в БД и чем будут заполнены
    public class AppDbContext : DbContext
    {
        //тут если честно не знаю что указывать,
        //по моему можно игнорировать некоторые значения из моделей и пропускать их
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        //Таблицы, их названия и модели, по которым они будут построены
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<KnowledgeItem> KnowledgeItems { get; set; }

        public DbSet<Admin> Admins { get; set; }


        //Конфигурация каждой из таблиц БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация Admin
            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasKey(e => e.Email);

                entity.HasIndex(e => e.Email)
                      .IsUnique();

                entity.Property(e => e.Email)
                      .HasMaxLength(255)
                      .IsRequired();

                entity.Property(e => e.PasswordHash)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(e => e.LastLoginAt)
                      .IsRequired(false);
            });

            // Конфигурация User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Уникальная комбинация платформы и ID на платформе
                entity.HasIndex(e => new { e.Platform, e.PlatformId })
                      .IsUnique()
                      .HasDatabaseName("IX_User_Platform_PlatformId");

                entity.Property(e => e.Platform)
                      .HasMaxLength(50)
                      .IsRequired()
                      .HasDefaultValue("Telegram");

                entity.Property(e => e.PlatformId)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.UserName)
                      .HasMaxLength(100)
                      .IsRequired(false);

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.LastActiveAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Связь с ChatMessages
                entity.HasMany(e => e.ChatMessages)
                      .WithOne(e => e.User)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Конфигурация ChatMessage
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Индексы для оптимизации запросов
                entity.HasIndex(e => e.UserId)
                      .HasDatabaseName("IX_ChatMessages_UserId");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_ChatMessages_CreatedAt");

                entity.HasIndex(e => e.KnowledgeItemId)
                      .HasDatabaseName("IX_ChatMessages_KnowledgeItemId");

                entity.HasIndex(e => e.Platform)
                      .HasDatabaseName("IX_ChatMessages_Platform");

                entity.HasIndex(e => e.IsFromUser)
                      .HasDatabaseName("IX_ChatMessages_IsFromUser");

                // Составные индексы для частых запросов
                entity.HasIndex(e => new { e.UserId, e.CreatedAt })
                      .HasDatabaseName("IX_ChatMessages_User_Time");

                entity.HasIndex(e => new { e.Platform, e.CreatedAt })
                      .HasDatabaseName("IX_ChatMessages_Platform_Time");

                // Ограничения полей
                entity.Property(e => e.TextMessage)
                      .IsRequired()
                      .HasMaxLength(2000);

                entity.Property(e => e.Platform)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Telegram");

                entity.Property(e => e.UserId)
                      .IsRequired(false);

                entity.Property(e => e.KnowledgeItemId)
                      .IsRequired(false);

                entity.Property(e => e.IsFromUser)
                      .IsRequired()
                      .HasDefaultValue(true);

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");

                // Внешние ключи
                entity.HasOne(e => e.User)
                      .WithMany(u => u.ChatMessages)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.KnowledgeItem)
                      .WithMany(k => k.ChatMessages)
                      .HasForeignKey(e => e.KnowledgeItemId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Конфигурация KnowledgeItem
            modelBuilder.Entity<KnowledgeItem>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Индексы для быстрого поиска
                entity.HasIndex(e => e.Category)
                      .HasDatabaseName("IX_KnowledgeItems_Category");

                entity.HasIndex(e => e.UseCount)
                      .HasDatabaseName("IX_KnowledgeItems_UseCount");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_KnowledgeItems_CreatedAt");

                entity.HasIndex(e => e.UpdatedAt)
                      .HasDatabaseName("IX_KnowledgeItems_UpdatedAt");

                // Составные индексы для частых запросов
                entity.HasIndex(e => new { e.Category, e.UseCount })
                      .HasDatabaseName("IX_KnowledgeItems_Category_UseCount");

                entity.HasIndex(e => new { e.Category, e.CreatedAt })
                      .HasDatabaseName("IX_KnowledgeItems_Category_CreatedAt");

                // Ограничения полей
                entity.Property(e => e.Category)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasDefaultValue("Общие вопросы");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.AnswerText)
                      .IsRequired()
                      .HasColumnType("text");

                entity.Property(e => e.Tags)
                      .HasMaxLength(500)
                      .HasDefaultValue("revit");

                entity.Property(e => e.UseCount)
                      .IsRequired()
                      .HasDefaultValue(0);

                entity.Property(e => e.CreatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UpdatedAt)
                      .IsRequired()
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                // Связь с ChatMessages
                entity.HasMany(e => e.ChatMessages)
                      .WithOne(e => e.KnowledgeItem)
                      .HasForeignKey(e => e.KnowledgeItemId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Заполнение начальными данными
            SeedData(modelBuilder);
        }

        //заполнение БД при включении сервера
        private void SeedData(ModelBuilder modelBuilder)
        {
            PasswordHasherService passwordHasherService = new PasswordHasherService();
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Email = "admin@example.com",
                    PasswordHash = passwordHasherService.GetHash("Password"),
                    LastLoginAt = null
                });
            modelBuilder.Entity<KnowledgeItem>().HasData(
                new KnowledgeItem
                {
                    Id = 1,
                    Category = "Общие вопросы",
                    Title = "Как начать работу с ботом?",
                    AnswerText = "Для начала работы просто отправьте любое сообщение боту. Он ответит на ваши вопросы и поможет с настройками.",
                    Tags = "начало,помощь,бот",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 2,
                    Category = "Revit",
                    Title = "Как создать новую стену в Revit?",
                    AnswerText = "1. Откройте вкладку 'Architecture'\n2. Нажмите на инструмент 'Wall'\n3. Выберите тип стены\n4. Укажите точки размещения на плане",
                    Tags = "revit,стена,создание",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 3,
                    Category = "Безопасность",
                    Title = "Безопасное подключение",
                    AnswerText = "🔒 Всегда проверяйте сертификаты сервера перед подключением к VPN.",
                    Tags = "vpn,безопасность,сертификаты",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 4,
                    Category = "Производительность",
                    Title = "Оптимальный сервер",
                    AnswerText = "🚀 Выбирайте сервер ближайший к вашему местоположению для лучшей скорости.",
                    Tags = "vpn,сервер,скорость,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 5,
                    Category = "Безопасность",
                    Title = "Wi-Fi защита",
                    AnswerText = "📡 Всегда используйте VPN при подключении к публичным Wi-Fi сетям.",
                    Tags = "vpn,безопасность,wi-fi,публичные сети",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 6,
                    Category = "Производительность",
                    Title = "Скорость соединения",
                    AnswerText = "📊 Если скорость низкая, попробуйте переключиться на другой сервер или протокол.",
                    Tags = "vpn,скорость,сервер,протокол,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 7,
                    Category = "Общие советы",
                    Title = "Включение акселератора",
                    AnswerText = "📱 Мой совет это - включить акселератор.",
                    Tags = "советы,производительность,акселератор,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 8,
                    Category = "Общие советы",
                    Title = "Проверка диска",
                    AnswerText = "📱 Можете проверить диск C.",
                    Tags = "советы,диск,память,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 9,
                    Category = "Общие советы",
                    Title = "Закрытие вкладок",
                    AnswerText = "📱 Попробуйте закрыть вкладки браузера.",
                    Tags = "советы,браузер,память,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 10,
                    Category = "Revit",
                    Title = "Проверка рабочих наборов",
                    AnswerText = "📱 Незабудьте проверить рабочие наборы.",
                    Tags = "revit,советы,рабочие наборы,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 11,
                    Category = "Revit",
                    Title = "Удаление неиспользуемых элементов",
                    AnswerText = "📱 Советую удалить неиспользуемые группы, виды и DWG.",
                    Tags = "revit,советы,очистка,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 12,
                    Category = "Revit",
                    Title = "Проверка веса файла",
                    AnswerText = "📱 Так же советую проверить вес файла хранилища.",
                    Tags = "revit,советы,память,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 13,
                    Category = "Revit",
                    Title = "Отключение аналитики",
                    AnswerText = "📱 Отключите аналитические данные.",
                    Tags = "revit,советы,аналитика,оптимизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new KnowledgeItem
                {
                    Id = 14,
                    Category = "Revit",
                    Title = "Корпоративные семейства",
                    AnswerText = "📱 Используйте корпоративные семейства.",
                    Tags = "revit,советы,семейства,стандартизация",
                    UseCount = 0,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                }
            );


        }
    }
}
