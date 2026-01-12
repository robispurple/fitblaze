using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Services
{
    public interface IMarkupOrchestrator
    {
        string Render(string content, MarkupType type);
    }
}
