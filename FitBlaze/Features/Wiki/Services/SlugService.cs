using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using FitBlaze.Features.Wiki.Repositories;

namespace FitBlaze.Features.Wiki.Services;

/// <summary>
/// Service for generating URL-friendly slugs from page titles with collision detection.
/// </summary>
public class SlugService
{
    private readonly IPageRepository _repository;
    private const int MaxSlugLength = 100;

    public SlugService(IPageRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Generates a URL-friendly slug from a title.
    /// Converts to lowercase, removes special characters, replaces spaces with hyphens.
    /// </summary>
    public string GenerateSlug(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        // Convert to lowercase
        var slug = title.ToLowerInvariant();

        // Remove accents and convert to ASCII
        slug = RemoveDiacritics(slug);

        // Replace spaces with hyphens
        slug = Regex.Replace(slug, @"\s+", "-");

        // Remove all characters except alphanumeric and hyphens
        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        // Remove consecutive hyphens
        slug = Regex.Replace(slug, @"-+", "-");

        // Remove leading and trailing hyphens
        slug = slug.Trim('-');

        // Truncate to max length
        if (slug.Length > MaxSlugLength)
        {
            slug = slug.Substring(0, MaxSlugLength).TrimEnd('-');
        }

        return string.IsNullOrEmpty(slug) ? "page" : slug;
    }

    /// <summary>
    /// Generates a unique slug by appending a numeric suffix if a collision is detected.
    /// </summary>
    public async Task<string> GenerateUniqueSlugAsync(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        var baseSlug = GenerateSlug(title);
        var slug = baseSlug;
        var counter = 2;

        // Check for collisions and append suffix
        while (await SlugExistsAsync(slug))
        {
            slug = $"{baseSlug}-{counter}";
            counter++;
        }

        return slug;
    }

    /// <summary>
    /// Checks if a slug already exists in the repository.
    /// </summary>
    private async Task<bool> SlugExistsAsync(string slug)
    {
        var page = await _repository.GetPageBySlugAsync(slug);
        return page != null;
    }

    /// <summary>
    /// Removes diacritical marks (accents) from text, converting to ASCII equivalents.
    /// </summary>
    private static string RemoveDiacritics(string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();

        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }
}
