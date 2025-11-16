using System;
using System.Collections.Generic;
using Tasks.RomeNumbers;
using Xunit;

namespace Tasks.Tests
{
    public class RomeNumbersTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.RomeNumbers.RomeNumbers() };
        }

        [Theory]
        [InlineData(1, "I")]
        [InlineData(2, "II")]
        [InlineData(3, "III")]
        [InlineData(4, "IV")]
        [InlineData(5, "V")]
        [InlineData(6, "VI")]
        [InlineData(7, "VII")]
        [InlineData(8, "VIII")]
        [InlineData(9, "IX")]
        [InlineData(10, "X")]
        [InlineData(11, "XI")]
        [InlineData(14, "XIV")]
        [InlineData(15, "XV")]
        [InlineData(19, "XIX")]
        [InlineData(20, "XX")]
        [InlineData(25, "XXV")]
        [InlineData(30, "XXX")]
        [InlineData(40, "XL")]
        [InlineData(50, "L")]
        [InlineData(60, "LX")]
        [InlineData(70, "LXX")]
        [InlineData(80, "LXXX")]
        [InlineData(90, "XC")]
        [InlineData(99, "XCIX")]
        [InlineData(100, "C")]
        public void Convert_WithValidInput_ReturnsCorrectRoman(int input, string expected)
        {
            // Arrange
            var solution = new Tasks.RomeNumbers.RomeNumbers();

            // Act
            var result = solution.Convert(input);

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Convert_WithZero_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var solution = new Tasks.RomeNumbers.RomeNumbers();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => solution.Convert(0));
        }

        [Fact]
        public void Convert_WithOneHundredOne_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var solution = new Tasks.RomeNumbers.RomeNumbers();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => solution.Convert(101));
        }
    }
}
