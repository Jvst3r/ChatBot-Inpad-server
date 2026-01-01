using System.ComponentModel.DataAnnotations;

namespace ChatBotInpadServer.Data.Models
{
    public class Admin
    {
        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; }


        [Required]
        
        public string PasswordHash { get; set; }

        public DateTime? LastLoginAt { get; set; }

        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.UtcNow;
        }
    }
}
