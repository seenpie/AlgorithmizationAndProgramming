using System;

using Tasks.Common;

namespace Tasks.GraphFunctionConsonat
{
    public class GraphFunctionConsonat : IGraphFunctionConsonatSolution
    {
        private static double period = 2.0;
        public void Run()
        {
            string input = Console.ReadLine();
            double x = double.Parse(input);
            double f = F(x);
            Console.WriteLine("{0:F4}", f);
        }

        public double F(double x)
        {
            double normalizedX = x % period;
            if (normalizedX < 0)
                normalizedX += period;

            if (normalizedX < 1)
            {
                return normalizedX - 1;
            }
            else
            {
                return 1 - normalizedX;
            }
        }
    }
}
