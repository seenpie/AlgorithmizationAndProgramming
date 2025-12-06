using Tasks.Common;

namespace Tasks.BuildTriangle
{
    public interface IBuildTriangleSolution : ISolution
    {
        string Calculate(int a, int b, int c);
    }
}
