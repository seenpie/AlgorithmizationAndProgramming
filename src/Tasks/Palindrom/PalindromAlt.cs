namespace Tasks.Palindrom;

public class PalindromAlt : IPalindromSolution
{
    public void Run()
    {
        string input = ReadInput();

        bool isPalindrom = IsPalindrom(input);

        Console.WriteLine($"{input} - {isPalindrom}");
    }

    private string ReadInput()
    {
        Console.WriteLine("Введите строку");

        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Попробуйте еще раз");
            return ReadInput();
        }

        return input;
    }

    public bool IsPalindrom(string str)
    {
        if (str.Length < 2) return true;

        return IsPalindromeRange(str, 0, str.Length - 1);
    }

    private bool IsPalindromeRange(string str, int startIndex, int endIndex)
    {
        if (startIndex >= endIndex) return true;

        if (char.ToLower(str[startIndex]) != char.ToLower(str[endIndex]))
            return false;

        return IsPalindromeRange(str, ++startIndex, --endIndex);
    }
}
