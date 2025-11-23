using System;

using Tasks.Common;

namespace Tasks.Boxes
{
    public class Boxes : IBoxesSolution
    {
        public void Run()
        {
            double a1, b1, c1;
            double a2, b2, c2;
            string a1Input = Console.ReadLine();
            string a2Input = Console.ReadLine();
            string a3Input = Console.ReadLine();
            if (!(double.TryParse(a1Input, out a1) && double.TryParse(a2Input, out b1) && double.TryParse(a3Input, out c1)))
            {
                Console.WriteLine("Incorrect size for box");
                return;
            }
            string b1Input = Console.ReadLine();
            string b2Input = Console.ReadLine();
            string b3Input = Console.ReadLine();
            if (!(double.TryParse(b1Input, out b1) && double.TryParse(b2Input, out b2) && double.TryParse(b3Input, out c2)))
            {
                Console.WriteLine("Incorrect size for mail box");
                return;
            }

            if (IsBoxFit(a1, b1, c1, b1, b2, c2))
            {
                Console.WriteLine("yes");
            }
            else
            {
                Console.WriteLine("no");
            }
        }

        public bool IsBoxFit(double a1, double b1, double c1, double a2, double b2, double c2)
        {
            double max1 = Math.Max(Math.Max(a1, b1), c1);
            double min1 = Math.Min(Math.Min(a1, b1), c1);
            double mid1 = (a1 + b1 + c1) - max1 - min1;

            double max2 = Math.Max(Math.Max(a2, b2), c2);
            double min2 = Math.Min(Math.Min(a2, b2), c2);
            double mid2 = a2 + b2 + c2 - max2 - min2;

            return min1 <= min2 && mid1 <= mid2 && max1 <= max2;
        }
    }
}
