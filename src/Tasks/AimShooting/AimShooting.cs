using System;

using Tasks.Common;

namespace Tasks.AimShooting
{
    public class AimShooting : IAimShootingSolution
    {
        private struct Shot
        {
            public double x;
            public double y;
        }
        
        public void Run()
        {
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExitHandler);

            // Максимальное значение в пределах которого вычисляется координаты выстрела
            short maxValue = 15;
            // Задержка при вычислении и отображении изменяющихся координат
            short sleep = 30;
            // Шаг с которым идут круги на мишени
            short step = 1;
            // Количество секций на мишени (остальное "молоко")
            short maxScore = 10;
            // Общий счет
            short totalScore = 0;
            // Текущий выстрел
            short currentShot = 1;

            if (AskUser("Изменить настройки по умолчанию? Y - да"))
                ConfigureGame(ref maxValue, ref sleep, ref step, ref maxScore);

            bool isGameContinue = true;

            while (isGameContinue)
            {
                Shot shot = DoShot(currentShot, maxValue, sleep);   
                short score = CalculateScore(shot, maxValue, step, maxScore);

                totalScore += score;
                currentShot += 1;

                PrintRoundResult(score, totalScore, shot, (short)(step * maxScore));

                if (AskUser("Выйти? Y - да"))
                    isGameContinue = false;
            }

            Console.WriteLine($"Score: {totalScore}");
        }

        private static bool AskUser(string message)
        {
            Console.WriteLine(message);
            while (!Console.KeyAvailable) { }
            return char.ToLower(Console.ReadKey(true).KeyChar) == 'y';
        }

        private static void PrintRoundResult(short score, short totalScore, Shot shot, short maxValue)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            PrintSeparator();
            Console.WriteLine($"Выстрел: {score}. Общий счет: {totalScore}");
            
            Console.ForegroundColor = ConsoleColor.Gray;
            DrawHitBar("X", shot.x, maxValue);
            DrawHitBar("Y", shot.y, maxValue);
            Console.ForegroundColor = ConsoleColor.DarkYellow;

            PrintSeparator();
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void DrawHitBar(string axis, double val, short max)
        {
            int width = 20;
            int center = width / 2;
            
            if (Math.Abs(val) <= max)
            {
                int pos = (int)((val + max) / (2.0 * max) * width);
                
                if (pos < 0) pos = 0;
                if (pos >= width) pos = width - 1;

                char[] bar = new string('-', width).ToCharArray();
                bar[center] = 'x'; 
                bar[pos] = '*';    

                Console.WriteLine($"{axis}: [{new string(bar)}] ({val:F2})");
            }
            else
            {
                char[] bar = new string('-', width).ToCharArray();
                bar[center] = 'x';
                
                if (val > max)
                {
                     Console.WriteLine($"{axis}: [{new string(bar)}]    * ({val:F2})");
                }
                else
                {
                     Console.WriteLine($"{axis}: *    [{new string(bar)}] ({val:F2})");
                }
            }
        }

        // Метод только для тестов
        public int CalculateScore(double x, double y, short maxValue, short step, short maxScore)
        {
            return CalculateScore(new Shot { x = x, y = y }, maxValue, step, maxScore);
        }

        private static void ConfigureGame(ref short maxValue, ref short sleep, ref short step, ref short maxScore)
        {
            if (TryReadValue($"Введите ширину всей мишени. {maxValue} по умолчанию", out short newMaxValue) &&
                TryReadValue($"Введите ширину одной секции. {step} по умолчанию", out short newStep) &&
                Validate(newStep <= newMaxValue) &&
                TryReadValue($"Введите количество секций. {maxScore} по умолчанию", out short newMaxScore) &&
                Validate(newMaxScore * newStep <= newMaxValue) &&
                TryReadValue($"Введите задержку. {sleep} по умолчанию", out short newSleep))
            {
                maxValue = newMaxValue;
                step = newStep;
                maxScore = newMaxScore;
                sleep = newSleep;
            }
        }

        private static bool TryReadValue(string message, out short value)
        {
            Console.WriteLine(message);
            if (short.TryParse(Console.ReadLine(), out value))
                return true;
            PrintErrorMessage();
            value = 0;
            return false;
        }

        private static bool Validate(bool condition)
        {
            if (!condition) PrintErrorMessage();
            return condition;
        }

        private static void PrintErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Введено некорректное значение. Будет использовано значение по умолчанию");
            Console.ResetColor();
        }

        private static Shot DoShot(short number, short maxValue, short sleep)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            PrintSeparator();
            Console.WriteLine($"Выстрел: {number}");
            PrintSeparator();
            Console.ResetColor();

            double x = GetCoordinate("X", maxValue, sleep);

            WaitOrSkip(sleep * 1000);

            Console.ForegroundColor = ConsoleColor.Blue;
            PrintSeparator();
            Console.ResetColor();

            double y = GetCoordinate("Y", maxValue, sleep);

            return new Shot { x = x, y = y };
        }

        private static void WaitOrSkip(int durationMs)
        {
            int elapsed = 0;
            int step = 50;
            while (elapsed < durationMs)
            {
                if (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                    return;
                }
                System.Threading.Thread.Sleep(step);
                elapsed += step;
            }
        }

        private static double GetCoordinate(string axisName, short maxValue, short sleep)
        {
            double value = -maxValue;
            
            Console.WriteLine($"Определение {axisName}... Нажмите любую клавишу...");

            Console.CursorVisible = false;

            while (!Console.KeyAvailable)
            {
                double delta = Random.Shared.NextDouble();
                value += delta;

                // При достижении максимума перескакиваем на минимум
                if (value >= maxValue)
                {
                    value = -maxValue;
                }

                Console.Write($"{value:F2}  "); 
                Console.ResetColor();

                Console.CursorLeft = 0;
                
                System.Threading.Thread.Sleep(sleep);
            }
            Console.ReadKey(true);
            Console.CursorVisible = true;

            Console.WriteLine($"{axisName} = {value:F2}");

            return value;
        }

        private static short CalculateScore(Shot shot, short maxValue, short step, short maxScore)
        {
            double distance = Math.Sqrt(shot.x * shot.x + shot.y * shot.y);
            
            int ring = distance == 0 ? 1 : (int)Math.Ceiling(distance / step);
            
            if (ring > maxScore)  return 0;
            
            return (short)(maxScore - ring + 1);
        }

        private static void PrintSeparator()
        {
            Console.WriteLine("**********************************");
        }

        private static void OnExitHandler(object? sender, ConsoleCancelEventArgs args)
        {
            Console.CursorVisible = true;
            Console.ResetColor();
        }
    }
}
