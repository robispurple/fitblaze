using FitBlaze.Data;
using FitBlaze.Features.Wiki.Models;
using Microsoft.EntityFrameworkCore;

namespace FitBlaze.Features.Wiki.Repositories;

/// <summary>
/// SQLite implementation of the page repository.
/// </summary>
public class SqlitePageRepository : IPageRepository
{
    private readonly WikiContext _context;

    public SqlitePageRepository(WikiContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<WikiPage?> GetPageByIdAsync(Guid pageId)
    {
        return await _context.WikiPages
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == pageId);
    }

    public async Task<WikiPage?> GetPageBySlugAsync(string slug)
    {
        ArgumentException.ThrowIfNullOrEmpty(slug, nameof(slug));

        return await _context.WikiPages
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Slug == slug);
    }

    public async Task<IEnumerable<WikiPage>> GetAllPagesAsync(int skip = 0, int take = 100)
    {
        if (skip < 0) skip = 0;
        if (take <= 0) take = 100;

        return await _context.WikiPages
            .Include(p => p.Parent)
            .Include(p => p.Children)
            .AsNoTracking()
            .OrderBy(p => p.Title)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<IEnumerable<WikiPage>> GetPageHierarchyAsync(Guid? parentId = null)
    {
        return await _context.WikiPages
            .Where(p => p.ParentId == parentId)
            .Include(p => p.Children)
            .AsNoTracking()
            .OrderBy(p => p.Title)
            .ToListAsync();
    }

    public async Task<WikiPage> CreatePageAsync(WikiPage page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _context.WikiPages.Add(page);
        await _context.SaveChangesAsync();

        return page;
    }

    public async Task<WikiPage> UpdatePageAsync(WikiPage page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        page.ModifiedDate = DateTime.UtcNow;

        _context.WikiPages.Update(page);
        await _context.SaveChangesAsync();

        return page;
    }

    public async Task<bool> DeletePageAsync(Guid pageId)
    {
        var page = await _context.WikiPages.FirstOrDefaultAsync(p => p.Id == pageId);

        if (page == null)
            return false;

        _context.WikiPages.Remove(page);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<WikiPage>> SearchByContentAsync(string query, int skip = 0, int take = 20)
    {
        ArgumentException.ThrowIfNullOrEmpty(query, nameof(query));

        if (skip < 0) skip = 0;
        if (take <= 0) take = 20;

        var searchTerm = query.ToLower();

        return await _context.WikiPages
            .Where(p => p.Title.ToLower().Contains(searchTerm) || p.Content.ToLower().Contains(searchTerm))
            .Include(p => p.Parent)
            .AsNoTracking()
            .OrderByDescending(p => p.ModifiedDate)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }
}
