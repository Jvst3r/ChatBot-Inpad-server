using System.ComponentModel.DataAnnotations;

namespace ChatBot_Inpad_server.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Platform { get; set; } = "Telegram";

        [Required]
        [MaxLength(100)]
        public string PlatformId { get; set; } = string.Empty; 

        [MaxLength(100)]
        public string? UserName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow; 

        [Required]
        public DateTime LastActiveAt { get; set; } = DateTime.UtcNow; 

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();
    }
}
