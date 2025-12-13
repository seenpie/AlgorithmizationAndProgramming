using System;

using Tasks.Common;

namespace Tasks.ConsoleGraphics;

public class ConsoleGraphicsRhomb : IConsoleGraphicsSolution
{
    public void Run()
    {

        int M = int.Parse(Console.ReadLine()); // кол-во рядов
        int N = int.Parse(Console.ReadLine()); // кол-во колонок
        byte K = byte.Parse(Console.ReadLine()); // высота половины ромба

        if (M < 1 || M > 5 || N < 1 || N > 5 || K < 1 || K > 5)
        {
            Console.WriteLine("значения должны быть от 1 до 5");
            return;
        }

        string drawing = GetRhomb(K, N, '@');
        string result = "";

        for (int i = 0; i < M; i++)
        {
            result += drawing;
        }

        Console.WriteLine(new string('=', 7) + " Программа РОМБЫ " + new string('=', 7));
        Console.WriteLine($"M={M}");
        Console.WriteLine($"N={N}");
        Console.WriteLine($"K={K}");
        Console.WriteLine(result);
    }

    private string GetRhomb(byte rows, int count, char symbol)
    {
        string pic = "";

        for (int i = 1; i < rows * 2; i++)
        {
            string s = "";
            for (int j = 0; j < count; j++)
            {
                int charInRows = i > rows ? rows - i % rows : i;
                s += GetString(symbol, rows, charInRows);
            }
            pic += s + Environment.NewLine;
        }

        return pic;
    }

    private string GetString(char el, byte rows, int charInRows)
    {
        return new string(' ' , rows - charInRows) + new string(el, charInRows) + new string(el, charInRows - 1) + new string(' ', rows - charInRows + 1);
    }
}
