using Xunit;
using SPM;

namespace SPM.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Add_WhenCalled_ReturnsSum()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Add(1, 2);

            // Assert
            Assert.Equal(3, result);
        }
    }
}
