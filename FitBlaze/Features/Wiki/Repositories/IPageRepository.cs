using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Features.Wiki.Repositories;

/// <summary>
/// Repository interface for WikiPage data access operations.
/// </summary>
public interface IPageRepository
{
    /// <summary>
    /// Retrieves a page by its unique identifier.
    /// </summary>
    /// <param name="pageId">The page ID.</param>
    /// <returns>The page if found; otherwise null.</returns>
    Task<WikiPage?> GetPageByIdAsync(Guid pageId);

    /// <summary>
    /// Retrieves a page by its URL slug.
    /// </summary>
    /// <param name="slug">The URL-friendly slug.</param>
    /// <returns>The page if found; otherwise null.</returns>
    Task<WikiPage?> GetPageBySlugAsync(string slug);

    /// <summary>
    /// Retrieves all pages with optional pagination.
    /// </summary>
    /// <param name="skip">Number of pages to skip for pagination.</param>
    /// <param name="take">Number of pages to retrieve.</param>
    /// <returns>Collection of all pages.</returns>
    Task<IEnumerable<WikiPage>> GetAllPagesAsync(int skip = 0, int take = 100);

    /// <summary>
    /// Retrieves the hierarchical structure of pages starting from a root or specified parent.
    /// </summary>
    /// <param name="parentId">Parent ID to start hierarchy from (null for root pages).</param>
    /// <returns>Collection of pages in the hierarchy.</returns>
    Task<IEnumerable<WikiPage>> GetPageHierarchyAsync(Guid? parentId = null);

    /// <summary>
    /// Creates a new page.
    /// </summary>
    /// <param name="page">The page to create.</param>
    /// <returns>The created page with ID assigned.</returns>
    Task<WikiPage> CreatePageAsync(WikiPage page);

    /// <summary>
    /// Updates an existing page.
    /// </summary>
    /// <param name="page">The page with updated data.</param>
    /// <returns>The updated page.</returns>
    Task<WikiPage> UpdatePageAsync(WikiPage page);

    /// <summary>
    /// Deletes a page by its ID.
    /// </summary>
    /// <param name="pageId">The page ID to delete.</param>
    /// <returns>True if deleted; false if not found.</returns>
    Task<bool> DeletePageAsync(Guid pageId);

    /// <summary>
    /// Searches pages by content (title and body).
    /// </summary>
    /// <param name="query">Search query string.</param>
    /// <param name="skip">Number of results to skip for pagination.</param>
    /// <param name="take">Number of results to retrieve.</param>
    /// <returns>Collection of matching pages.</returns>
    Task<IEnumerable<WikiPage>> SearchByContentAsync(string query, int skip = 0, int take = 20);
}
