using System;
using System.Collections.Generic;
using Tasks.Boxes;
using Xunit;

namespace Tasks.Tests
{
    public class BoxesTests
    {
        // Метод поставки данных для тестов
        public static IEnumerable<object[]> GetTestCases()
        {
            var solution = new Tasks.Boxes.Boxes();

            // Формат: { solution, a1, b1, c1, a2, b2, c2, expectedResult }

            // 1. Влезает (без вращения)
            yield return new object[] { solution, 1, 2, 3, 2, 3, 4, true };

            // 2. Не влезает (объем маловат)
            yield return new object[] { solution, 5, 5, 5, 2, 2, 2, false };

            // 3. ВРАЩЕНИЕ: Коробка (5, 1, 1), Ящик (2, 6, 2)
            // У коробки размеры: 1, 1, 5
            // У ящика размеры: 2, 2, 6 
            // 1 <= 2, 1 <= 2, 5 <= 6 -> Да
            yield return new object[] { solution, 5, 1, 1, 2, 6, 2, true };

            // 4. НЕ ВЛЕЗАЕТ: Одна сторона торчит
            // Коробка (10, 1, 1) -> Макс 10
            // Ящик (5, 5, 5) -> Макс 5
            // 10 > 5 -> Нет
            yield return new object[] { solution, 10, 1, 1, 5, 5, 5, false };

            // 5. Граничный случай: точное совпадение
            yield return new object[] { solution, 3, 4, 5, 3, 4, 5, true };

            // 6. Точное совпадение, но цифры перепутаны местами
            yield return new object[] { solution, 5, 4, 3, 3, 5, 4, true };
            
            // 7. Проверка Double (с точкой)
            yield return new object[] { solution, 1.5, 2.5, 3.5, 1.5, 2.5, 3.5, true };
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void IsBoxFit_WithoutArrays_ReturnsCorrectValue(
            Tasks.Boxes.Boxes solution, 
            double a1, double b1, double c1,
            double a2, double b2, double c2,
            bool expected)
        {
            // Act
            bool actual = solution.IsBoxFit(a1, b1, c1, a2, b2, c2);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
