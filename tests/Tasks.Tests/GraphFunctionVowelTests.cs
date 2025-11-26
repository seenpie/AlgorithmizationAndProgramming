using System;
using System.Collections.Generic;
using Tasks.GraphFunctionVowel;
using Xunit;

namespace Tasks.Tests
{
    public class GraphFunctionVowelTests
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            var solutions = new IGraphFunctionVowelSolution[]
            {
                new Tasks.GraphFunctionVowel.GraphFunctionVowel(),
                new Tasks.GraphFunctionVowel.GraphFunctionVowelAlternative(),
            };

            // Определяем все тестовые точки отдельно
            var testPoints = new (double x, double expected)[]
            {
                // Группа 1: Примеры из условия
                (1.7, 0.3),
                (-2.2, 0.2),
                (0.0, 0.0),

                // Группа 2: Целые узловые точки
                (2.0, 0.0),
                (4.0, 0.0),
                (100.0, 0.0),
                (1.0, 1.0),
                (3.0, 1.0),
                (11.0, 1.0),

                // Группа 3: Отрицательные значения
                (-2.0, 0.0),
                (-1.0, 1.0),

                // Группа 4: Серединные значения
                (0.5, 0.5),
                (1.5, 0.5),
                (-0.5, 0.5),
                (-1.5, 0.5),

                // Группа 5: Граничные значения
                (0.1, 0.1),
                (1.9, 0.1),
                (-2.8, 0.8)
            };

            // Для каждой реализации и каждого тест-кейса — отдельный набор данных
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
        public void CheckFunctionLogic(IGraphFunctionVowelSolution solution, double x, double expected)
        {
            // Act
            // Предполагается, что метод называется F. Если Run или Calculate - поправь здесь.
            double actual = solution.F(x);

            // Assert
            // Используем точность до 5 знаков, чтобы избежать ошибок плавающей точки
            Assert.Equal(expected, actual, 5);
        }
    }
}

