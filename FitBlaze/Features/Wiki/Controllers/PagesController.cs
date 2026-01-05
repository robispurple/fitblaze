using FitBlaze.Features.Wiki.Models;
using FitBlaze.Features.Wiki.Services;
using Microsoft.AspNetCore.Mvc;

namespace FitBlaze.Features.Wiki.Controllers;

/// <summary>
/// API endpoints for wiki page management.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class PagesController : ControllerBase
{
    private readonly PageService _pageService;
    private readonly ILogger<PagesController> _logger;

    public PagesController(PageService pageService, ILogger<PagesController> logger)
    {
        _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// GET /api/pages - List all pages with pagination.
    /// </summary>
    /// <param name="pageNumber">Page number (default 1)</param>
    /// <param name="pageSize">Page size (default 100, max 500)</param>
    /// <returns>List of pages</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WikiPage>>> GetPages([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
    {
        // Validate page size
        if (pageSize > 500) pageSize = 500;
        if (pageSize <= 0) pageSize = 100;

        var pages = await _pageService.GetAllPagesAsync(pageNumber, pageSize);
        return Ok(pages);
    }

    /// <summary>
    /// GET /api/pages/{slug} - Get a page by slug.
    /// </summary>
    /// <param name="slug">The page slug</param>
    /// <returns>The page or 404 if not found</returns>
    [HttpGet("{slug}")]
    public async Task<ActionResult<WikiPage>> GetPageBySlug(string slug)
    {
        if (string.IsNullOrEmpty(slug))
            return BadRequest("Slug cannot be empty");

        var page = await _pageService.GetPageBySlugAsync(slug);
        if (page == null)
            return NotFound($"Page with slug '{slug}' not found");

        return Ok(page);
    }

    /// <summary>
    /// GET /api/pages/search?q={query} - Search pages by content.
    /// </summary>
    /// <param name="q">Search query</param>
    /// <param name="pageNumber">Page number (default 1)</param>
    /// <param name="pageSize">Page size (default 20, max 100)</param>
    /// <returns>Search results</returns>
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<WikiPage>>> SearchPages([FromQuery] string q, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
    {
        if (string.IsNullOrWhiteSpace(q))
            return BadRequest("Search query cannot be empty");

        // Validate page size
        if (pageSize > 100) pageSize = 100;
        if (pageSize <= 0) pageSize = 20;

        var results = await _pageService.SearchPagesAsync(q, pageNumber, pageSize);
        return Ok(results);
    }

    /// <summary>
    /// POST /api/pages - Create a new page.
    /// </summary>
    /// <param name="request">Page creation request</param>
    /// <returns>Created page with 201 status</returns>
    [HttpPost]
    public async Task<ActionResult<WikiPage>> CreatePage([FromBody] CreatePageRequest request)
    {
        if (string.IsNullOrEmpty(request.Title))
            return BadRequest("Title is required");

        try
        {
            var page = await _pageService.CreatePageAsync(
                request.Title,
                request.Content,
                request.ParentId,
                request.Slug,
                request.Tags
            );

            return CreatedAtAction(nameof(GetPageBySlug), new { slug = page.Slug }, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating page: {Title}", request.Title);
            return StatusCode(500, "An error occurred while creating the page");
        }
    }

    /// <summary>
    /// PUT /api/pages/{id} - Update an existing page.
    /// </summary>
    /// <param name="id">The page ID</param>
    /// <param name="request">Page update request</param>
    /// <returns>Updated page or 404 if not found</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<WikiPage>> UpdatePage(Guid id, [FromBody] UpdatePageRequest request)
    {
        if (string.IsNullOrEmpty(request.Title))
            return BadRequest("Title is required");

        try
        {
            var page = await _pageService.UpdatePageAsync(
                id,
                request.Title,
                request.Content,
                request.Slug,
                request.ParentId,
                request.Tags
            );

            return Ok(page);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Page not found: {PageId}", id);
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating page: {PageId}", id);
            return StatusCode(500, "An error occurred while updating the page");
        }
    }

    /// <summary>
    /// DELETE /api/pages/{id} - Delete a page.
    /// </summary>
    /// <param name="id">The page ID</param>
    /// <returns>204 No Content on success, 404 if not found</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePage(Guid id)
    {
        try
        {
            var success = await _pageService.DeletePageAsync(id);
            if (!success)
                return NotFound($"Page with ID {id} not found");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting page: {PageId}", id);
            return StatusCode(500, "An error occurred while deleting the page");
        }
    }
}

/// <summary>
/// Request model for creating a new page.
/// </summary>
public class CreatePageRequest
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public string? Slug { get; set; }
    public Guid? ParentId { get; set; }
    public string? Tags { get; set; }
}

/// <summary>
/// Request model for updating a page.
/// </summary>
public class UpdatePageRequest
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public string? Slug { get; set; }
    public Guid? ParentId { get; set; }
    public string? Tags { get; set; }
}
