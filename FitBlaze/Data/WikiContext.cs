using FitBlaze.Features.Wiki.Models;
using Microsoft.EntityFrameworkCore;

namespace FitBlaze.Data;

/// <summary>
/// Entity Framework DbContext for the FitBlaze wiki application.
/// </summary>
public class WikiContext : DbContext
{
    public WikiContext(DbContextOptions<WikiContext> options) : base(options)
    {
    }

    /// <summary>
    /// Wiki pages dataset.
    /// </summary>
    public DbSet<WikiPage> WikiPages { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure WikiPage entity
        modelBuilder.Entity<WikiPage>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.Slug)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Content)
                .HasDefaultValue(string.Empty);

            entity.Property(e => e.Tags)
                .HasDefaultValue(string.Empty);

            entity.Property(e => e.IsPublished)
                .HasDefaultValue(true);

            entity.Property(e => e.Version)
                .HasDefaultValue(1)
                .IsConcurrencyToken();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            entity.Property(e => e.ModifiedDate)
                .HasDefaultValueSql("datetime('now')");

            // Configure parent-child relationship
            entity.HasOne(e => e.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(e => e.ParentId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            // Create indexes for common queries
            entity.HasIndex(e => e.Slug)
                .IsUnique();

            entity.HasIndex(e => e.IsPublished);

            entity.HasIndex(e => e.CreatedDate);

            entity.HasIndex(e => e.ModifiedDate);
        });
    }
}
