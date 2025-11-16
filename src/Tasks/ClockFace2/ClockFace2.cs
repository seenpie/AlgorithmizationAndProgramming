using System;

using Tasks.Common;

namespace Tasks.ClockFace2
{
    public class ClockFace2 : IClockFace2Solution
    {
        const double degreePerHour = 360.0 / 12.0;

        const double minutesToHourSpeedRation = 12.0;
        public void Run()
        {
            string angleHoursInput = Console.ReadLine();

            try
            {
                double angleHours = double.Parse(angleHoursInput);

                double angle = Calculate(angleHours);
                Console.WriteLine(angle);
            }
            catch
            {
                return;
            }
        }

        public double Calculate(double angleHours)
        {
            if (angleHours < 0 || angleHours >= 360)
            {
                throw new ArgumentOutOfRangeException(nameof(angleHours), "Угол должен быть между 0 и 359.");
            }

            double minutesDegrees = (angleHours % degreePerHour) * minutesToHourSpeedRation;

            return Math.Round(minutesDegrees, 2);
        }
    }
}
