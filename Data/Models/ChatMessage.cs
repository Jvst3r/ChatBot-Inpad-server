using System.ComponentModel.DataAnnotations;

namespace ChatBot_Inpad_server.Data.Models
{
    public class ChatMessage
    {
        [Required]
        public int Id { get; set; }

        [Required]
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
