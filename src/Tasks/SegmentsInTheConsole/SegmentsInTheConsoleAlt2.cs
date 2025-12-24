
using System;
using System.Text;
using Tasks.Common;

namespace Tasks.SegmentsInTheConsole
{
    public class SegmentsInTheConsoleAlt2 : ISegmentsInTheConsoleSolution
    {
        private (string input, int[] parsedInput) GetInput()
        {
            while (true)
            {
                Console.WriteLine("Введите данные в формате 'общая ширина линии, длина 1 отрезка, длина 2 отрезка, ..., длина n отрезка'\nПример: 12, 3, 2, 5\nВыйти из программы Ctrl+C");

                string input = Console.ReadLine();

                if (string.IsNullOrEmpty(input)) continue;

                string[] splitInput = input.Split(",");
                int[] parsedInput = new int[splitInput.Length];
                StringBuilder normalizedInput = new StringBuilder();
                bool isValid = true;

                for (int i = 0; i < splitInput.Length; i++)
                {
                    if (!int.TryParse(splitInput[i], out int intValue) || intValue < 0)
                    {
                        Console.WriteLine($"некорректное значение: {splitInput[i]}\nПопробуйте заново");
                        isValid = false;
                        break;
                    }

                    parsedInput[i] = intValue;
                    normalizedInput.Append(parsedInput[i]);
                    if (i < splitInput.Length - 1) normalizedInput.Append(", ");
                }

                if (isValid)
                {
                    return (normalizedInput.ToString(), parsedInput);
                };
            }
        }

        public void Run()
        {
            (string input, int[] parsedInput) = GetInput();
            string pic = CreateConsolePic(parsedInput);

            Console.WriteLine(input + "\n" + pic);
        }

        public string CreateConsolePic(int[] data)
        {
            const char symbol = '-';
            const char symbolDivider = '|';
            int width = data[0];
            int sum = data.Skip(1).Sum();
            int segmentsCount = data.Length - 1;
            int dividersCount = segmentsCount - 1;
            int freeSpaces = width - dividersCount;

            if (segmentsCount > freeSpaces || data.Length < 2 || sum == 0) return "Error!";

            (double fracPart, int dashCount, int order)[] d = new (double, int, int)[data.Length - 1];
            int baseSum = 0;

            for (int i = 1; i < data.Length; i++)
            {
                double idealWidth = (double)data[i] * freeSpaces / sum;
                int floorBase = (int)Math.Floor(idealWidth);
                d[i - 1] = (idealWidth % 1, floorBase, i);
                baseSum += floorBase;
            }

            int remainder = freeSpaces - baseSum;

            //сортируем по убыванию дробной части, чтобы сегментам, которые почти дотянули до следующего целого числа выделить тире
            Array.Sort(d, (a, b) => b.fracPart.CompareTo(a.fracPart));

            for (int i = 0; i < remainder; i++) d[i].dashCount++;

            //отсортировать можно только один раз, если использовать изначально 2 массива только для тире и остатка и создать массив индексов (0..n-1) и отсортировать только его по остаткам 'Array.Sort(indices, (a, b) => fractional[b].CompareTo(fractional[a]));' и в раздаче остатка использовать [indices[i]]
            Array.Sort(d, (a, b) => a.order.CompareTo(b.order));

            StringBuilder pic = new StringBuilder();

            for (int i = 0; i < d.Length; i++)
            {
                if (d[i].dashCount == 0) return "Error!";
                pic.Append(new string(symbol, d[i].dashCount));
                if (i < d.Length - 1) pic.Append(symbolDivider);
            }

            return pic.ToString();
        }
    }
}
