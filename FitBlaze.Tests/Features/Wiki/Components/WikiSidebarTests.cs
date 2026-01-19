using Bunit;
using FitBlaze.Features.Wiki.Components;
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Models;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using System.Threading.Tasks;
using FluentAssertions;

namespace FitBlaze.Tests.Features.Wiki.Components
{
    public class WikiSidebarTests : BunitContext, IAsyncLifetime
    {
        private SqliteConnection _connection;
        private ApplicationDbContext _context;
        private PageService _pageService;

        public Task InitializeAsync()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _pageService = new PageService(_context);

            Services.AddScoped<ApplicationDbContext>(_ => _context);
            Services.AddScoped<PageService>(_ => _pageService);

            return Task.CompletedTask;
        }

        public Task DisposeAsync()
        {
            _context?.Dispose();
            _connection?.Dispose();
            base.Dispose();
            return Task.CompletedTask;
        }

        [Fact]
        public void WikiSidebar_RendersPageList()
        {
            // Arrange
            _context.Pages.Add(new Page { Title = "Home Page", Slug = "home", Content = "Welcome", LastModified = System.DateTime.UtcNow });
            _context.Pages.Add(new Page { Title = "Second Page", Slug = "second", Content = "Content", LastModified = System.DateTime.UtcNow });
            _context.SaveChanges();

            // Act
            var cut = Render<WikiSidebar>();

            // Assert
            cut.WaitForState(() => cut.FindAll("a.list-group-item").Count > 0);
            var links = cut.FindAll("a.list-group-item");
            links.Should().HaveCount(2);
            links[0].TextContent.Should().Contain("Home Page");
            links[1].TextContent.Should().Contain("Second Page");
        }

        [Fact]
        public void WikiSidebar_RendersNewPageButton()
        {
            // Act
            var cut = Render<WikiSidebar>();

            // Assert
            var button = cut.Find("a.btn-success");
            button.TextContent.Should().Contain("+ New Page");
            button.GetAttribute("href").Should().Be("/wiki/create");
        }
    }
}
