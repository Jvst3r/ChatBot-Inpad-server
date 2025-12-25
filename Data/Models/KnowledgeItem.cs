using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot_Inpad_server.Data.Models
{
    public class KnowledgeItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Category { get; set; } = "Общие вопросы";
        [Required]
        public string Title { get; set; }

        [Required]
        [Column(TypeName = "text")]
        public string AnswerText { get; set; }

        //тут картинки, ещё что то
        //public object Content { get; set; }

        public int UseCount { get; set; } = 0;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string Tags { get; set; } = "revit";

        public virtual ICollection<ChatMessage> ChatMessages { get; set; } = new List<ChatMessage>();


        public void AddTag(string tag)
        {
            var tags = GetTagList();
            var normalizedTag = tag.Trim().ToLower();

            if (!tags.Contains(normalizedTag))
            {
                tags.Add(normalizedTag);
                Tags = string.Join(",", tags);
                UpdatedAt = DateTime.UtcNow;
            }
        }

        public List<string> GetTagList()
        {
            return string.IsNullOrEmpty(Tags)
                ? new List<string>()
                : Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                      .Select(t => t.Trim().ToLower())
                      .Distinct()
                      .ToList();
        }

        public void IncrementUseCount()
        {
            UseCount++;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
