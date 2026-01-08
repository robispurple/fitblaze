using FitBlaze.Features.Wiki.Services;
using FitBlaze.Features.Wiki.Models;
using Moq;
using Xunit;

namespace FitBlaze.Tests.Features.Wiki.Services
{
    public class MarkupOrchestratorTests
    {
        [Fact]
        public void Render_ShouldUseCorrectEngine_WhenTypeIsRegistered()
        {
            // Arrange
            var markdownEngineMock = new Mock<IMarkupEngine>();
            markdownEngineMock.Setup(e => e.Type).Returns(MarkupType.Markdown);
            markdownEngineMock.Setup(e => e.Render(It.IsAny<string>())).Returns("Markdown Rendered");

            var legacyEngineMock = new Mock<IMarkupEngine>();
            legacyEngineMock.Setup(e => e.Type).Returns(MarkupType.LegacyFitNesse);
            legacyEngineMock.Setup(e => e.Render(It.IsAny<string>())).Returns("Legacy Rendered");

            var engines = new[] { markdownEngineMock.Object, legacyEngineMock.Object };
            var orchestrator = new MarkupOrchestrator(engines);

            // Act
            var resultMarkdown = orchestrator.Render("test", MarkupType.Markdown);
            var resultLegacy = orchestrator.Render("test", MarkupType.LegacyFitNesse);

            // Assert
            Assert.Equal("Markdown Rendered", resultMarkdown);
            Assert.Equal("Legacy Rendered", resultLegacy);
        }

        [Fact]
        public void Render_ShouldFallbackToMarkdown_WhenTypeIsNotRegistered()
        {
            // Arrange
            var markdownEngineMock = new Mock<IMarkupEngine>();
            markdownEngineMock.Setup(e => e.Type).Returns(MarkupType.Markdown);
            markdownEngineMock.Setup(e => e.Render(It.IsAny<string>())).Returns("Fallback Rendered");

            var engines = new[] { markdownEngineMock.Object };
            var orchestrator = new MarkupOrchestrator(engines);

            // Act
            // Assuming we might have a hypothetical missing type, or just simulating missing registration
            // Since enum is fixed, let's simulate by NOT passing legacy engine
            var result = orchestrator.Render("test", MarkupType.LegacyFitNesse);

            // Assert
            Assert.Equal("Fallback Rendered", result);
        }
        
        [Fact]
        public void Render_ShouldReturnOriginalContent_WhenNoEnginesAvailable()
        {
             // Arrange
            var engines = Enumerable.Empty<IMarkupEngine>();
            var orchestrator = new MarkupOrchestrator(engines);

            // Act
            var result = orchestrator.Render("Original Content", MarkupType.Markdown);

            // Assert
            Assert.Equal("Original Content", result);
        }
    }
}
