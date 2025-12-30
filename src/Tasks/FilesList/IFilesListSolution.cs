using Tasks.Common;

namespace Tasks.FilesList
{
    public interface IFilesListSolution : ISolution
    {
        public void PrintFilesTree(string path, int level);
    }
}
