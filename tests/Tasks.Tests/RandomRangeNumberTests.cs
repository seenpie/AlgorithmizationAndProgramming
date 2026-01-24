using System;
using System.Collections.Generic;
using System.Linq; // Нужно для .Distinct() и .ToList()
using Tasks.RandomRangeNumber;
using Xunit;

namespace Tasks.Tests
{
    public class RandomRangeNumberTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.RandomRangeNumber.RandomRangeNumber() };
            yield return new object[] { new RandomRangeNumberAlt() };
        }

        // --- БАЗОВЫЕ ПРОВЕРКИ ---

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_WithValidInputs_ReturnsCorrectDivisibleNumber(IRandomRangeNumberSolution solution)
        {
            uint d = 5;
            uint min = 10;
            uint max = 30;
            uint result = solution.GetNumber(d, min, max);

            Assert.True(result >= min);
            Assert.True(result <= max);
            Assert.True(result % d == 0);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_NoNumbersInScope_ThrowsInvalidOperationException(IRandomRangeNumberSolution solution)
        {
            uint d = 10;
            uint min = 1;
            uint max = 4;
            Assert.Throws<InvalidOperationException>(() => solution.GetNumber(d, min, max));
        }

        // --- ПРОВЕРКИ КОРНЕР КЕЙСОВ И МНОЖЕСТВ ---

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_SmallRangeSpecificSet_ReturnsValidValue(IRandomRangeNumberSolution solution)
        {
            // Сценарий из твоего запроса: [3, 9], d = 3
            // Ожидаемые числа: 3, 6, 9
            uint d = 3;
            uint min = 3;
            uint max = 9;
            var expectedValues = new HashSet<uint> { 3, 6, 9 };

            // Запускаем 50 раз, чтобы убедиться, что всегда возвращается корректное значение
            for (int i = 0; i < 50; i++)
            {
                uint result = solution.GetNumber(d, min, max);
                Assert.Contains(result, expectedValues);
            }
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_MaxUintBoundary_DoesNotOverflow(IRandomRangeNumberSolution solution)
        {
            // Корнер кейс: работа с самыми большими числами uint
            // Диапазон: [Max-10, Max], d = 1
            // Это проверка того, что внутри формулы (min + ...), не происходит переполнения,
            // которое выбросило бы ошибку или вернуло 0.

            uint max = uint.MaxValue;       // 4,294,967,295
            uint min = uint.MaxValue - 10;
            uint d = 1;

            uint result = solution.GetNumber(d, min, max);

            Assert.True(result >= min);
            Assert.True(result <= max);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_OnlyOnePossibleValue_AlwaysReturnsIt(IRandomRangeNumberSolution solution)
        {
            // Сценарий: Диапазон широкий, но делитель такой большой, что влезает только одно число.
            // [1, 10], d = 8. Единственное число: 8.
            uint d = 8;
            uint min = 1;
            uint max = 10;

            for (int i = 0; i < 10; i++)
            {
                uint result = solution.GetNumber(d, min, max);
                Assert.Equal((uint)8, result);
            }
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void GetNumber_ProbabilisticDistribution_ReturnsDifferentValues(IRandomRangeNumberSolution solution)
        {
            // Тест на то, что работает именно рандом, а не всегда возвращается первое число.
            // Диапазон [10, 20], d = 5. Варианты: {10, 15, 20}.
            // Если мы вызовем метод 100 раз, крайне маловероятно, что мы получим только одно число.

            uint d = 5;
            uint min = 10;
            uint max = 20;
            var results = new HashSet<uint>();

            for (int i = 0; i < 100; i++)
            {
                results.Add(solution.GetNumber(d, min, max));
            }

            // Проверяем, что мы встретили (вернули) более одного уникального варианта.
            // Если всегда возвращается 10 (ошибка в логике рандома, например minDivisible + 0), тест упадет.
            Assert.True(results.Count > 1, "Метод должен возвращать разные значения при многократном запуске");
        }
    }
}
