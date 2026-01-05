namespace FitBlaze.Features.Wiki.Models;

/// <summary>
/// Represents a wiki page with metadata, content, and hierarchy support.
/// </summary>
public class WikiPage
{
    /// <summary>
    /// Unique identifier for the page.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Human-readable title of the page.
    /// </summary>
    public required string Title { get; set; }

    /// <summary>
    /// URL-friendly slug for the page (lowercase, no special characters).
    /// </summary>
    public required string Slug { get; set; }

    /// <summary>
    /// Page content in Markdown format.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// ID of the parent page for hierarchical organization (null for root pages).
    /// </summary>
    public Guid? ParentId { get; set; }

    /// <summary>
    /// Date and time when the page was created.
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date and time when the page was last modified.
    /// </summary>
    public DateTime ModifiedDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Version number for optimistic concurrency control.
    /// </summary>
    public int Version { get; set; } = 1;

    /// <summary>
    /// Indicates whether the page is published and visible.
    /// </summary>
    public bool IsPublished { get; set; } = true;

    /// <summary>
    /// Comma-separated tags for categorization and search.
    /// </summary>
    public string Tags { get; set; } = string.Empty;

    /// <summary>
    /// Navigation reference to parent page (loaded via EF Core relationship).
    /// </summary>
    public WikiPage? Parent { get; set; }

    /// <summary>
    /// Collection of child pages (loaded via EF Core relationship).
    /// </summary>
    public ICollection<WikiPage> Children { get; set; } = new List<WikiPage>();
}
