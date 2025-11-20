using System;

using Tasks.Common;

//вариант 1

namespace Tasks.GraphOfTheFunction
{
    public class GraphOfTheFunctionV1 : IGraphOfTheFunctionSolution
    {
        public void Run()
        {
            double x = double.Parse(Console.ReadLine());
            double y = Calculate(x);

            Console.WriteLine(y);
        }

        public double Calculate(double x)
        {
            double absX = Math.Abs(x);
            double remainder = absX % 1;

            if (absX % 2 == 0) return 0;

            if (Math.Floor(absX) % 2 != 0) return 1 - remainder;

            return remainder;
        }
    }
}
