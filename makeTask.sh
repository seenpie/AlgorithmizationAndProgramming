#!/bin/bash

# Проверка, находимся ли мы в корневой директории (где находится Tasks.sln)
if [ ! -f "Tasks.sln" ]; then
    echo "Ошибка: Скрипт должен запускаться из корневой директории, содержащей Tasks.sln"
    exit 1
fi

# Парсинг аргументов
NAME=""
while [[ $# -gt 0 ]]; do
    case $1 in
        --name)
            NAME="$2"
            shift 2
            ;;
        *)
            echo "Использование: $0 --name НазваниеЗадачи"
            exit 1
            ;;
    esac
done

if [ -z "$NAME" ]; then
    echo "Ошибка: Аргумент --name обязателен"
    exit 1
fi

# Валидация имени (PascalCase, без пробелов и т.д.)
if [[ ! "$NAME" =~ ^[A-Z][a-zA-Z0-9]*$ ]]; then
    echo "Ошибка: Название должно быть в PascalCase (например, StringReversal)"
    exit 1
fi

echo "Создание задачи: $NAME"

# Создание директорий
mkdir -p "src/Tasks/$NAME"
mkdir -p "tests/Tasks.Tests"

# Создание нового класса задачи
dotnet new classlib -n Tasks.$NAME -o src/Tasks/$NAME

# Добавление ссылки на Tasks.Common
dotnet add src/Tasks/$NAME/Tasks.$NAME.csproj reference src/Tasks.Common/Tasks.Common.csproj

# Добавление Runner ссылки на новый проект
dotnet add src/Runner/Runner.csproj reference src/Tasks/$NAME/Tasks.$NAME.csproj

# Добавление в решение
dotnet sln add src/Tasks/$NAME/Tasks.$NAME.csproj

# Добавление ссылки на проект в тестовый csproj
dotnet add tests/Tasks.Tests/Tasks.Tests.csproj reference src/Tasks/$NAME/Tasks.$NAME.csproj

# Удаление лишнего файла Class1.cs
rm -f src/Tasks/$NAME/Class1.cs

# Создание файла интерфейса
cat > "src/Tasks/$NAME/I${NAME}Solution.cs" << EOF
using Tasks.Common;

namespace Tasks.$NAME
{
    public interface I${NAME}Solution : ISolution
    {
        // TODO: Определить сигнатуру метода для этой задачи
        // Пример: object Solve(string input);
    }
}
EOF

# Создание файла класса
cat > "src/Tasks/$NAME/${NAME}.cs" << EOF
using System;

using Tasks.Common;

namespace Tasks.$NAME
{
    public class $NAME : I${NAME}Solution
    {
        public void Run()
        {
            // TODO: Реализовать метод Run
            // Пример: string input = Console.ReadLine();
            // var result = Solve(input);
            // Console.WriteLine(result);
        }

        // TODO: Реализовать метод задачи
        // public object Solve(string input)
        // {
        //     // Реализация здесь
        // }
    }
}
EOF

# Создание файла тестов
cat > "tests/Tasks.Tests/${NAME}Tests.cs" << EOF
using System;
using System.Collections.Generic;
using Tasks.$NAME;
using Xunit;

namespace Tasks.Tests
{
    public class ${NAME}Tests
    {
        public static IEnumerable<object[]> GetSolutions()
        {
            yield return new object[] { new Tasks.${NAME}.${NAME}() };
        }

        // TODO: Добавить тестовые случаи
        // [Theory]
        // [MemberData(nameof(GetSolutions))]
        // public void SomeTest_WithValidInputs_ReturnsCorrectValue(I${NAME}Solution solution)
        // {
        //     // Arrange
        //     // Act
        //     // Assert
        // }
    }
}
EOF

echo "Задача $NAME создана успешно!"
echo "Следующие шаги:"
echo "1. Определить метод в I${NAME}Solution.cs"
echo "2. Реализовать метод в ${NAME}.cs"
echo "3. Добавить тестовые случаи в ${NAME}Tests.cs"
echo "4. Запустить 'dotnet build' для проверки"