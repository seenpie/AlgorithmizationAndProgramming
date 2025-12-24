using Tasks.Common;

namespace Tasks.AimShooting
{
    public interface IAimShootingSolution : ISolution
    {
        int CalculateScore(double x, double y, short maxValue, short step, short maxScore);
    }
}
