using Tasks.Common;

namespace Tasks.Palindrom
{
    public interface IPalindromSolution : ISolution
    {
        bool IsPalindrom(string str);
    }
}
