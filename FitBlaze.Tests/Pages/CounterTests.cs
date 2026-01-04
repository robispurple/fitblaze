using Bunit;
using FitBlaze.Pages;
using FluentAssertions;
using Xunit;

namespace FitBlaze.Tests.Pages
{
    public class CounterTests : BunitContext
    {
        [Fact]
        public void Counter_DisplaysInitialCount()
        {
            // Arrange & Act
            var cut = Render<Counter>();

            // Assert
            cut.Find("p[role='status']").TextContent.Should().Contain("Current count: 0");
        }

        [Fact]
        public void Counter_IncrementsCountOnButtonClick()
        {
            // Arrange
            var cut = Render<Counter>();

            // Act
            cut.Find("button").Click();

            // Assert
            cut.Find("p[role='status']").TextContent.Should().Contain("Current count: 1");
        }

        [Fact]
        public void Counter_IncrementsCountMultipleTimes()
        {
            // Arrange
            var cut = Render<Counter>();

            // Act
            var button = cut.Find("button");
            button.Click();
            button.Click();
            button.Click();

            // Assert
            cut.Find("p[role='status']").TextContent.Should().Contain("Current count: 3");
        }
    }
}
