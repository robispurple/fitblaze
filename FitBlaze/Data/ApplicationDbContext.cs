using Microsoft.EntityFrameworkCore;
using FitBlaze.Features.Wiki.Models;

namespace FitBlaze.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
    }
}