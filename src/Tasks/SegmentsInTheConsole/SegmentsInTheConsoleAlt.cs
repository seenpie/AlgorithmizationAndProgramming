using System;
using Tasks.Common;

namespace Tasks.SegmentsInTheConsole
{
    public class SegmentsInTheConsoleAlt : ISegmentsInTheConsoleSolution
    {
        public void Run()
        {
            string input = Console.ReadLine();
            if (string.IsNullOrEmpty(input)) return;

            string[] parts = input.Split(',', StringSplitOptions.TrimEntries);
            
            if (parts.Length < 2 || !int.TryParse(parts[0], out int width))
                return;

            int[] segments = new int[parts.Length - 1];
            for (int i = 0; i < segments.Length; i++)
            {
                int.TryParse(parts[i + 1], out segments[i]);
            }

            Console.WriteLine(string.Join(", ", parts));
            Console.WriteLine(GetSegmentsString(width, segments));
        }

        public string CreateConsolePic(int[] data)
        {
            if (data == null || data.Length < 2) return "Error!";
            int width = data[0];
            int[] segments = data[1..];
            return GetSegmentsString(width, segments);
        }

        private string GetSegmentsString(int totalWidth, params int[] segments)
        {
            double sumValue = 0;
            for (int i = 0; i < segments.Length; i++) sumValue += segments[i];

            if (sumValue <= 0) return "Error!";

            int separatorsCount = segments.Length - 1;
            if (separatorsCount < 0) separatorsCount = 0;

            int availableForDashes = totalWidth - separatorsCount;

            if (availableForDashes < segments.Length) return "Error!";

            double ratio = (double)availableForDashes / sumValue;
            string result = "";
            int usedDashCount = 0; 

            for (int i = 0; i < segments.Length - 1; i++)
            {
                double calculatedWidth = segments[i] * ratio;
                int dashCount = (int)(calculatedWidth + 0.5);

                if (dashCount < 1) return "Error!";

                for (int k = 0; k < dashCount; k++)
                {
                    result += "-";
                }
                
                result += "|"; 
                usedDashCount += dashCount;
            }

            int lastDashCount = availableForDashes - usedDashCount;

            if (lastDashCount < 1) return "Error!";

            for (int k = 0; k < lastDashCount; k++)
            {
                result += "-";
            }

            return result;
        }
    }
}