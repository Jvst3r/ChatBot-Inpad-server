using ChatBotInpadserver.Data.DataBase;

namespace ChatBot_Inpad_server.Services
{
    // заготовка обработчика БД
    // через него будет происходить изменения БД
    public class DataBaseHandler
    {
        public AppDbContext db;
        public DataBaseHandler(AppDbContext _db) 
        { 
            db = _db; 
        }

        
    }
}
