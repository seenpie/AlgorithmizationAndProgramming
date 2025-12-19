using System;
using System.Collections.Generic;
using Tasks.SegmentsInTheConsole;
using Xunit;

namespace Tasks.Tests
{
    public class SegmentsInTheConsoleTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.SegmentsInTheConsole.SegmentsInTheConsole() };
        }

        public static IEnumerable<object[]> GetTestScenario()
        {
            var solutions = GetSolutions().ToArray();
            var data = GetTestData().ToArray();

            foreach (var solution in solutions)
            {
                foreach (var testCase in data)
                {
                    yield return [solution[0], testCase[0], testCase[1]];
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestScenario))]
        public void RunTests(ISegmentsInTheConsoleSolution solution, int[] input, string expected)
        {
            // Act
            var result = solution.CreateConsolePic(input);

            // Assert
            Assert.Equal(expected, result);
        }
        private static IEnumerable<object[]> GetTestData()
        {
            // Твои оригинальные тесты
            yield return new object[] { new int[] { 12, 3, 2, 5 }, "---|--|-----" };
            yield return new object[] { new int[] { 12, 3, 2, 17 }, "-|-|--------" };
            yield return new object[] { new int[] { 12, 3, 1, 25 }, "Error!" };
            yield return new object[] { new int[] { 22, 3, 2, 5 }, "------|----|----------" };

            // 1. Один отрезок (нет разделителей)
            // yield return new object[] { new int[] { 5, 10 }, "-----" };

            // 2. Минимальная ширина: ровно на символы + разделители
            // yield return new object[] { new int[] { 3, 1, 1, 1 }, "-|-|-" }; // 3 тире + 2 разделителя = 5? Нет! → Wait!

            // ⚠️ Важно: ширина = 3, отрезков = 3 → нужно 3 тире + 2 "|" = 5 символов → невозможно!
            // Значит, такой ввод должен дать "Error!" (потому что на каждый отрезок достанется <1)

            // Пример: ширина = 5, отрезки = [1,1,1] → тире: 5 - 2 = 3 → по 1 на каждый → "-|-|-"
            // yield return new object[] { new int[] { 5, 1, 1, 1 }, "-|-|-" };

            // 3. Ширина в точности равна числу разделителей + 1 на каждый отрезок
            // yield return new object[] { new int[] { 5, 1, 1, 1 }, "-|-|-" };
            // Исправим: для 4 отрезков нужно 3 разделителя → мин. ширина = 4 + 3 = 7
            // yield return new object[] { new int[] { 7, 1, 1, 1, 1 }, "-|-|-|-" };

            // 4. Большой разброс значений (проверка округления)
            // yield return new object[] { new int[] { 10, 1, 9 }, "-" + new string('|', 0) + "---------" }; // "-|---------" → длина 10
            // Но: разделитель один → тире: 10 - 1 = 9
            // доля: 1/10 * 9 = 0.9 → округляется до 1
            // доля: 9/10 * 9 = 8.1 → округляется до 8
            // Итого: 1 + 8 = 9 тире + 1 "|" = 10 → "-|--------" (8 тире)
            // Но 1 + 8 = 9 → ширина = 10 → верно.
            // yield return new object[] { new int[] { 10, 1, 9 }, "-|--------" };

            // 5. Округление вверх: 0.5 → 1
            // yield return new object[] { new int[] { 5, 1, 1 }, "--|--" };
            // сумма = 2, ширина под тире = 5 - 1 = 4
            // 1/2 * 4 = 2 → "--", "--" → "--|--" (длина 5) → верно

            // 6. Округление, где один получает 0 → Error!
            // yield return new object[] { new int[] { 3, 1, 100 }, "Error!" };
            // сумма = 101, ширина под тире = 3 - 1 = 2
            // 1/101 * 2 ≈ 0.019 → округляется до 0 → Error!

            // 7. Все числа одинаковые
            // yield return new object[] { new int[] { 9, 2, 2, 2 }, "--|--|---" };
            // сумма = 6, тире = 9 - 2 = 7
            // каждый: 2/6 * 7 ≈ 2.33 → округляется до 2 → 2+2+2=6 тире + 2 "|" = 8, но нужно 9!
            // Но по твоей логике — округляем каждый отдельно, даже если итог ≠ width
            // 2.33 → 2, 2.33 → 2, 2.33 → 2 → итого 6+2=8, но ширина 9 → возможно, но по условию — ок?
            // Однако в твоих примерах так и делается.
            // Но давай точнее: 7 * 2 / 6 = 2.333 → Math.Round → 2
            // Так что "---|---|---" = 2+1+2+1+2 = 8, а не 9 → но в условии не сказано, что итог должен быть строго width!
            // Поэтому оставим, но лучше пример с точным совпадением:

            // Лучше: 10, [1,1,1,1] → тире = 10 - 3 = 7 → 7/4 = 1.75 → 2,2,2,1 → итого 7
            // yield return new object[] { new int[] { 10, 1, 1, 1, 1 }, "--|--|--|-" }; // 2+2+2+1=7 тире + 3 "|" = 10

            // 8. Минимально возможный корректный ввод
            // yield return new object[] { new int[] { 1, 1 }, "-" };

            // 9. Ширина слишком мала для количества отрезков
            // yield return new object[] { new int[] { 2, 1, 1, 1 }, "Error!" }; // нужно минимум 3 + 2 = 5? Нет:
            // отрезков = 3 → разделителей = 2 → мин. ширина = 3 (по 1 тире) + 2 = 5
            // но ширина = 2 → тире = 2 - 2 = 0 → все сегменты получат 0 → Error!

            // 10. Большое количество маленьких отрезков
            // yield return new object[] { new int[] { 20, 1, 1, 1, 1, 1, 1, 1, 1 }, "Error!" }; // 8 тире + 7 "|" = 15 → но ширина 20
            // На самом деле: тире = 20 - 7 = 13 → 13/8 ≈ 1.625 → 2,2,2,2,2,2,2,1 → итого 13
            // Но для простоты возьмём другой пример:

            // Более предсказуемый:
            // yield return new object[] { new int[] { 15, 1, 1, 1, 1, 1 }, "--|--|--|--|---" };
            // Лучше избегать таких. Возьмём точный:

            // Точный пример: ширина = 11, отрезки = [1,2,3]
            // сумма = 6, тире = 11 - 2 = 9
            // 1/6*9 = 1.5 → 2
            // 2/6*9 = 3 → 3
            // 3/6*9 = 4.5 → 4 или 5? Math.Round(4.5) → 4 (в .NET — банковское округление!)
            // → 2 + 3 + 4 = 9 → итого: "--|---|----" → длина: 2+1+3+1+4 = 11
            // yield return new object[] { new int[] { 11, 1, 2, 3 }, "--|---|----" };

            // 11. Случай с банковским округлением (0.5 → к ближайшему чётному)
            // Math.Round(2.5) = 2, Math.Round(3.5) = 4
            // Пример: ширина = 7, [1, 1, 1, 1] → тире = 7 - 3 = 4 → каждый 1 → 1.0 → "-"
            // Не подходит. Возьмём: ширина = 9, [1, 3] → сумма=4, тире=8
            // 1/4*8=2, 3/4*8=6 → "--|------" → длина 9
            // yield return new object[] { new int[] { 9, 1, 3 }, "--|------" };

            // 12. Один отрезок, ширина = 100
            // yield return new object[] { new int[] { 100, 42 }, new string('-', 100) };
        }
    }
}
