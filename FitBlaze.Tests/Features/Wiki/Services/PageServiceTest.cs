using FitBlaze.Features.Wiki.Models;
using FitBlaze.Features.Wiki.Repositories;
using FitBlaze.Features.Wiki.Services;
using Moq;
using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace FitBlaze.Tests.Features.Wiki.Services;

public class PageServiceTest
{
    private Mock<IPageRepository> CreateMockRepository()
    {
        return new Mock<IPageRepository>();
    }

    private Mock<SlugService> CreateMockSlugService()
    {
        var mockRepository = new Mock<IPageRepository>();
        return new Mock<SlugService>(mockRepository.Object);
    }

    [Fact]
    public async Task CreatePageAsync_CreatesPageWithGeneratedSlug()
    {
        // Arrange
        var mockRepository = CreateMockRepository();
        var realSlugService = new SlugService(mockRepository.Object);
        var mockLogger = new Mock<ILogger<PageService>>();

        mockRepository.Setup(r => r.CreatePageAsync(It.IsAny<WikiPage>()))
            .ReturnsAsync((WikiPage p) => p);
        
        mockRepository.Setup(r => r.GetPageBySlugAsync(It.IsAny<string>()))
            .ReturnsAsync((WikiPage?)null);

        var service = new PageService(mockRepository.Object, realSlugService, mockLogger.Object);

        // Act
        var result = await service.CreatePageAsync("Test Page", "Content");

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be("Test Page");
        result.Slug.Should().Be("test-page");
        mockRepository.Verify(r => r.CreatePageAsync(It.IsAny<WikiPage>()), Times.Once);
    }

    [Fact]
    public async Task GetPageBySlugAsync_ReturnsPage()
    {
        // Arrange
        var mockRepository = CreateMockRepository();
        var mockSlugService = CreateMockSlugService();
        var mockLogger = new Mock<ILogger<PageService>>();

        var expectedPage = new WikiPage { Id = Guid.NewGuid(), Title = "Test", Slug = "test" };
        mockRepository.Setup(r => r.GetPageBySlugAsync("test"))
            .ReturnsAsync(expectedPage);

        var service = new PageService(mockRepository.Object, mockSlugService.Object, mockLogger.Object);

        // Act
        var result = await service.GetPageBySlugAsync("test");

        // Assert
        result.Should().Be(expectedPage);
    }

    [Fact]
    public async Task UpdatePageAsync_ThrowsExceptionWhenPageNotFound()
    {
        // Arrange
        var mockRepository = CreateMockRepository();
        var mockSlugService = CreateMockSlugService();
        var mockLogger = new Mock<ILogger<PageService>>();

        mockRepository.Setup(r => r.GetPageByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((WikiPage?)null);

        var service = new PageService(mockRepository.Object, mockSlugService.Object, mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.UpdatePageAsync(Guid.NewGuid(), "Updated", "Content"));
    }

    [Fact]
    public async Task DeletePageAsync_CallsRepository()
    {
        // Arrange
        var mockRepository = CreateMockRepository();
        var mockSlugService = CreateMockSlugService();
        var mockLogger = new Mock<ILogger<PageService>>();

        var pageId = Guid.NewGuid();
        mockRepository.Setup(r => r.DeletePageAsync(pageId))
            .ReturnsAsync(true);

        var service = new PageService(mockRepository.Object, mockSlugService.Object, mockLogger.Object);

        // Act
        var result = await service.DeletePageAsync(pageId);

        // Assert
        result.Should().BeTrue();
        mockRepository.Verify(r => r.DeletePageAsync(pageId), Times.Once);
    }

    [Fact]
    public async Task SearchPagesAsync_CallsRepositoryWithPagination()
    {
        // Arrange
        var mockRepository = CreateMockRepository();
        var mockSlugService = CreateMockSlugService();
        var mockLogger = new Mock<ILogger<PageService>>();

        var results = new List<WikiPage>
        {
            new() { Title = "Test Result", Slug = "test-result" }
        };

        mockRepository.Setup(r => r.SearchByContentAsync("test", It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(results);

        var service = new PageService(mockRepository.Object, mockSlugService.Object, mockLogger.Object);

        // Act
        var result = await service.SearchPagesAsync("test");

        // Assert
        result.Should().HaveCount(1);
        mockRepository.Verify(r => r.SearchByContentAsync("test", 0, 20), Times.Once);
    }
}
