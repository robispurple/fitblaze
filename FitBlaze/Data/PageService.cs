using Microsoft.EntityFrameworkCore;
using FitBlaze.Models;
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

        public async Task<Page> AddPageAsync(Page newPage)
        {
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

            await _context.SaveChangesAsync();
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
    }
}