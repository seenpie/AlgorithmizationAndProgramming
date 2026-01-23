using System;
using System.Collections.Generic;
using Tasks.FormNumber;
using Xunit;

namespace Tasks.Tests
{
    public class FormNumberTests
    {
        // Строка, которую мы ожидаем, если решения нет
        private const string ImpossibleMsg = IFormNumberSolution.ERROR_MESSAGE;

        public static IEnumerable<object[]> GetSolutions()
        {
            // Сюда подставится твоя реализация
            yield return new object[] { new Tasks.FormNumber.FormNumber() };
            // yield return new object[] { new FormNumberAlt() };
        }

        #region Тесты из ТЗ (Точное совпадение)

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Example1_MixedOps(IFormNumberSolution solution)
        {
            // Входные: num = 25; a = 3; b = 4
            // Ожидаем: *4 *4 +3 +3 +3 (с возможными пробелами)
            // Логика: 1 *4=4 -> *4=16 -> +3=19 -> +3=22 -> +3=25

            var result = solution.FindPathToNumber(25, 3, 4);

            // Проверяем математику
            AssertCalculationLogic(25, 3, 4, result);

            // Проверяем формат строки (обрезаем крайние пробелы для надежности)
            // Ожидается "*4 *4 +3 +3 +3"
            Assert.Contains("*4 *4 +3 +3 +3", result.Trim());
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Example2_AdditionOnly(IFormNumberSolution solution)
        {
            // Входные: num = 13; a = 3; b = 5
            var result = solution.FindPathToNumber(13, 3, 5);

            AssertCalculationLogic(13, 3, 5, result);
            // Проверка на наличие конкретных подстрок
            Assert.Contains("+3 +3 +3 +3", result.Trim());
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Example3_ExactOutputFormat(IFormNumberSolution solution)
        {
            // Входные: num = 50; a = 7; b = 5
            // Вывод: +7 +7 +7 +7 +7 +7 +7
            var result = solution.FindPathToNumber(50, 7, 5);

            AssertCalculationLogic(50, 7, 5, result);

            // Если реализация добавляет пробел в начале, Trim() это обработает
            Assert.Equal("+7 +7 +7 +7 +7 +7 +7", result.Trim());
        }

        #endregion

        #region Логические тесты (Математика)

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ValuesAlreadyMatch(IFormNumberSolution solution)
        {
            // num = 1. Мы уже находимся в 1. Операций не требуется.
            // Можно вернуть пустую строку.
            var result = solution.FindPathToNumber(1, 5, 5);
            Assert.True(string.IsNullOrWhiteSpace(result), "Если число уже равно 1, действий быть не должно");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_SingleMultiplication(IFormNumberSolution solution)
        {
            // 1 * 10 = 10
            var result = solution.FindPathToNumber(10, 2, 10);

            Assert.Contains("*10", result);
            AssertCalculationLogic(10, 2, 10, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ShortPathViaMult(IFormNumberSolution solution)
        {
            // Цель 100. a=1, b=100.
            // Можно сделать +1 (99 раз), а можно *100 (1 раз).
            // Рекурсия обычно находит кратчайший путь или первый подходящий.
            // Ожидаем "*100"
            var result = solution.FindPathToNumber(100, 1, 100);

            Assert.Contains("*100", result);
            Assert.DoesNotContain("+1 +1", result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ComplexRecursivePath(IFormNumberSolution solution)
        {
            // Цель 11. a=2, b=3.
            // Путь: 1 -> *3 (3) -> *3 (9) -> +2 (11).
            var result = solution.FindPathToNumber(11, 2, 3);

            AssertCalculationLogic(11, 2, 3, result);
            // Проверим порядок операций
            Assert.Matches(@"\*3\s*\*3\s*\+2", result.Trim());
        }

        #endregion

        #region Невозможные сценарии (Impossible Cases)

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Impossible_Simple(IFormNumberSolution solution)
        {
            // Цель 2. a=5, b=5.
            // Из 1 можно получить сразу 6 или 5. 2 пропускаем.
            var result = solution.FindPathToNumber(2, 5, 5);

            Assert.Equal(ImpossibleMsg, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Impossible_ParityCheck(IFormNumberSolution solution)
        {
            // Цель: 4 (четное).
            // a=2, b=6.
            // Старт 1 (нечет).
            // 1 + 2 = 3 (нечет). 3 + 2 = 5... Всегда нечетные при сложении.
            // 1 * 6 = 6 (чет), но перепрыгнули 4.
            // Получить 4 невозможно.
            var result = solution.FindPathToNumber(4, 2, 6);

            Assert.Equal(ImpossibleMsg, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Impossible_TargetIsZero(IFormNumberSolution solution)
        {
            // Цель 0. С помощью сложения и умножения (a,b > 0) из 1 не получить 0.
            var result = solution.FindPathToNumber(0, 3, 3);

            Assert.Equal(ImpossibleMsg, result);
        }

        #endregion

                #region Дополнительные сложные тесты (More Advanced)

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_GreedyTrap_AddBeforeMult(IFormNumberSolution solution)
        {
            // Цель 14. a=6, b=2.
            // Если алгоритм пытается сначала умножать (1*2=2, 2*2=4, 4*2=8, 8*2=16 > 14), он проиграет.
            // Правильный путь: сначала сложить, потом умножить.
            // 1 + 6 = 7 -> 7 * 2 = 14.
            var result = solution.FindPathToNumber(14, 6, 2);

            AssertCalculationLogic(14, 6, 2, result);
            Assert.Contains("+6 *2", result.Trim()); // Здесь порядок важен для оптимальности
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_GreedyTrap_MultBeforeAdd(IFormNumberSolution solution)
        {
            // Цель 10. a=8, b=2.
            // Если жадно складывать (1+8=9, перелет), тупик.
            // Правильный путь:
            // 1 * 2 = 2 -> 2 + 8 = 10.
            var result = solution.FindPathToNumber(10, 8, 2);

            AssertCalculationLogic(10, 8, 2, result);
            Assert.Contains("*2 +8", result.Trim());
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_Overshoot_Immediate(IFormNumberSolution solution)
        {
            // Цель 4. a=5, b=5.
            // 1 + 5 = 6 ( > 4)
            // 1 * 5 = 5 ( > 4)
            // Сразу перелет, даже в один шаг.
            var result = solution.FindPathToNumber(4, 5, 5);
            Assert.Equal(ImpossibleMsg, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_LargeSteps_ExactLanding(IFormNumberSolution solution)
        {
            // Цель 100. a=50, b=50.
            // Пути из 1:
            // 1 + 50 = 51 -> +50 = 101 (Мимо)
            // 1 * 50 = 50 -> *50 = 2500 (Мимо)
            // Но: 1 * 50 = 50 -> + 50 = 100 (Попали!)
            var result = solution.FindPathToNumber(100, 50, 50);

            AssertCalculationLogic(100, 50, 50, result);
            Assert.Contains("*50 +50", result.Trim());
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_LongChain_SmallAdd(IFormNumberSolution solution)
        {
            // Цель 15. a=1, b=100.
            // Умножение сразу перекидывает за 15.
            // Единственный путь - 14 раз прибавить 1.
            // Это проверяет, что рекурсия не падает слишком рано.
            var result = solution.FindPathToNumber(15, 1, 100);

            AssertCalculationLogic(15, 1, 100, result);
            // В строке должно быть много "+1"
            Assert.Contains("+1 +1", result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ModuloImpossible(IFormNumberSolution solution)
        {
            // Цель 11. a=3, b=3.
            // Анализ остатков от деления на 3:
            // Старт: 1 (остаток 1).
            // Операция (+3): (x + 3) % 3 == x % 3. Остаток не меняется (всегда 1).
            // Операция (*3): (x * 3) % 3 == 0. Остаток становится 0.
            // Таким образом, мы можем получить числа с остатком 1 (1, 4, 7, 10, 13...)
            // Или числа с остатком 0 (3, 6, 9, 12...), если хоть раз умножим.
            // Цель 11 имеет остаток 2 (11 % 3 == 2).
            // Получить 11 невозможно.
            var result = solution.FindPathToNumber(11, 3, 3);

            Assert.Equal(ImpossibleMsg, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ValuesOne(IFormNumberSolution solution)
        {
            // Граничный случай: умножение на 1.
            // a=2, b=1. Цель 5.
            // 1 * 1 = 1 (бесполезная операция, потенциальный бесконечный цикл, если не обработать).
            // Решение только через сложение: 1 -> +2 -> 3 -> +2 -> 5.
            var result = solution.FindPathToNumber(5, 2, 1);

            AssertCalculationLogic(5, 2, 1, result);
            Assert.Contains("+2 +2", result.Trim());
            // Убедимся, что нет бессмысленных умножений на 1
            Assert.DoesNotContain("*1", result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ExactMatch_OneAdd(IFormNumberSolution solution)
        {
            // 1 -> +4 -> 5.
            string result = solution.FindPathToNumber(5, 4, 10);
            Assert.Equal("+4", result.Trim());
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void FindPathToNumber_ExactMatch_OneMult(IFormNumberSolution solution)
        {
            // 1 -> *5 -> 5.
            string result = solution.FindPathToNumber(5, 4, 5);
            Assert.Equal("*5", result.Trim());
        }

        #endregion


                #region Corner Cases (Граничные случаи и Стресс-тесты)

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_B_IsZero(IFormNumberSolution solution)
        {
            // Кейс: b = 0. Умножение на 0 превращает число в 0.
            // Из 0 (через +a, где a > 0) выбраться можно, теоретически,
            // но обычно в таких задачах путь 1 -> 0 считается тупиком, так как num > 0.
            // Цель 7. a=2, b=0.
            // Путь: 1 +2=3 +2=5 +2=7. (Умножение убивает ветку).
            var result = solution.FindPathToNumber(7, 2, 0);

            AssertCalculationLogic(7, 2, 0, result);
            Assert.Contains("+2 +2 +2", result.Trim());
            Assert.DoesNotContain("*0", result); // Умножать на 0 бессмысленно
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_A_IsZero(IFormNumberSolution solution)
        {
            // Кейс: a = 0. Прибавление 0 не меняет число (возможен бесконечный цикл: 1+0=1+0...).
            // Цель 4. a=0, b=2.
            // Единственный путь: 1 *2=2 *2=4.
            // Если код попытается сделать +0, он может зависнуть.
            var result = solution.FindPathToNumber(4, 0, 2);

            AssertCalculationLogic(4, 0, 2, result);
            Assert.Contains("*2 *2", result.Trim());
            Assert.DoesNotContain("+0", result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_B_IsOne(IFormNumberSolution solution)
        {
            // Кейс: b = 1. Умножение на 1 не меняет число.
            // Опасность бесконечного цикла (1*1*1...).
            // Цель 3. a=1, b=1.
            // Путь только сложением: 1 +1=2 +1=3.
            var result = solution.FindPathToNumber(3, 1, 1);

            AssertCalculationLogic(3, 1, 1, result);
            Assert.Equal("+1 +1", result.Trim()); // Умножения быть не должно
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_TargetIsStart(IFormNumberSolution solution)
        {
            // Цель = 1. Старт = 1.
            // Никаких действий не требуется.
            // Должна вернуться пустая строка (или null, в зависимости от реализации,
            // но обычно пустая строка для совместимости с AssertCalculationLogic).
            var result = solution.FindPathToNumber(1, 5, 5);

            // Либо пусто, либо пробелы
            Assert.True(string.IsNullOrWhiteSpace(result), $"Ожидалась пустая строка, получено: '{result}'");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_TargetLessThanStart(IFormNumberSolution solution)
        {
            // Цель = 0. Старт = 1.
            // Операции +a и *b (при a,b > 0) только увеличивают число.
            // Получить 0 невозможно.
            var result = solution.FindPathToNumber(0, 3, 3);
            Assert.Equal(ImpossibleMsg, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_RecursionDepth(IFormNumberSolution solution)
        {
            // Проверка на StackOverflow или производительность.
            // Цель 200. a=1, b=2000 (умножение недоступно, так как сразу перелет).
            // Придется сделать 199 операций сложения +1.
            // Это проверяет, выдержит ли стек такую глубину.
            var result = solution.FindPathToNumber(200, 1, 2000);

            AssertCalculationLogic(200, 1, 2000, result);
            // Простая проверка, что строка длинная
            Assert.True(result.Length > 100, "Строка решения должна быть длинной для 199 сложений");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_PrimeNumberTarget(IFormNumberSolution solution)
        {
            // Цель 17 (простое число). a=2, b=3.
            // Множители не помогут попасть ровно в 17 напрямую делением.
            // Сложный путь:
            // 1 *3=3 *3=9 *2(нет) ...
            // 1 +2=3 +2=5 +2=7 *3=21 (перелет)
            // 1+2=3, 3*3=9, 9+2+2+2+2 = 17 (долго)
            // или 1*3=3, 3*3=9, 9+2=11, 11+2=13, 13+2=15, 15+2=17.
            // Главное - найти хоть какой-то путь.
            var result = solution.FindPathToNumber(17, 2, 3);

            AssertCalculationLogic(17, 2, 3, result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void Test_Corner_LargeNumbers_A(IFormNumberSolution solution)
        {
            // Цель 100. a = 99. b = 100.
            // Варианты:
            // 1 * 100 = 100 (1 шаг)
            // 1 + 99 = 100 (1 шаг)
            // Оба решения валидны. Тест должен принять любое валидное.
            var result = solution.FindPathToNumber(100, 99, 100);

            AssertCalculationLogic(100, 99, 100, result);
            // Либо паc, либо паc
            bool isAdd = result.Contains("+99");
            bool isMult = result.Contains("*100");
            Assert.True(isAdd || isMult, "Должно быть использовано либо сложение, либо умножение");
        }

        #endregion


        [Theory(Timeout = 1000)]
        [MemberData(nameof(GetSolutions))]
        public void Test_Crash_RecursiveDeadEnd_ZeroA(IFormNumberSolution solution)
        {
            // num = 6, a = 0, b = 2
            // Стек вызовов:
            // 1. FindPath(6) -> Делим на 2 -> Вызываем FindPath(3)
            // 2. FindPath(3) -> 3 % 2 != 0 -> Идем вычитать
            // 3. FindPath(3) -> Вычитаем 0 -> Вызываем FindPath(3) ... КРАШ.
            // Так как внутренний вызов завис, внешний (для 6) тоже никогда не завершится.
            var result = solution.FindPathToNumber(6, 0, 2);
        }




        #region Helpers

        /// <summary>
        /// Разбирает строку операций и проверяет математическую верность пути.
        /// </summary>
        private void AssertCalculationLogic(uint target, int a, int b, string output)
        {
            // Если строка пустая и цель != 1, это ошибка
            if (string.IsNullOrWhiteSpace(output))
            {
                if (target == 1) return;
                Assert.Fail($"Вернулась пустая строка, хотя ожидался путь к {target}");
            }

            // Если вернулось сообщение об ошибке, фейлим тест (так как этот метод для VALID кейсов)
            if (output == ImpossibleMsg)
            {
                Assert.Fail($"Метод вернул '{ImpossibleMsg}', хотя решение должно существовать для числа {target}");
            }

            // Разбиваем по пробелам, игнорируя пустые элементы (решает проблему " +7 +7")
            var operations = output.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            long current = 1; // используем long во избежание переполнения при проверке промежуточных

            foreach (var op in operations)
            {
                if (op.StartsWith($"*{b}"))
                {
                    current *= b;
                }
                else if (op.StartsWith($"+{a}"))
                {
                    current += a;
                }
                else
                {
                    Assert.Fail($"Некорректная операция в строке '{output}'. Найдено '{op}', ожидалось +{a} или *{b}");
                }
            }

            Assert.Equal((long)target, current);
        }

        #endregion
    }
}
