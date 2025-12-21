using System.ComponentModel.DataAnnotations;

namespace ChatBot_Inpad_server.Data.Models
{
    public class User
    {
        [Key] // PK - первичный ключ
        public int Id { get; set; }

        public string Platform { get; set; } = string.Empty; //Telegram, Revit, Web
        public int PlatformId { get; set; } //ID на самой платформе


    }
}
