using System;

using Tasks.Common;

namespace Tasks.Palindrom
{
    public class Palindrom : IPalindromSolution
    {
        public void Run()
        {
            Console.WriteLine("Введите строку");
            string str = Console.ReadLine();
            string isPalindromMessage = IsPalindrom(str) ? "палиндром" : "не палиндром";
            Console.WriteLine($"Строка '{str}' это {isPalindromMessage}");
        }

        public bool IsPalindrom(string str)
        {
            if (str.Length <= 1) return true;
            char first = char.ToLower(str[0]);
            char last = char.ToLower(str[str.Length - 1]);
            if (first != last) return false;
            return IsPalindrom(str.Substring(1, str.Length - 2));
        }
    }
}
