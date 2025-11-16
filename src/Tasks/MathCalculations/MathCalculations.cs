using System;

using Tasks.Common;

namespace Tasks.MathCalculations
{
    public class MathCalculations : IMathCalculationsSolution
    {
        public void Run()
        {
            double a = double.Parse(Console.ReadLine());
            Console.WriteLine(Calculate(a));
        }

        public double Calculate(double a)
        {
            double d = Math.Sqrt((2 * a + Math.Sin(Math.Abs(3 * a))) / 3.56);
            return Math.Round(d, 3);
        }
    }
}
