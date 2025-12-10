using System;

using Tasks.Common;

namespace Tasks.ConsoleGraphics
{
    public class ConsoleGraphicsTree : IConsoleGraphicsSolution
    {
        public void Run()
        {
            int M = int.Parse(Console.ReadLine()); // кол-во рядов
            int N = int.Parse(Console.ReadLine()); // кол-во колонок
            byte K = byte.Parse(Console.ReadLine()); // высота елки

            string drawing = GetTree(K, N);
            string result = "";

            for (int i = 0; i < M; i++)
            {
                string s = i == M - 1 ? drawing : drawing + Environment.NewLine;
                result += s;
            }

            Console.WriteLine(new string('=', 7) + " Программа ЕЛЫ " + new string('=', 7));
            Console.WriteLine($"M={M}");
            Console.WriteLine($"N={N}");
            Console.WriteLine($"K={K}");
            Console.WriteLine(result);
        }

        private string GetTree(byte rows, int count)
        {
            string pic = "";
            const char element = '#';

            for (int i = 1; i <= rows + 1; i++)
            {
                string s = "";
                for (int j = 0; j < count; j++)
                {
                    int charInRows = i == rows + 1 ? 1 : i;
                    s += GetString(element, rows, charInRows);
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
}
