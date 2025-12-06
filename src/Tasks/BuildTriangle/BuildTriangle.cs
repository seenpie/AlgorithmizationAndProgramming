using System;

using Tasks.Common;

namespace Tasks.BuildTriangle
{
    public class BuildTriangle : IBuildTriangleSolution
    {
        public void Run()
        {
            if (!int.TryParse(Console.ReadLine(), out int a) || !int.TryParse(Console.ReadLine(), out int b) || !int.TryParse(Console.ReadLine(), out int c))
            {
                Console.WriteLine("incorrect input");
                return;
            }

            string resultStr = Calculate(a, b, c);
            Console.WriteLine(resultStr);
        }

        public string Calculate(int a, int b, int c)
        {
            int min = Math.Min(Math.Min(a, b), c);
            if (min <= 0) return "NO";
            int max = Math.Max(Math.Max(a, b), c);
            int mid = a + b + c - min - max;
            return min + mid > max ? "YES" : "NO";
        }
    }
}
