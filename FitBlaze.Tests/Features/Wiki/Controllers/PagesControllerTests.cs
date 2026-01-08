using FitBlaze.Controllers;
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using FluentAssertions;
using Xunit;

namespace FitBlaze.Tests.Features.Wiki.Controllers
{
    public class PagesControllerTests : IDisposable
    {
        private readonly SqliteConnection _connection;
        private readonly ApplicationDbContext _dbContext;
        private readonly PageService _pageService;
        private readonly PagesController _controller;

        public PagesControllerTests()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(_connection)
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();

            _pageService = new PageService(_dbContext);
            _controller = new PagesController(_pageService);
        }

        [Fact]
        public async Task PostPage_ReturnsBadRequest_WhenContentIsNull()
        {
            // Arrange
            var request = new CreatePageRequest
            {
                Title = "Test Page",
                Content = null!, // Explicitly setting null
                Slug = "test-page"
            };

            // Act
            var result = await _controller.PostPage(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Content is required.");
        }

        [Fact]
        public async Task PostPage_ReturnsBadRequest_WhenContentIsEmpty()
        {
            // Arrange
            var request = new CreatePageRequest
            {
                Title = "Test Page",
                Content = "",
                Slug = "test-page"
            };

            // Act
            var result = await _controller.PostPage(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Content is required.");
        }

        [Fact]
        public async Task PostPage_ReturnsCreatedAtAction_WhenRequestIsValid()
        {
            // Arrange
            var request = new CreatePageRequest
            {
                Title = "Test Page",
                Content = "Some content",
                Slug = "test-page"
            };

            // Act
            var result = await _controller.PostPage(request);

            // Assert
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdPage = (Page)((CreatedAtActionResult)result.Result!).Value!;
            createdPage.Title.Should().Be("Test Page");
            createdPage.Content.Should().Be("Some content");
        }

        [Fact]
        public async Task PutPage_ReturnsBadRequest_WhenContentIsNull()
        {
            // Arrange
            var request = new UpdatePageRequest
            {
                Id = 1,
                Title = "Test Page",
                Content = null!,
                Slug = "test-page"
            };

            // Act
            var result = await _controller.PutPage(1, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Content is required.");
        }

        [Fact]
        public async Task PutPage_ReturnsBadRequest_WhenContentIsEmpty()
        {
            // Arrange
            var request = new UpdatePageRequest
            {
                Id = 1,
                Title = "Test Page",
                Content = "",
                Slug = "test-page"
            };

            // Act
            var result = await _controller.PutPage(1, request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be("Content is required.");
        }

        [Fact]
        public async Task PutPage_ReturnsNoContent_WhenRequestIsValid()
        {
            // Arrange
            var page = new Page { Title = "Initial Title", Content = "Initial Content" };
            await _pageService.AddPageAsync(page);

            var request = new UpdatePageRequest
            {
                Id = page.Id,
                Title = "Updated Title",
                Content = "Updated Content",
                Slug = "updated-slug"
            };

            // Act
            var result = await _controller.PutPage(page.Id, request);

            // Assert
            result.Should().BeOfType<NoContentResult>();

            var updatedPage = await _pageService.GetPageByIdAsync(page.Id);
            updatedPage!.Title.Should().Be("Updated Title");
            updatedPage.Content.Should().Be("Updated Content");
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            _connection.Close();
            _connection.Dispose();
        }
    }
}
