using System;
using System.Collections.Generic;
using Tasks.ClockFace2;
using Xunit;

namespace Tasks.Tests
{
    public class ClockFace2SolutionTests
    {

        [Theory]
        [InlineData(0.0, 0.0)]
        [InlineData(15.0, 180.0)]
        [InlineData(30.0, 0.0)]
        [InlineData(45.0, 180.0)]
        [InlineData(90.0, 0.0)]
        [InlineData(135.0, 180.0)]
        [InlineData(180.0, 0.0)]
        [InlineData(270.0, 0.0)]
        [InlineData(315.0, 180.0)]
        [InlineData(359.0, 348.0)]
        [InlineData(32.5, 30.0)]
        public void Calculate_WithValidInput_ReturnsCorrectAngle(double hourAngle, double expected)
        {
            var solution = new Tasks.ClockFace2.ClockFace2();
            // Act
            double result = solution.Calculate(hourAngle);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(-1.0)]
        [InlineData(-0.1)]
        [InlineData(360.0)]
        [InlineData(400.0)]
        public void Calculate_WithInvalidInput_ThrowsArgumentOutOfRangeException(double invalidAngle)
        {
            var solution = new Tasks.ClockFace2.ClockFace2();
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => solution.Calculate(invalidAngle));
        }
    }
}