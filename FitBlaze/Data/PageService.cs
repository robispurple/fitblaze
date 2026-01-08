using Microsoft.EntityFrameworkCore;
using FitBlaze.Features.Wiki.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using FitBlaze.Utilities;

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

        // Removed private GenerateSlug in favor of SlugGenerator.Generate

        public async Task<Page?> GetPageBySlugAsync(string slug)
        {
            return await _context.Pages.FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<Page> AddPageAsync(Page newPage)
        {
            if (string.IsNullOrEmpty(newPage.Slug))
            {
                newPage.Slug = SlugGenerator.Generate(newPage.Title);
            }
            newPage.LastModified = DateTime.UtcNow;

            try
            {
                _context.Pages.Add(newPage);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateSlugException(newPage.Slug);
            }

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
            existingPage.Slug = string.IsNullOrEmpty(updatedPage.Slug) ? SlugGenerator.Generate(updatedPage.Title) : updatedPage.Slug;
            existingPage.LastModified = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new DuplicateSlugException(existingPage.Slug);
            }

            return existingPage;
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

        private bool IsUniqueConstraintViolation(DbUpdateException ex)
        {
            return ex.InnerException is SqliteException sqliteEx && sqliteEx.SqliteErrorCode == 19;
        }
    }

    public class DuplicateSlugException : Exception
    {
        public DuplicateSlugException(string slug)
            : base($"A page with the slug '{slug}' already exists.")
        {
        }
    }
}