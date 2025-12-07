using System;

using Tasks.Common;

namespace Tasks.TrigonometricFunctions
{
    public class TrigonometricFunctionsCos : ITrigonometricFunctionsSolution
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



        private double ConvertDegreesToRadians(double angle)
        {
            return angle * Math.PI / 180.0;
        }

        public double Calculate(double angle, double e)
        {   
            double x = ConvertDegreesToRadians(angle);
            x %= 2 * Math.PI;
            if (x < 0) x += 2 * Math.PI;
            int n = 0;
            double term = 1;
            double sum = term;
            while (Math.Abs(term) > e || n < 2)
            {
                n++;
                term *= -x * x / ((2 * n - 1) * (2 * n));
                sum += term;
            }
            return sum;
        }
    }
}
