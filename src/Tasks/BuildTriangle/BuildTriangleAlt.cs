using System;

using Tasks.Common;

namespace Tasks.BuildTriangle
{
    public class BuildTriangleAlt : IBuildTriangleSolution
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
            if (a <= 0 || b <= 0 || c <= 0) return "NO";
            int maxSide = Math.Max(a, Math.Max(b, c));
            int sumOfOthers = a + b + c - maxSide;
            return sumOfOthers > maxSide ? "YES" : "NO";
        }
    }
}
