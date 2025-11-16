using System;

using Tasks.Common;

namespace Tasks.ClockFace
{
    public class ClockFace : IClockFaceSolution
    {
        const double degreePerHour = 360 / 12;
        const double degreePerMinute = degreePerHour / 60;
        const double degreePerSecond = degreePerMinute / 60;
        public void Run()
        {
            string hoursInput = Console.ReadLine();
            string minutesInput = Console.ReadLine();
            string secondsInput = Console.ReadLine();

            try
            {
                int hours = int.Parse(hoursInput);
                int minutes = int.Parse(minutesInput);
                int seconds = int.Parse(secondsInput);

                double angle = Calculate(hours, minutes, seconds);
                Console.WriteLine(angle);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите корректные целые числа.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неизвестная ошибка: {ex.Message}");
            }
        }

        public double Calculate(int hours, int minuites, int seconds)
        {
            if (hours < 0 || hours >= 12)
            {
                throw new ArgumentOutOfRangeException(nameof(hours), "Часы должны быть между 0 и 11.");
            }
            if (minuites < 0 || minuites >= 60)
            {
                throw new ArgumentOutOfRangeException(nameof(minuites), "Минуты должны быть между 0 и 59.");
            }
            if (seconds < 0 || seconds >= 60)
            {
                throw new ArgumentOutOfRangeException(nameof(seconds), "Секунды должны быть между 0 и 59.");
            }
            double angle = hours * degreePerHour + minuites * degreePerMinute + seconds * degreePerSecond;
            return Math.Round(angle, 3);
        }
    }
}
