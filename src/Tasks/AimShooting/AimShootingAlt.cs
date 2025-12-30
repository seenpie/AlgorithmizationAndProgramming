using System;
using Tasks.Common;

namespace Tasks.AimShooting;

public class AimShootingAlt : IAimShootingSolution
{
    // Параметры игры по умолчанию
    private const short DefaultMaxValue = 15;
    private const short DefaultMaxScore = 10;
    private const short DefaultStep = 1;
    private const ushort DefaultDelay = 30;

    // Структура для игровых настроек, нельзя изменять после инициализации
    private readonly struct GameSettings(short maxValue, short maxScore, short step, ushort delay)
    {
        public short MaxValue { get; } = maxValue;
        public short MaxScore { get; } = maxScore;
        public short Step { get; } = step;
        public ushort Delay { get; } = delay;
    }

    public void Run()
    {
        GameSettings gameSettings = ConfigureGameSettings();
        PlayGame(gameSettings);
    }

    // Конфигурирует настройки игры (по умолчанию, либо пользовательские)
    private GameSettings ConfigureGameSettings()
    {
        return UserWants("Изменить установки игры?")
            ? GetUserGameSettings()
            : GetDefaultGameSettings();
    }

    // Запрашивает у пользователя целое число в указанном диапазоне
    private int PromptIntInRange(string label, int minValue, int maxValue, int defaultValue, int attempt = 1)
    {
        string text = attempt < 2
            ? $"Введите {label} [{minValue}, {maxValue}] ({defaultValue} по умолчанию)"
            : $"Введите в диапазоне [{minValue}, {maxValue}]";

        Console.WriteLine(text);
        string input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input)) return defaultValue;

        if (int.TryParse(input, out int value)
            && value <= maxValue
            && value >= minValue
            )
        {
            return value;
        }

        return PromptIntInRange(label, minValue, maxValue, defaultValue, ++attempt);
    }

    // Возвращает параметры игры, заданные пользователем
    private GameSettings GetUserGameSettings()
    {
        short maxValue = (short)PromptIntInRange("ширину всей мишени", 1, 50, DefaultMaxValue);

        short step = (short)PromptIntInRange("ширину одной секции", 1, 10, DefaultStep);

        short maxScore = (short)PromptIntInRange("количество секций", 1, 50, DefaultMaxScore);

        ushort delay = (ushort)PromptIntInRange("задержку", 10, 300, DefaultDelay);

        return new GameSettings(maxScore, maxValue, step, delay);
    }

    // Возвращает параметры игры 'по умолчанию'
    private GameSettings GetDefaultGameSettings()
    {
        return new GameSettings(DefaultMaxValue, DefaultMaxScore, DefaultStep, DefaultDelay);
    }

    // Запускает игровой цикл (выстрел, подсчет очков, проверку на выход)
    private void PlayGame(GameSettings gameSettings)
    {
        int shotCount = 1;
        int totalScore = 0;

        while (true)
        {
            GamePrint($"Выстрел: {shotCount}", ConsoleColor.Blue);

            int shotScore = TakeShot(gameSettings);
            totalScore += shotScore;

            GamePrint($"Очков за выстрел: {shotScore} Общий счет: {totalScore}", ConsoleColor.Blue);

            if (UserWants("Выйти из игры?")) return;

            shotCount++;
        }
    }

    // Проверяет согласие игрока на выполнение действия
    private bool UserWants(string text)
    {
        Console.WriteLine($"{text} (Y/y - Да)");
        string input = Console.ReadLine();
        return input?.Trim().ToLower() == "y";
    }

    // Реализует процесс выстрела и возвращает итоговый результат.
    private int TakeShot(GameSettings gameSettings)
    {
        (double x, double y) = GetShotCoords(gameSettings.MaxValue, gameSettings.Delay);
        int shotScore = CalculateScore(x, y, gameSettings.MaxValue, gameSettings.Step, gameSettings.MaxScore);
        return shotScore;
    }

    // Отрисовывает игровой вывод в консоль
    private void GamePrint(string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(new string('*', 10));
        Console.WriteLine(text);
        Console.WriteLine(new string('*', 10));
        Console.ResetColor();
    }

    // Возвращает координаты выстрела (x, y)
    private (double x, double y) GetShotCoords(short halfOfAxis, ushort timeDelay)
    {
        double coordX = GetPointCoord(halfOfAxis, timeDelay, 'X');
        double coordY = GetPointCoord(halfOfAxis, timeDelay, 'Y');
        return (coordX, coordY);
    }

    // Возвращает координаты точки (по x или по y)
    private double GetPointCoord(short halfOfAxis, ushort delay, char coordName, bool isStartFromLeft = true)
    {
        double pointCoord = isStartFromLeft ? -halfOfAxis : halfOfAxis;
        bool isAdding = isStartFromLeft;

        Console.WriteLine($"Определение {coordName}... нажмите клавишу...");

        while (true)
        {
            Console.Write($"{pointCoord:F13} ");
            Console.CursorLeft = 0;

            if (Console.KeyAvailable) break;

            Thread.Sleep(delay);

            pointCoord = CalculateNextCoord(pointCoord, halfOfAxis, ref isAdding);
        }

        Console.ReadKey(true);
        Console.WriteLine($"\n{coordName}={pointCoord:F13}\n");

        return pointCoord;
    }

    // Вычисление следующей координаты (обновление позиции прицела)
    private double CalculateNextCoord(double currCoord, short limit, ref bool isAdding)
    {
        double nextCoord = currCoord;

        if (isAdding) nextCoord += Random.Shared.NextDouble();
        else nextCoord -= Random.Shared.NextDouble();

        if (Math.Abs(nextCoord) > limit)
        {
            nextCoord = isAdding ? limit : -limit;
            isAdding = !isAdding;
        }

        return nextCoord;
    }

    // Возвращает результат выстрела
    private int CalculateShotScore(double x, double y, short step, short maxScore)
    {
        int shotSection = CalculateShotSection(x, y, step);
        if (shotSection > maxScore) return 0;
        return maxScore - shotSection;
    }

    // Для тестов
    public int CalculateScore(double x, double y, short maxValue, short step, short maxScore)
    {
        return CalculateShotScore(x, y, step, maxScore);
    }

    // Вычисление секции, в которую попал выстрел
    private int CalculateShotSection(double x, double y, short step)
    {
        double vectorLength = Math.Sqrt(x * x + y * y);
        //нумерация секций начинается с 0
        double shotSection = vectorLength / step;
        int flShotSection = (int)Math.Floor(shotSection);
        const double epsilon = 1e-9;

        //границы пренадлежат внутренней секции
        if (flShotSection != 0 && shotSection - flShotSection < epsilon) return flShotSection - 1;
        return flShotSection;
    }
}
