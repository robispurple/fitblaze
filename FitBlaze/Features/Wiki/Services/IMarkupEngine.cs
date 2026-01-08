using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Services
{
    public interface IMarkupEngine
    {
        MarkupType Type { get; }
        string Render(string content);
    }
}
