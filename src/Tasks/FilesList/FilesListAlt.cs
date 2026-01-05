using System;
using System.IO;

namespace Tasks.FilesList;

public class FilesListAlt : IFilesListSolution
{
    private const char DirLabel = '+';
    private const ConsoleColor DirColor = ConsoleColor.Cyan;
    private const char FileLabel = '-';
    private const ConsoleColor FileColor = ConsoleColor.Yellow;
    private const char TreeEntryPrefixSymbol = '└';
    private const char TreeBranchFillSymbol = '─';
    private const char VerticalLineSymbol = '│';
    private const int TreeIndentWidth = 3;

    public void Run()
    {
        WriteInstructions();
        WriteCurrentDirPath();

        string dirPath = ReadInput("Введите путь до папки: ");

        Console.WriteLine();
        PrintFilesTree(dirPath, 0);
    }

    // Вывод инструкции для использования приложения
    private void WriteInstructions()
    {
        Console.WriteLine("Инструкция:");
        Console.WriteLine("- Показать наполнение текущей директории: '.'");
        Console.WriteLine("- Показать наполнение определенной поддиректории (если поддиректория существует в текущей директории): 'имя папки'");
        Console.WriteLine("- Показать наполнение определенной директории (если директория существует): 'абсолютный путь до папки'");
        Console.WriteLine("Пример: /home/user или C:/Users");
    }

    private void WriteCurrentDirPath()
    {
        Console.WriteLine($"Текущая папка: '{Directory.GetCurrentDirectory()}'");
    }

    // Читает ввод пользователя (проверка на пустой ввод)
    private string ReadInput(string label)
    {
        Console.Write(label);

        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Попробуйте еще раз");
            return ReadInput(label);
        }

        return input;
    }

    private string NormalizePath(string path)
    {
        return Path.GetFullPath(path);
    }

    // Рекурсивный метод для вывода дерева директории
    private void PrintDirectoryTree(string path, int level, int[] verticalLineLevels)
    {
        try
        {
            WriteEntry(path, level, true, verticalLineLevels);

            string[] files = Directory.GetFiles(path);
            string[] dirs= Directory.GetDirectories(path);

            foreach (string file in files)
                WriteEntry(file, level + 1, false, verticalLineLevels);

            for (int i = 0; i < dirs.Length; i++)
            {
                int[] newVerticalLineLevels = verticalLineLevels;

                if (dirs.Length > 1 && i != dirs.Length - 1)
                {
                    // добавляем текущий уровень в verticalLineLevels,
                    // если после этой папки на том же уровне есть
                    // ещё подпапки - это нужно, чтобы вложенные элементы
                    // рисовали вертикальную линию, показывая,
                    // что ветка продолжается
                    newVerticalLineLevels = new int[verticalLineLevels.Length + 1];
                    Array.Copy(verticalLineLevels, newVerticalLineLevels, verticalLineLevels.Length);
                    newVerticalLineLevels[verticalLineLevels.Length] = level;
                }

                PrintDirectoryTree(dirs[i], level + 1, newVerticalLineLevels);
            }
        }
        catch (UnauthorizedAccessException)
        {
            // нет прав на чтение - пропуск
        }
        catch (DirectoryNotFoundException)
        {
            // папка исчезла - пропуск
        }
    }

    // Выводит в консоль запись (папка/файл)
    private void WriteEntry(string entryPath, int level, bool isDir, int[] verticalLineLevels)
    {
        string indentPart = CreateIndentPart(level, verticalLineLevels);
        string entryPart = CreateEntryPart(Path.GetFileName(entryPath), isDir ? DirLabel : FileLabel);
        Console.Write(indentPart);
        WriteLineWithColor(entryPart, isDir ? DirColor : FileColor);
    }

    // Формирует визуальный префикс строки дерева
    // (отступы + ветка)
    private string CreateIndentPart(int level, int[] verticalLineLevels)
    {
        if (level < 1) return "";

        // т.к IndentPart состоит из indent + branch
        // длина branch суммарно равна TreeIndentWidth
        // поэтому нужно освободить место (level - 1)
        int indentLength = TreeIndentWidth * (level - 1);
        // массив для удобного изменения элемента по индексу
        char[] indent = new char[indentLength];

        for (int i = 0; i < indentLength; i++) indent[i] = ' ';

        foreach (int verticalLineLevel in verticalLineLevels)
        {
            // вычисление позиции, где нужна вертикальная линия
            int position = TreeIndentWidth * verticalLineLevel;
            // пропускаем уровни, которые выходят за пределы отступа
            // (например, текущий уровень для папки)
            if (position < indentLength)
                indent[position] = VerticalLineSymbol;
        }

        // TreeIndentWidth - 1 т.к Prefix имеет длину 1
        string branchPart = TreeEntryPrefixSymbol + new string(TreeBranchFillSymbol, TreeIndentWidth - 1);
        return new string(indent) + branchPart;
    }

    private string CreateEntryPart(string name, char label)
    {
        return $"{label} {name}";
    }

    private void WriteLineWithColor(string line, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(line);
        Console.ResetColor();
    }

    // Публичный метод входа (для тестов)
    public void PrintFilesTree(string path, int level)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("Путь пустой");
            return;
        }

        string normalizedPath = NormalizePath(path);
        if (!Directory.Exists(normalizedPath))
        {
            Console.WriteLine("Папки не существует");
            return;
        }

        PrintDirectoryTree(normalizedPath, level, Array.Empty<int>());
    }
}
