using Tasks.MathCalculations;
using Xunit;

namespace Tasks.Tests
{
    public class MathCalculationsTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.MathCalculations.MathCalculations() };
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Calculate_WithPositiveInput_ReturnsCorrectValue(IMathCalculationsSolution solution)
        {
            double a = 5;
            double expected = 1.730;

            double actual = solution.Calculate(a);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Calculate_WithInputCausingNegativeSqrt_ReturnsNaN(IMathCalculationsSolution solution)
        {
            // Arrange
            double a = -10;

            // Act
            double actual = solution.Calculate(a);

            // Assert
            Assert.True(double.IsNaN(actual));
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Calculate_WithZeroInput_ReturnsZero(IMathCalculationsSolution solution)
        {
            // Arrange
            double a = 0;
            double expected = 0;

            // Act
            double actual = solution.Calculate(a);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
