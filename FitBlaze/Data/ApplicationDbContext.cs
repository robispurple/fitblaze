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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Page>()
                .HasIndex(p => p.Slug)
                .IsUnique();
        }
    }
}