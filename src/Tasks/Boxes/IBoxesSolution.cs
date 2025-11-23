using Tasks.Common;

namespace Tasks.Boxes
{
    public interface IBoxesSolution : ISolution
    {
       public bool IsBoxFit(double a1, double b1, double c1, double a2, double b2, double c2);
    }
}
