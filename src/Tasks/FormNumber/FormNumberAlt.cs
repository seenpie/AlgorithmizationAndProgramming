namespace Tasks.FormNumber;

public class FormNumberAlt : IFormNumberSolution
{
    public void Run()
    {
        (uint num, uint a, uint b) = ReadNumberParams();
        string path = FindPathToNumber(num, a, b);
        Console.WriteLine(path);
    }

    public string FindPathToNumber(uint num, uint a, uint b)
    {
        return ComposeNumber(num, a, b);
    }

    private (uint num, uint a, uint b) ReadNumberParams()
    {
        uint num = ReadUintInput("Введите num");
        uint a = ReadUintInput("Введите a");
        uint b = ReadUintInput("Введите b");
        return (num, a, b);
    }

    private uint ReadUintInput(string consoleText)
    {
        Console.WriteLine(consoleText);
        string input = Console.ReadLine();
        if (uint.TryParse(input, out uint num)) return num;
        Console.WriteLine("Ошибка ввода, попробуйте еще раз");
        return ReadUintInput(consoleText);
    }

    private string ComposeNumber(uint num, uint a, uint b)
    {
        (int addCount, int mulCount) = CalculateRepeats(num, a, b);
        return CreateSequenceLine(a, addCount, b, mulCount);
    }

    // решает уравнение b^m=num-an (1)
    // если в diff = 1, то либо num = 1, либо добито n-ками до 1 => решение есть через +
    // если m - целое, значит найдена степень для решения (1)
    private (int addCount, int mulCount) CalculateRepeats(uint num, uint a, uint b, int n = 0)
    {
        long diff = num - a * n;
        switch (diff)
        {
            case <= 0:
                return b == 0 && n == 0 ? (0, 1) : (0, 0);
            case 1:
                return b == 1 && n == 0 ? (0, 1) : (n, 0);
            default:
            {
                double m = Math.Log(diff, b);
                return IsInteger(m) ? (n, (int)m) : CalculateRepeats(num, a, b, n + 1);
            }
        }
    }

    private bool IsInteger(double x, double epsilon = 1e-10)
    {
        return Math.Abs(x - Math.Round(x)) < epsilon;
    }

    private string CreateSequenceLine(uint a, int repeatA, uint b, int repeatB)
    {
        int length = repeatA + repeatB;

        if (length == 0) return IFormNumberSolution.ERROR_MESSAGE;

        List<string> parts = new List<string>(length);
        string bStr = $"*{b}";
        string aStr = $"+{a}";
        for (int i = 0; i < repeatB; i++) parts.Add(bStr);
        for (int i = 0; i < repeatA; i++) parts.Add(aStr);
        return string.Join(" ", parts);
    }
}
