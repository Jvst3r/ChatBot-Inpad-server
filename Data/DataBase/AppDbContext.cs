using Microsoft.EntityFrameworkCore;

namespace ChatBotInpadserver.Data.DataBase
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

        //тут будут DbSet`ы если нам надо будет

    }
}
