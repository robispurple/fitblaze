using FitBlaze.Features.Wiki.Repositories;
using FitBlaze.Features.Wiki.Services;
using Moq;
using Xunit;
using FluentAssertions;

namespace FitBlaze.Tests.Features.Wiki.Services;

public class SlugServiceTest
{
    [Theory]
    [InlineData("Hello World", "hello-world")]
    [InlineData("Test Page", "test-page")]
    [InlineData("123 Numeric Title", "123-numeric-title")]
    [InlineData("Special!@#$%Chars", "specialchars")]
    public void GenerateSlug_CreatesValidSlug(string title, string expected)
    {
        // Arrange
        var mockRepository = new Mock<IPageRepository>();
        var service = new SlugService(mockRepository.Object);

        // Act
        var result = service.GenerateSlug(title);

        // Assert
        result.Should().Be(expected);
    }

    [Fact]
    public void GenerateSlug_TrimsExcessiveLength()
    {
        // Arrange
        var mockRepository = new Mock<IPageRepository>();
        var service = new SlugService(mockRepository.Object);
        var longTitle = string.Concat(Enumerable.Repeat("long-title-", 20));

        // Act
        var result = service.GenerateSlug(longTitle);

        // Assert
        result.Length.Should().BeLessThanOrEqualTo(100);
    }

    [Fact]
    public void GenerateSlug_HandlesMultipleSpaces()
    {
        // Arrange
        var mockRepository = new Mock<IPageRepository>();
        var service = new SlugService(mockRepository.Object);

        // Act
        var result = service.GenerateSlug("Multiple    Spaces");

        // Assert
        result.Should().Be("multiple-spaces");
    }

    [Fact]
    public async Task GenerateUniqueSlugAsync_ReturnsSameSlugIfNotExists()
    {
        // Arrange
        var mockRepository = new Mock<IPageRepository>();
        mockRepository.Setup(r => r.GetPageBySlugAsync(It.IsAny<string>()))
            .ReturnsAsync((string slug) => null);

        var service = new SlugService(mockRepository.Object);

        // Act
        var result = await service.GenerateUniqueSlugAsync("Test Title");

        // Assert
        result.Should().Be("test-title");
    }

    [Fact]
    public async Task GenerateUniqueSlugAsync_AppendsSuffixOnCollision()
    {
        // Arrange
        var mockRepository = new Mock<IPageRepository>();
        mockRepository.Setup(r => r.GetPageBySlugAsync("test-title"))
            .ReturnsAsync(new FitBlaze.Features.Wiki.Models.WikiPage { Title = "Test", Slug = "test-title" });
        mockRepository.Setup(r => r.GetPageBySlugAsync("test-title-2"))
            .ReturnsAsync((FitBlaze.Features.Wiki.Models.WikiPage?)null);

        var service = new SlugService(mockRepository.Object);

        // Act
        var result = await service.GenerateUniqueSlugAsync("Test Title");

        // Assert
        result.Should().Be("test-title-2");
    }
}
