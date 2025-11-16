using System;
using System.Collections.Generic;
using Tasks.ClockFace;
using Xunit;

namespace Tasks.Tests
{
    public class ClockFaceTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.ClockFace.ClockFace() };
        }

        public static IEnumerable<object[]> GetValidTestCases()
        {
            yield return new object[] { 0, 0, 0, 0 };
            yield return new object[] { 1, 0, 0, 30 };
            yield return new object[] { 2, 0, 0, 60 };
            yield return new object[] { 3, 0, 0, 90 };
            yield return new object[] { 6, 0, 0, 180 };
            yield return new object[] { 0, 1, 0, 0.500 };
            yield return new object[] { 0, 0, 1, 0.008 };
            yield return new object[] { 0, 30, 0, 15 };
            yield return new object[] { 0, 0, 30, 0.250 };
            yield return new object[] { 3, 30, 0, 105 };
            yield return new object[] { 11, 59, 59, 359.992 };
        }

        public static IEnumerable<object[]> GetInvalidTestCases()
        {
            yield return new object[] { -1, 0, 0 };
            yield return new object[] { 12, 0, 0 };
            yield return new object[] { 13, 0, 0 };
            yield return new object[] { 0, -1, 0 };
            yield return new object[] { 0, 60, 0 };
            yield return new object[] { 0, 0, -1 };
            yield return new object[] { 0, 0, 60 };
            yield return new object[] { 11, 59, 60 };
        }

        [Theory]
        [MemberData(nameof(GetValidTestCases))]
        public void Calculate_WithValidInputs_ReturnsCorrectAngle(int h, int m, int s, double expected)
        {
            var solution = new Tasks.ClockFace.ClockFace();
            double result = solution.Calculate(h, m, s);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(GetInvalidTestCases))]
        public void Calculate_WithInvalidInputs_ThrowsArgumentOutOfRangeException(int h, int m, int s)
        {
            var solution = new Tasks.ClockFace.ClockFace();
            Assert.Throws<ArgumentOutOfRangeException>(() => solution.Calculate(h, m, s));
        }
    }
}
