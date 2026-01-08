using System.Text.RegularExpressions;

namespace FitBlaze.Features.Wiki.Services
{
    public static class SlugGenerator
    {
        public static string Generate(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                return string.Empty;

            // Simple slug generation logic: lowercase, replace spaces with hyphens, remove special characters
            string slug = title.ToLowerInvariant();
            slug = Regex.Replace(slug, @"\s+", "-");
            slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = slug.Trim('-');

            return slug;
        }
    }
}
