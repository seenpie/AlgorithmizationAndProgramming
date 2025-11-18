namespace Tasks.ClockFace2;

public class ClockFace2Alternative : IClockFace2Solution
{
    public void Run()
    {
        double a_hours = double.Parse(Console.ReadLine());
        double res = Calculate(a_hours);

        Console.WriteLine(res);
    }

    public double Calculate(double a_hours)
    {
        //1м = 0.5гр сдвиг на часовой стрелке, 1м = 6гр сдвиг по минутной стрелке => 6/0.5 = 12
        double totalMinsAngle = a_hours * 12;
        double minsCircleCount = totalMinsAngle / 360;
        double remainder = minsCircleCount % 1;
        double res = remainder * 360;
        return res - a_hours == 0 ? 0 : Math.Round(res, 2);
    }
}
