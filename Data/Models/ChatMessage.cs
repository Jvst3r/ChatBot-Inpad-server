using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBotInpadServer.Data.Models
{
    public class ChatMessage
    {
        [Key] // PK
        public int Id { get; set; }

        [ForeignKey("User")] // FK - ссылка на модель User`a
        public int? UserId { get; set; } 

        public virtual User? User { get; set; } //навигационное свойство

        [Required]
        public string TextMessage { get; set; }

        [Required]
        public string Platform { get; set; } = "Telegram";

        [Required]
        public bool IsFromUser { get; set; }

        [ForeignKey("KnowledgeItem")]
        public int? KnowledgeItemId { get; set; }

        public virtual KnowledgeItem? KnowledgeItem { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //для удобства метод
        public bool IsBotMessage => !IsFromUser;

        //Для логов в консоль
        public string ToLogString()
        {
            return $"[{CreatedAt:HH:mm:ss}] {(IsFromUser ? "👤" : "🤖")} {TextMessage}";
        }

    }
}
