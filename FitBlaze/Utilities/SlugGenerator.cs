namespace FitBlaze.Utilities
{
    public static class SlugGenerator
    {
        public static string Generate(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) return "";
            return title.ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("/", "-")
                .Replace("?", "")
                .Replace("&", "")
                .Replace(":", "");
        }
    }
}
