namespace FitBlaze.Features.Wiki.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty; // Non-optional as per the task
        public MarkupType MarkupType { get; set; } = MarkupType.Markdown;
        public DateTime LastModified { get; set; }
    }
}