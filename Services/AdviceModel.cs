
namespace Services.Models
{
    public class AdviceItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    public class UserSession
    {
        public long ChatId { get; set; }
        public int CurrentAdviceIndex { get; set; }
        public string CurrentCategory { get; set; } = "general";
    }
}