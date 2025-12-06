using System;
using System.Collections.Generic;
using Tasks.BuildTriangle;
using Xunit;

namespace Tasks.Tests
{
    public class BuildTriangleTests
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            var solutions = new IBuildTriangleSolution[]
            {
                new Tasks.BuildTriangle.BuildTriangle(),
            };

            var testPoints = new ((int a, int b, int c), string expected)[]
            {
                ((4, 5, 6), "YES"),
                ((2, 9, 4), "NO"),
                ((4, 5, -6), "NO"),
                ((1, 1, 1), "YES"),
                ((1, 2, 3), "NO"), // 1+2 = 3 → нестрогое неравенство → нельзя
                ((0, 5, 6), "NO"),
                ((10, 2, 9), "YES")
            };

            foreach (var solution in solutions)
            {
                foreach (var (x, expected) in testPoints)
                {
                    yield return new object[] { solution, x, expected };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void CheckFunctionLogic(IBuildTriangleSolution solution, (int a, int b, int c) sides, string expected)
        {
            // Act
            string actual = solution.Calculate(sides.a, sides.b, sides.c);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
