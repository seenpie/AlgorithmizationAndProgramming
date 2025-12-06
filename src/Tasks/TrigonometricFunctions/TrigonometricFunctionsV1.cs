using System;

using Tasks.Common;

namespace Tasks.TrigonometricFunctions
{
    public class TrigonometricFunctionsV1 : ITrigonometricFunctionsSolution
    {
        public void Run()
        {
            if (!double.TryParse(Console.ReadLine(), out double angle) || !double.TryParse(Console.ReadLine(), out double epsilon))
            {
                Console.WriteLine("incorrect input");
                return;
            }

            double calc = Calculate(angle, epsilon);

            Console.WriteLine(calc);
        }

        private long CalcFactorial(int value, long prevValue)
        {
            if (value == 0) return 1;
            return value < 3 ? value : value * (value - 1) * prevValue;
        }

        private double ConvertDegreesToRadians(double angle)
        {
            double radians = angle * Math.PI / 180;
            radians %= 2 * Math.PI;

            if (radians > Math.PI) radians -= 2 * Math.PI;
            if (radians < -Math.PI) radians += 2 * Math.PI;

            return radians;
        }

        public double Calculate(double x, double e)
        {
            double radians = ConvertDegreesToRadians(x);
            int i = 0;
            int start = 1;
            long prevFactorial = 1;
            double result = 0;
            double seriesMember;

            do
            {
                long currFactorial = CalcFactorial(start, prevFactorial);
                seriesMember = Math.Pow(-1, i++) * Math.Pow(radians, start) / currFactorial;
                result += seriesMember;
                prevFactorial = currFactorial;
                start += 2;
                if (i == 20) break;
            } while (i < 3 || Math.Abs(seriesMember) > e);

            return result;
        }
    }
}
