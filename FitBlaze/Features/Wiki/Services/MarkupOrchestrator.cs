using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Services
{
    public class MarkupOrchestrator
    {
        private readonly Dictionary<MarkupType, IMarkupEngine> _engines;

        public MarkupOrchestrator(IEnumerable<IMarkupEngine> engines)
        {
            _engines = engines.ToDictionary(e => e.Type, e => e);
        }

        public string Render(string content, MarkupType type)
        {
            if (_engines.TryGetValue(type, out var engine))
            {
                return engine.Render(content);
            }

            // Fallback to Markdown or throw exception depending on requirements.
            // For now, defaulting to Markdown if available, otherwise just text.
            if (_engines.TryGetValue(MarkupType.Markdown, out var defaultEngine))
            {
                return defaultEngine.Render(content);
            }

            return content;
        }
    }
}
