using System;

using Tasks.Common;

namespace Tasks.MathCalculations;

public class MathCalculationsAlternative : IMathCalculationsSolution
{
    public void Run()
    {
        double parsedValue = double.Parse(Console.ReadLine());
        double x = Calculate(parsedValue);
        // Console.WriteLine(x == 0 ? "0" : $"{x:f3}");
        Console.WriteLine(x);
    }

    public double Calculate(double a)
    {
        // return Math.Sqrt((2 * a + Math.Sin(Math.Abs(3 * a))) / 3.56);
        double r = Math.Sqrt((2 * a + Math.Sin(Math.Abs(3 * a))) / 3.56);
        return r == 0 ? r : Math.Round(r, 3);
    }
}
