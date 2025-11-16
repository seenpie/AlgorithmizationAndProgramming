using Tasks.Common;

namespace Tasks.ClockFace
{
    public interface IClockFaceSolution : ISolution
    {
        double Calculate(int hours, int minuites, int seconds);
    }
}
