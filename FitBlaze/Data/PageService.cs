using Microsoft.EntityFrameworkCore;
using FitBlaze.Features.Wiki.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FitBlaze.Data
{
    public class PageService
    {
        private readonly ApplicationDbContext _context;

        public PageService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Page>> GetPagesAsync()
        {
            return await _context.Pages.ToListAsync();
        }

        public async Task<Page?> GetPageByIdAsync(int id)
        {
            return await _context.Pages.FindAsync(id);
        }

        public async Task<Page?> GetPageBySlugAsync(string slug)
        {
            return await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<Page> AddPageAsync(Page newPage)
        {
            if (string.IsNullOrEmpty(newPage.Slug))
            {
                newPage.Slug = GenerateSlug(newPage.Title);
            }
            newPage.LastModified = DateTime.UtcNow;
            _context.Pages.Add(newPage);
            await _context.SaveChangesAsync();
            return newPage;
        }

        public async Task<Page?> UpdatePageAsync(Page updatedPage)
        {
            var existingPage = await _context.Pages.FindAsync(updatedPage.Id);

            if (existingPage == null)
            {
                return null;
            }

            existingPage.Title = updatedPage.Title;
            existingPage.Content = updatedPage.Content;
            existingPage.Slug = string.IsNullOrEmpty(updatedPage.Slug) ? GenerateSlug(updatedPage.Title) : updatedPage.Slug;
            existingPage.LastModified = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existingPage;
        }

        private string GenerateSlug(string title)
        {
            return title.ToLowerInvariant().Replace(" ", "-").Replace("/", "-"); // Simple slug generation
        }

        public async Task<bool> DeletePageAsync(int id)
        {
            var pageToDelete = await _context.Pages.FindAsync(id);
            if (pageToDelete == null)
            {
                return false;
            }

            _context.Pages.Remove(pageToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsSlugUniqueAsync(string slug, int? excludeId = null)
        {
            var query = _context.Pages.AsQueryable();
            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }
            return !await query.AnyAsync(p => p.Slug == slug);
        }
    }
}