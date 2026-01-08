using FitBlaze.Features.Wiki.Models;
using Markdig;

namespace FitBlaze.Features.Wiki.Services
{
    public class MarkdownEngine : IMarkupEngine
    {
        public MarkupType Type => MarkupType.Markdown;

        public string Render(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            return Markdown.ToHtml(content);
        }
    }
}
