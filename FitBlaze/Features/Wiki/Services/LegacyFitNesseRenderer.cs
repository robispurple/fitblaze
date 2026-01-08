using FitBlaze.Features.Wiki.Models;
using System.Net;

namespace FitBlaze.Features.Wiki.Services
{
    public class LegacyFitNesseRenderer : IMarkupRenderer
    {
        public MarkupType SupportedType => MarkupType.FitNesse;

        public string Render(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            // Simple fallback: render as preformatted text for now
            return $"<pre>{WebUtility.HtmlEncode(content)}</pre>";
        }
    }
}
