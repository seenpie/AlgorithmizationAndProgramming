using System;
using System.IO;
using Tasks.Common;

namespace Tasks.FilesList
{
    public class FilesList : IFilesListSolution
    {
        private const string IndentStep = "   ";
        private const string FolderMarker = "+ ";
        private const string FileMarker = "└ ";
        private const ConsoleColor FolderColor = ConsoleColor.DarkGreen;

        public void Run()
        {
            Console.WriteLine("Введите имя папки: ");
            string path = Console.ReadLine();
            if (Directory.Exists(path))
            {
                Console.WriteLine();
                Console.WriteLine(path);
                PrintFilesTree(path);
            }
            else
            {
                Console.WriteLine("Папка не найдена.");
            }
        }

        public void PrintFilesTree(string path, int level = 0)
        {
            if (!Path.Exists(path)) return;

            string currentIndent = "";
            for (int i = 0; i < level; i++)
                currentIndent += IndentStep;

            if (level > 0)
            {
                Console.ForegroundColor = FolderColor;
                Console.WriteLine(currentIndent + FolderMarker + Path.GetFileName(path));
                Console.ResetColor();
            }

            try
            {
                foreach (string file in Directory.GetFiles(path))
                    Console.WriteLine(currentIndent + IndentStep + FileMarker + Path.GetFileName(file));

                foreach (string directory in Directory.GetDirectories(path))
                    PrintFilesTree(directory, level + 1);
            }
            catch (UnauthorizedAccessException) { }
        }
    }
}
