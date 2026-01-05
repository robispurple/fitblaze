using FitBlaze.Features.Wiki.Models;
using FitBlaze.Features.Wiki.Repositories;
using Microsoft.Extensions.Logging;

namespace FitBlaze.Features.Wiki.Services;

/// <summary>
/// Business logic service for wiki page operations.
/// </summary>
public class PageService
{
    private readonly IPageRepository _repository;
    private readonly SlugService _slugService;
    private readonly ILogger<PageService> _logger;

    public PageService(IPageRepository repository, SlugService slugService, ILogger<PageService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _slugService = slugService ?? throw new ArgumentNullException(nameof(slugService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Creates a new wiki page.
    /// </summary>
    public async Task<WikiPage> CreatePageAsync(string title, string content, Guid? parentId = null, string? slug = null, string? tags = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        // Generate slug if not provided
        slug ??= await _slugService.GenerateUniqueSlugAsync(title);

        var page = new WikiPage
        {
            Title = title,
            Slug = slug,
            Content = content ?? string.Empty,
            ParentId = parentId,
            Tags = tags ?? string.Empty,
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow,
            IsPublished = true
        };

        var created = await _repository.CreatePageAsync(page);
        _logger.LogInformation("Created wiki page: {PageId} - {Title}", created.Id, created.Title);

        return created;
    }

    /// <summary>
    /// Retrieves a page by its ID.
    /// </summary>
    public async Task<WikiPage?> GetPageAsync(Guid pageId)
    {
        return await _repository.GetPageByIdAsync(pageId);
    }

    /// <summary>
    /// Retrieves a page by its slug.
    /// </summary>
    public async Task<WikiPage?> GetPageBySlugAsync(string slug)
    {
        ArgumentException.ThrowIfNullOrEmpty(slug, nameof(slug));
        return await _repository.GetPageBySlugAsync(slug);
    }

    /// <summary>
    /// Updates an existing page.
    /// </summary>
    public async Task<WikiPage> UpdatePageAsync(Guid pageId, string title, string content, string? slug = null, Guid? parentId = null, string? tags = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));

        var page = await _repository.GetPageByIdAsync(pageId);
        if (page == null)
            throw new InvalidOperationException($"Page with ID {pageId} not found.");

        // Update basic properties
        page.Title = title;
        page.Content = content ?? string.Empty;
        page.Tags = tags ?? string.Empty;
        page.ModifiedDate = DateTime.UtcNow;

        // Update slug if changed
        if (!string.IsNullOrEmpty(slug) && slug != page.Slug)
        {
            page.Slug = slug;
        }

        // Update parent if provided
        if (parentId.HasValue && parentId != page.ParentId)
        {
            page.ParentId = parentId;
        }

        var updated = await _repository.UpdatePageAsync(page);
        _logger.LogInformation("Updated wiki page: {PageId} - {Title}", updated.Id, updated.Title);

        return updated;
    }

    /// <summary>
    /// Deletes a page by its ID.
    /// </summary>
    public async Task<bool> DeletePageAsync(Guid pageId)
    {
        var success = await _repository.DeletePageAsync(pageId);
        if (success)
            _logger.LogInformation("Deleted wiki page: {PageId}", pageId);

        return success;
    }

    /// <summary>
    /// Retrieves the hierarchical structure of pages.
    /// </summary>
    public async Task<IEnumerable<WikiPage>> GetPageHierarchyAsync(Guid? parentId = null)
    {
        return await _repository.GetPageHierarchyAsync(parentId);
    }

    /// <summary>
    /// Searches pages by content (title and body).
    /// </summary>
    public async Task<IEnumerable<WikiPage>> SearchPagesAsync(string query, int pageNumber = 1, int pageSize = 20)
    {
        ArgumentException.ThrowIfNullOrEmpty(query, nameof(query));

        if (pageNumber < 1) pageNumber = 1;
        if (pageSize <= 0) pageSize = 20;

        var skip = (pageNumber - 1) * pageSize;
        return await _repository.SearchByContentAsync(query, skip, pageSize);
    }

    /// <summary>
    /// Generates a unique slug for a given title.
    /// </summary>
    public async Task<string> GenerateSlugAsync(string title)
    {
        ArgumentException.ThrowIfNullOrEmpty(title, nameof(title));
        return await _slugService.GenerateUniqueSlugAsync(title);
    }

    /// <summary>
    /// Gets all pages with pagination.
    /// </summary>
    public async Task<IEnumerable<WikiPage>> GetAllPagesAsync(int pageNumber = 1, int pageSize = 100)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize <= 0) pageSize = 100;

        var skip = (pageNumber - 1) * pageSize;
        return await _repository.GetAllPagesAsync(skip, pageSize);
    }
}
