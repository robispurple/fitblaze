using Bunit;
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Pages;
using FitBlaze.Features.Wiki.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.Sqlite;
using FluentAssertions;
using Xunit;

namespace FitBlaze.Tests.Features.Wiki.Pages
{
    public class PageEditorTests : BunitContext, IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ApplicationDbContext _dbContext;
        private readonly PageService _pageService;

        public PageEditorTests()
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
        public void RendersCreateModeCorrectly()
        {
            var cut = Render<PageEditor>();

            cut.Markup.Should().Contain("Create Page");
            cut.Find("#title").GetAttribute("value").Should().BeNullOrEmpty();
        }

        [Fact]
        public void AutoGeneratesSlugFromTitle()
        {
            var cut = Render<PageEditor>();
            var titleInput = cut.Find("#title");

            titleInput.Change("My New Page");

            // We need to trigger the binding update. 
            // In the component, we used @bind-Value:after="OnTitleChanged".
            // BUnit's Change() triggers the standard onchange event which updates the value.
            // The :after callback should be invoked by the runtime.

            // Wait for state change if necessary, or check immediately.
            cut.Find("#slug").GetAttribute("value").Should().Be("my-new-page");
        }

        [Fact]
        public async Task RendersEditModeCorrectly()
        {
            // Arrange
            var page = new Page { Title = "Existing Page", Content = "Old Content" };
            await _pageService.AddPageAsync(page);
            // The slug will be generated as 'existing-page'

            // Act
            var cut = Render<PageEditor>(parameters => parameters
                .Add(p => p.Slug, "existing-page")
            );

            // Assert
            cut.Markup.Should().Contain("Edit Page: Existing Page");
            cut.Find("#title").GetAttribute("value").Should().Be("Existing Page");
            cut.Find("#slug").GetAttribute("value").Should().Be("existing-page");
            cut.Find("#content").GetAttribute("value").Should().Be("Old Content");
        }

        [Fact]
        public void ShowsErrorForReservedSlug()
        {
            var cut = Render<PageEditor>();
            cut.Find("#title").Change("Create"); // Should gen slug "create"
            cut.Find("#slug").GetAttribute("value").Should().Be("create");

            cut.Find("form").Submit();

            cut.Markup.Should().Contain("The slug 'create' is reserved");
        }

        [Fact]
        public async Task ShowsErrorForDuplicateSlug()
        {
            await _pageService.AddPageAsync(new Page { Title = "Existing Page", Content = "Content" });
            // Slug is 'existing-page'

            var cut = Render<PageEditor>();
            cut.Find("#title").Change("Existing Page"); // Generates 'existing-page'

            cut.Find("form").Submit();

            cut.Markup.Should().Contain("The slug 'existing-page' is already in use");
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
