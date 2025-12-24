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
            yield return new object[] { new Tasks.SegmentsInTheConsole.SegmentsInTheConsoleAlt() };
            // yield return new object[] { new Tasks.SegmentsInTheConsole.SegmentsInTheConsoleAlt2() };
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
            var result = solution.CreateConsolePic(input);

            Assert.Equal(expected, result);
        }

        private static IEnumerable<object[]> GetTestData()
        {
            // === Оригинальные тесты ===
            yield return new object[] { new int[] { 12, 3, 2, 5 }, "---|--|-----" };
            yield return new object[] { new int[] { 12, 3, 2, 17 }, "-|-|--------" };
            yield return new object[] { new int[] { 12, 3, 1, 25 }, "Error!" };
            yield return new object[] { new int[] { 22, 3, 2, 5 }, "------|----|----------" };

            // === Граничные случаи ===
            yield return new object[] { new int[] { 5, 10 }, "-----" };
            yield return new object[] { new int[] { 100, 42 }, new string('-', 100) };
            yield return new object[] { new int[] { 0, 1, 2 }, "Error!" };
            yield return new object[] { new int[] { 10, 0, 0 }, "Error!" }; // если валидация по сумме важна
            yield return new object[] { new int[] { 5, 1, 1, 1 }, "-|-|-" };
            yield return new object[] { new int[] { 4, 1, 1, 1 }, "Error!" };

            // Округление: 1.5 -> 2. Остаток (4-2=2) последнему.
            // 5, [1,1] -> "--|--" (длина 5)
            yield return new object[] { new int[] { 5, 1, 1 }, "--|--" };

            yield return new object[] { new int[] { 10, 1, 100 }, "Error!" };
            yield return new object[] { new int[] { -12, -3, -2, -5 }, "Error!" };
            yield return new object[] { new int[] { -12, 3, 2, -5 }, "Error!" };
            yield return new object[] { new int[] { 30, 5, 4, 9, 8, 2 }, "-----|----|--------|-------|--"  };
            yield return new object[] { new int[] { 30, 5, 4, 9, 8, 1 }, "Error!"  };

            // === ТЕСТЫ, КОТОРЫЕ РАНЬШЕ ДАВАЛИ 12 СИМВОЛОВ ===

            // 9. Ширина 11, [1, 2, 3]. avail=9. ratio=1.5.
            // Seg 1: 1*1.5=1.5 -> 2 тире. Used=2. Res="--|"
            // Seg 2: 2*1.5=3.0 -> 3 тире. Used=2+3=5. Res="--|---|"
            // Last: Avail(9) - Used(5) = 4. Res="--|---|----"
            // Итог длина: 2+1+3+1+4 = 11. (А раньше было 12!)
            yield return new object[] { new int[] { 11, 1, 2, 3 }, "--|---|----" };

            // 10. Ширина 9, [2, 2, 2]. avail=7. ratio=1.166.
            // Seg 1: 2*1.166=2.33 -> 2 тире. Used=2. Res="--|"
            // Seg 2: 2*1.166=2.33 -> 2 тире. Used=4. Res="--|--|"
            // Last: Avail(7) - Used(4) = 3 тире. Res="--|--|---"
            // Итог длина: 2+1+2+1+3 = 9. (Раньше было 8!)
            yield return new object[] { new int[] { 9, 2, 2, 2 }, "--|--|---" };
            yield return new object[] { new int[] { 10, 1, 1, 1 }, "---|---|--" };

        }
    }
}
