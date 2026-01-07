using Bunit;
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Pages;
using FitBlaze.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using FluentAssertions;
using Xunit;

namespace FitBlaze.Tests.Features.Wiki.Pages
{
    public class PageListTests : TestContext, IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ApplicationDbContext _dbContext;
        private readonly PageService _pageService;

        public PageListTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            _pageService = new PageService(_dbContext);

            Services.AddSingleton(_pageService);
            Services.AddSingleton<ApplicationDbContext>(_dbContext);
        }

        [Fact]
        public void RendersEmptyStateCorrectly()
        {
            var cut = Render<PageList>();

            cut.Markup.Should().Contain("Wiki Pages");
            cut.Markup.Should().Contain("No wiki pages found");
            cut.Find("a.btn-primary").TextContent.Should().Contain("Create New Page");
        }

        [Fact]
        public async Task RendersListOfPages()
        {
            // Arrange
            await _pageService.AddPageAsync(new Page { Title = "Page One", Content = "Content 1" });
            await _pageService.AddPageAsync(new Page { Title = "Page Two", Content = "Content 2" });

            // Act
            var cut = Render<PageList>();

            // Assert
            cut.FindAll(".list-group-item").Should().HaveCount(2);
            cut.Markup.Should().Contain("Page One");
            cut.Markup.Should().Contain("Page Two");
        }

        public new void Dispose()
        {
            _dbContext.Dispose();
            _connection.Close();
            _connection.Dispose();
            base.Dispose();
        }
    }
}
