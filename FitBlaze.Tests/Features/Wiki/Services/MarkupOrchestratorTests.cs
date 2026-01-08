using Xunit;
using Moq;
using FitBlaze.Features.Wiki.Services;
using FitBlaze.Features.Wiki.Models;
using System.Collections.Generic;
using System;

namespace FitBlaze.Tests.Features.Wiki.Services
{
    public class MarkupOrchestratorTests
    {
        [Fact]
        public void Render_ShouldUseCorrectRenderer()
        {
            // Arrange
            var mockRendererFitNesse = new Mock<IMarkupRenderer>();
            mockRendererFitNesse.Setup(r => r.SupportedType).Returns(MarkupType.FitNesse);
            mockRendererFitNesse.Setup(r => r.Render(It.IsAny<string>())).Returns("FitNesse Rendered");

            var mockRendererMarkdown = new Mock<IMarkupRenderer>();
            mockRendererMarkdown.Setup(r => r.SupportedType).Returns(MarkupType.CommonMark);
            mockRendererMarkdown.Setup(r => r.Render(It.IsAny<string>())).Returns("Markdown Rendered");

            var orchestrator = new MarkupOrchestrator(new List<IMarkupRenderer> { mockRendererFitNesse.Object, mockRendererMarkdown.Object });

            // Act
            var result = orchestrator.Render("content", MarkupType.CommonMark);

            // Assert
            Assert.Equal("Markdown Rendered", result);
        }

        [Fact]
        public void Render_ShouldThrow_WhenNoRendererFound()
        {
            // Arrange
            var orchestrator = new MarkupOrchestrator(new List<IMarkupRenderer>());

            // Act & Assert
            Assert.Throws<NotSupportedException>(() => orchestrator.Render("content", MarkupType.FitNesse));
        }
    }
}
