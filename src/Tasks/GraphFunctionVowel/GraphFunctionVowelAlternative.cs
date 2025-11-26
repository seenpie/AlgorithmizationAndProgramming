using System;

using Tasks.Common;

namespace Tasks.GraphFunctionVowel
{
    public class GraphFunctionVowelAlternative : IGraphFunctionVowelSolution
    {
        public void Run()
        {
            double x = double.Parse(Console.ReadLine());
            double y = F(x);

            Console.WriteLine(y);
        }

        public double F(double x)
        {
            double absX = Math.Abs(x);
            double remainder = absX % 1;

            if (absX % 2 == 0) return 0;

            if (Math.Floor(absX) % 2 != 0) return 1 - remainder;

            return remainder;
        }
    }
}
