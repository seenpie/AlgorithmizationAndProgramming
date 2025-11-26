using System.Collections.Generic;
using Xunit;
using Tasks.VowelOrConsonant;

namespace Tasks.Tests
{
    public class VowelOrConsonantTests
    {
        public static IEnumerable<object[]> GetTestCases()
        {
            // Все реализации, которые нужно протестировать
            var solutions = new IVowelOrConsonantSolution[]
            {
                new Tasks.VowelOrConsonant.VowelOrConsonant(),
                new VowelOrConsonantAlternative()
            };

            // ГЛАСНЫЕ (все 10 + регистр)
            var vowels = new char[]
            {
                'а', 'у', 'о', 'ы', 'и', 'э', 'я', 'ю', 'ё', 'е',
                'А', 'У', 'О', 'Ы', 'И', 'Э', 'Я', 'Ю', 'Ё', 'Е'
            };

            // СОГЛАСНЫЕ и знаки
            var consonants = new char[]
            {
                'б', 'в', 'г', 'д', 'ж', 'з', 'й', 'к', 'л', 'м',
                'н', 'п', 'р', 'с', 'т', 'ф', 'х', 'ц', 'ч', 'ш', 'щ',
                'ъ', 'ь',
                'Б', 'В', 'Й', 'Ъ'
            };

            // НЕВАЛИДНЫЕ символы
            var invalid = new char[]
            {
                'a', 'e', 'y', 'H',  // латиница
                '1', '!', ' '        // цифра, спецсимвол, пробел
            };

            // Генерация тестов: для каждой реализации — все кейсы
            foreach (var solution in solutions)
            {
                // Гласные → "Гласная"
                foreach (var c in vowels)
                {
                    yield return new object[] { solution, c, "Гласная" };
                }

                // Согласные → "Согласная"
                foreach (var c in consonants)
                {
                    yield return new object[] { solution, c, "Согласная" };
                }

                // Невалидные → "Error"
                foreach (var c in invalid)
                {
                    yield return new object[] { solution, c, "Error" };
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetTestCases))]
        public void Determine_WithVariousChars_ReturnsCorrectCategory(
            IVowelOrConsonantSolution solution,
            char input,
            string expected)
        {
            // Act
            string actual = solution.DetermineLetter(input);

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}
