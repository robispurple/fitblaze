using Bunit;
using FitBlaze.Pages;
using FluentAssertions;
using Xunit;

namespace FitBlaze.Tests.Pages
{
    /// <summary>
    /// Unit tests for the Counter Blazor component.
    /// Tests the counter's ability to display and increment the current count value.
    /// </summary>
    public class CounterTests : BunitContext
    {
        /// <summary>
        /// Verifies that the Counter component displays an initial count of 0.
        /// </summary>
        [Fact]
        public void Counter_DisplaysInitialCount()
        {
            // Arrange & Act
            var cut = Render<Counter>();

            // Assert
            cut.Find("p[role='status']").TextContent.Should().Contain("Current count: 0");
        }

        /// <summary>
        /// Verifies that clicking the button increments the counter by 1.
        /// </summary>
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

        /// <summary>
        /// Verifies that the counter correctly accumulates multiple button clicks.
        /// </summary>
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
