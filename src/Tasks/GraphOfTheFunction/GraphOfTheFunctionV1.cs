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

            if ((int)absX % 2 != 0) return Math.Round(1 - remainder, 1);

            return Math.Round(remainder, 1);
        }
    }
}
