using System;
using System.Collections.Generic;
using Tasks.TrigonometricFunctions;
using Xunit;

namespace Tasks.Tests
{
    public class TrigonometricFunctionsV1Tests
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            var solutions = new ITrigonometricFunctionsSolution[]
            {
                new Tasks.TrigonometricFunctions.TrigonometricFunctionsV1()
            };

            var testPoints = new ((double x, double e), double expected)[]
            {
                ((100, 0.0001), Math.Sin(100 * Math.PI / 180)),
                ((71, 0.0001), Math.Sin(71 * Math.PI / 180)),
                ((360, 0.0001), Math.Sin(360 * Math.PI / 180)),
                ((22, 0.0001), Math.Sin(22 * Math.PI / 180)),
                ((1111, 0.01), Math.Sin(1111 * Math.PI / 180)),
            };

            foreach (var solution in solutions)
            {
                foreach (var ((x, e), expected) in testPoints)
                {
                    yield return new object[] { solution, x, e, expected };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void CheckFunctionLogic(ITrigonometricFunctionsSolution solution, double x, double e, double expected)
        {
            // Act
            double actual = solution.Calculate(x, e);

            // Assert
            Assert.Equal(expected, actual, 5);
        }
    }
}
