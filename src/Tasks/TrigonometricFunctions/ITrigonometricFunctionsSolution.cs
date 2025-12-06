using Tasks.Common;

namespace Tasks.TrigonometricFunctions
{
    public interface ITrigonometricFunctionsSolution : ISolution
    {
        double Calculate(double x, double e);
    }
}
