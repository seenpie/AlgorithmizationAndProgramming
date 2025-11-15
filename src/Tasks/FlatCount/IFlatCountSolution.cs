using Tasks.Common;

namespace Tasks.FlatCount
{
    public interface IFlatCountSolution : ISolution
    {
        (int entrance, int floor) Calculate(int flatNumber, int floorCount, int flatsPerFloor);
    }
}