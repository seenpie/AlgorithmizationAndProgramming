using System;
using System.Collections.Generic;
using Tasks.FlatCount;
using Xunit;

namespace Tasks.Tests
{
    public class FlatCountTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.FlatCount.FlatCount() };
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Calculate_WithValidInputs_ReturnsCorrectValue(IFlatCountSolution solution)
        {
            // Test case 1: First flat in building
            var result = solution.Calculate(1, 5, 4);
            Assert.Equal(1, result.entrance);
            Assert.Equal(1, result.floor);

            // Test case 2: Flat in first entrance, second floor
            result = solution.Calculate(5, 5, 4);
            Assert.Equal(1, result.entrance);
            Assert.Equal(2, result.floor);

            // Test case 3: Flat in second entrance, first floor
            result = solution.Calculate(21, 5, 4);
            Assert.Equal(2, result.entrance);
            Assert.Equal(1, result.floor);

            // Test case 4: Larger building
            result = solution.Calculate(100, 10, 10);
            Assert.Equal(1, result.entrance);
            Assert.Equal(10, result.floor);

            // Test case 5: Second entrance
            result = solution.Calculate(101, 10, 10);
            Assert.Equal(2, result.entrance);
            Assert.Equal(1, result.floor);
        }
    }
}