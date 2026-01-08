using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Services
{
    public interface IMarkupRenderer
    {
        MarkupType SupportedType { get; }
        string Render(string content);
    }
}
