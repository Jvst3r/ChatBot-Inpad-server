using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot_Inpad_server.Data.Models
{
    public class ChatMessage
    {
        [Key] // PK
        public int Id { get; set; }

        [ForeignKey("User")] // FK - ссылка на модель User`a
        public int UserId { get; set; } 

        [Required]
        public string TextMessage { get; set; }

        [Required]
        public string BotResponse {  get; set; }

        [Required]
        public string Platfom { get; set; }

        [Required]
        public DateTime Time { get; set; }


    }
}
