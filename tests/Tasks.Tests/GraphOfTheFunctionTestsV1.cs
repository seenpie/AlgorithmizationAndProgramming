using System;
using System.Collections.Generic;
using Tasks.GraphOfTheFunction;
using Xunit;

namespace Tasks.Tests
{
    public class GraphOfTheFunctionTestsV1
    {
        public static IEnumerable<object[]> CalculateTestCasesForAllSolutions
        {
            get
            {
                var solutions = new IGraphOfTheFunctionSolution[]
                {
                    new GraphOfTheFunctionV1()
                };

                var testCases = new (double input, double expected)[]
                {
                    (1.7, 0.3),
                    (-2.2, 0.2),
                    (0.0, 0.0),
                    (10.1, 0.1),
                    (11.1, 0.9),
                    (2, 0)
                };

                foreach (var sol in solutions)
                {
                    foreach (var (input, expected) in testCases)
                    {
                        yield return new object[] { sol, input, expected };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(CalculateTestCasesForAllSolutions))]
        public void SomeTest_WithValidInputs_ReturnsCorrectValueV1(IGraphOfTheFunctionSolution solution, double input, double expected)
        {
            //act
            double actual = solution.Calculate(input);

            //assert
            Assert.Equal(expected, actual, precision: 10);
        }
    }
}
