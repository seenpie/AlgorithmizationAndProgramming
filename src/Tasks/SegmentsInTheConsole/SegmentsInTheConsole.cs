using System;
using System.Text;
using Tasks.Common;

namespace Tasks.SegmentsInTheConsole
{
    public class SegmentsInTheConsole : ISegmentsInTheConsoleSolution
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
            int sum = Math.Abs(data.Sum() - width);
            int segmentsCount = data.Length - 1;
            int dividersCount = segmentsCount - 1;
            int freeSpaces = width - dividersCount;
            int sumSegmentsWidth = 0;

            if (segmentsCount > freeSpaces || data.Length < 2 || sum == 0) return "Error!";

            StringBuilder pic = new StringBuilder();

            for (int i = 1; i < data.Length; i++)
            {
                double segmentWidth = (double)data[i] * freeSpaces / sum;
                int roundSegmentWidth = (int)Math.Round(segmentWidth);

                if (i == data.Length - 1)
                {
                    roundSegmentWidth = freeSpaces - sumSegmentsWidth;
                }

                sumSegmentsWidth += roundSegmentWidth;

                if (roundSegmentWidth <= 0 || sumSegmentsWidth > freeSpaces) return "Error!";

                pic.Append(new string(symbol, roundSegmentWidth));

                if (i < data.Length - 1) pic.Append(symbolDivider);
            }

            return pic.Length != width ? "Error" : pic.ToString();
        }
    }
}
