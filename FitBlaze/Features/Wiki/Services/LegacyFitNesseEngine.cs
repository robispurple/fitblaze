using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Services
{
    public class LegacyFitNesseEngine : IMarkupEngine
    {
        public MarkupType Type => MarkupType.LegacyFitNesse;

        public string Render(string content)
        {
            // Placeholder for Legacy FitNesse rendering logic
            return $"<div class='alert alert-info'>Legacy FitNesse Content (Not implemented yet)</div><pre>{content}</pre>";
        }
    }
}
