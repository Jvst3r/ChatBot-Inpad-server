using Microsoft.EntityFrameworkCore;
using ChatBot_Inpad_server.Data.Models;

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


        //Конфигурация каждой из таблиц БД
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {

            });

            modelBuilder.Entity<ChatMessage>(entity =>
            {
                // Индексы для быстрого поиска
                entity.HasIndex(e => e.UserId)
                      .HasDatabaseName("UserId");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("CreatedAtTime");

                entity.HasIndex(e => new { e.UserId, e.CreatedAt })
                      .HasDatabaseName("IX_ChatMessages_User_Time");

                entity.HasIndex(e => e.KnowledgeItemId)
                      .HasDatabaseName("IX_ChatMessages_KnowledgeItemId");

                // Ограничения полей
                entity.Property(e => e.TextMessage)
                      .IsRequired()
                      .HasMaxLength(2000);

                entity.Property(e => e.Platform)
                      .IsRequired()
                      .HasMaxLength(50)
                      .HasDefaultValue("Telegram");

                // Внешние ключи
                entity.HasOne(e => e.User)
                      .WithMany(u => u.ChatMessages)
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(e => e.KnowledgeItem)
                      .WithMany(k => k.ChatMessages)
                      .HasForeignKey(e => e.KnowledgeItemId)
                      .OnDelete(DeleteBehavior.SetNull);

                // Значение по умолчанию
                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP"
                      });

            modelBuilder.Entity<KnowledgeItem>(entity =>
            {
                // Индексы для быстрого поиска
                entity.HasIndex(e => e.Category)
                      .HasDatabaseName("IX_KnowledgeItems_Category");

                entity.HasIndex(e => e.IsActive)
                      .HasDatabaseName("IX_KnowledgeItems_IsActive");

                entity.HasIndex(e => e.UseCount)
                      .HasDatabaseName("IX_KnowledgeItems_UseCount");

                entity.HasIndex(e => e.CreatedAt)
                      .HasDatabaseName("IX_KnowledgeItems_CreatedAt");

                entity.HasIndex(e => e.UpdatedAt)
                      .HasDatabaseName("IX_KnowledgeItems_UpdatedAt");

                // Составной индекс для частых запросов
                entity.HasIndex(e => new { e.Category, e.IsActive, e.UseCount })
                      .HasDatabaseName("IX_KnowledgeItems_Category_Active_Usage");

                // Ограничения полей
                entity.Property(e => e.Category)
                      .IsRequired()
                      .HasMaxLength(100)
                      .HasDefaultValue("Общие вопросы");

                entity.Property(e => e.Title)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(e => e.Tags)
                      .HasMaxLength(500)
                      .HasDefaultValue("revit");

                // Значения по умолчанию
                entity.Property(e => e.UseCount)
                      .HasDefaultValue(0);

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.UpdatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP")
                      .ValueGeneratedOnAddOrUpdate();

                // Связь с ChatMessage
                entity.HasMany(e => e.ChatMessages)
                      .WithOne(e => e.KnowledgeItem)
                      .HasForeignKey(e => e.KnowledgeItemId)
                      .OnDelete(DeleteBehavior.SetNull);


            };
    

        //заполнение БД при включении сервера
        private void SeedData(ModelBuilder modelBuilder)
        {

        }
    }
}
