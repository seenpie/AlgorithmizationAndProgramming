using System;

using Tasks.Common;

namespace Tasks.WhileInput
{
    public class WhileInput : IWhileInputSolution
    {
        public void Run()
        {
            int P = 1;
            int S = 0;

            while (true)
            {
                int input = int.Parse(Console.ReadLine());
                if (input == 0) break;
                P *= input;
                S += input;
            }

            Console.WriteLine(S == 0 ? "данные не получены" : P / S);
        }
    }
}
