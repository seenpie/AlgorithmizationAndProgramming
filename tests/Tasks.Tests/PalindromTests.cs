using System;
using System.Collections.Generic;
using Tasks.Palindrom;
using Xunit;

namespace Tasks.Tests
{
    public class PalindromTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.Palindrom.Palindrom() };
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_WithEmptyString_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            string input = "";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result, "Пустая строка должна считаться палиндромом (базовый случай).");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_WithSingleCharacter_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            string input = "a";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result, "Строка из одного символа всегда палиндром.");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_WithSimplePalindromeOddLength_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            string input = "topot";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_WithSimplePalindromeEvenLength_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            string input = "abba";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_MixedCasePalindrome_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            // Задача предполагает, что Радар == true, значит регистр не важен
            string input = "RaDaR"; 

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result, "Проверка должна быть регистронезависимой (Радар == radar).");
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_CyrillicExample_ReturnsTrue(IPalindromSolution solution)
        {
            // Arrange
            string input = "Радар";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_NotPalindrome_ReturnsFalse(IPalindromSolution solution)
        {
            // Arrange
            string input = "Макар";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_AlmostPalindrome_ReturnsFalse(IPalindromSolution solution)
        {
            // Arrange
            // Почти палиндром, но последний символ отличается
            string input = "abbx"; 

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.False(result);
        }
        
        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void IsPalindrom_WithSpacesButNotPalindrome_ReturnsFalse(IPalindromSolution solution)
        {
            // Если считать пробелы символами: " a b" != "b a "
            // Обычно в простых задачах пробелы считаются символами, если не сказано обратное.
            string input = "hello world";

            // Act
            bool result = solution.IsPalindrom(input);

            // Assert
            Assert.False(result);
        }
    }
}
