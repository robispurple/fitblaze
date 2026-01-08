using System.ComponentModel.DataAnnotations;

namespace FitBlaze.Features.Wiki.Models
{
    public class CreatePageRequest
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;
        public MarkupType MarkupType { get; set; } = MarkupType.Markdown;

        public string Slug { get; set; } = string.Empty;
    }
}
