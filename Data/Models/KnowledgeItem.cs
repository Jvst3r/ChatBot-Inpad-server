using System.ComponentModel.DataAnnotations;

namespace ChatBot_Inpad_server.Data.Models
{
    public class KnowledgeItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Category { get; set; }
        [Required]
        public string AnswerText { get; set; }

        //тут картинки, ещё что то
        public object Content { get; set; }

        public int UseCount = 0;
    }
}
