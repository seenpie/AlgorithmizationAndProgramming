using System;
using System.Collections.Generic;
using System.IO;
using Tasks.FilesList;
using Xunit;

namespace Tasks.Tests
{
    public class FilesListTests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.FilesList.FilesList() };
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void PrintFilesTree_ComplexStructure_PrintsCorrectHierarchy(IFilesListSolution solution)
        {
            // 1. Arrange (Подготовка)
            // Создаем временную уникальную папку для теста
            string rootPath = Path.Combine(Path.GetTempPath(), "TestFilesList_" + Guid.NewGuid());
            Directory.CreateDirectory(rootPath);

            try
            {
                // Создаем структуру файлов:
                // Root/
                //   └ rootFile.txt
                //   + SubFolder/
                //       └ subFile.txt

                // Файл в корне
                string rootFile = Path.Combine(rootPath, "rootFile.txt");
                File.WriteAllText(rootFile, "content");

                // Подпапка
                string subPath = Path.Combine(rootPath, "SubFolder");
                Directory.CreateDirectory(subPath);

                // Файл в подпапке
                string subFile = Path.Combine(subPath, "subFile.txt");
                File.WriteAllText(subFile, "content");
                
                // Сохраняем стандартный вывод консоли, чтобы потом его вернуть
                var originalOut = Console.Out;
                
                using (var sw = new StringWriter())
                {
                    // Перенаправляем вывод в нашу переменную
                    Console.SetOut(sw);

                    // 2. Act (Действие)
                    // Вызываем метод (rootPath, по умолчанию level=0)
                    solution.PrintFilesTree(rootPath, 0);

                    // 3. Assert (Проверка)
                    var output = sw.ToString();

                    // Возвращаем консоль на место (хороший тон, даже если тест упадет)
                    Console.SetOut(originalOut);

                    // Расщепляем вывод на строки для удобной проверки
                    // Используем Split, так как Enviroment.NewLine может отличаться
                    var lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                    // Проверяем логику отступов.
                    // Согласно твоему коду:
                    // Level 0 (Files): отступ = "   " (3 пробела)
                    // Level 1 (Folder): отступ = "   " (3 пробела)
                    // Level 1 (Files): отступ = "      " (6 пробелов)

                    // 1. Корень не должен печататься методом PrintFilesTree (если level == 0)
                    // Значит, "+ TestFilesList_..." быть не должно.

                    // 2. Файл в корне должен быть с отступом в 3 пробела и значком └
                    Assert.Contains("   └ rootFile.txt", output);

                    // 3. Подпапка должна быть с отступом в 3 пробела и значком +
                    Assert.Contains("   + SubFolder", output);

                    // 4. Файл внутри подпапки должен быть глубже (6 пробелов)
                    Assert.Contains("      └ subFile.txt", output);
                }
            }
            finally
            {
                // 4. Cleanup (Очистка)
                // Удаляем временную папку, даже если текст упал с ошибкой
                if (Directory.Exists(rootPath))
                {
                    Directory.Delete(rootPath, true);
                }
            }
        }

        [Theory]
        [MemberData(nameof(GetSolutions))]
        public void PrintFilesTree_EmptyFolder_PrintsNothing(IFilesListSolution solution)
        {
            // Тест на пустую папку
            string rootPath = Path.Combine(Path.GetTempPath(), "TestEmpty_" + Guid.NewGuid());
            Directory.CreateDirectory(rootPath);

            var originalOut = Console.Out;
            try
            {
                using (var sw = new StringWriter())
                {
                    Console.SetOut(sw);
                    
                    solution.PrintFilesTree(rootPath, 0);
                    
                    var output = sw.ToString();
                    Console.SetOut(originalOut);

                    // Вывод должен быть пустым (или содержать только переводы строк), 
                    // так как файлов нет, а имя корневой папки при level=0 не выводится.
                    Assert.True(string.IsNullOrWhiteSpace(output));
                }
            }
            finally
            {
                if (Directory.Exists(rootPath)) Directory.Delete(rootPath, true);
            }
        }
    }
}

