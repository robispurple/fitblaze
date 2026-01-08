using FitBlaze.Features.Wiki.Models;
using Markdig;

namespace FitBlaze.Features.Wiki.Services
{
    public class CommonMarkRenderer : IMarkupRenderer
    {
        public MarkupType SupportedType => MarkupType.CommonMark;

        public string Render(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            return Markdown.ToHtml(content);
        }
    }
}
