using Microsoft.EntityFrameworkCore;
using ChatBot_Inpad_server.Data.Models;

namespace ChatBotInpadserver.Data.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        //тут будут DbSet`ы если нам надо будет
        public DbSet<User> Users { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<KnowledgeItem> KnowledgeItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {

            });
            modelBuilder.Entity<ChatMessage>(entity =>
            {

            });
            modelBuilder.Entity<KnowledgeItem>(entity =>
            {
            });
        }

        //заполнение БД при включении сервера
        private void SeedData(ModelBuilder modelBuilder)
        {

        }
    }
}
