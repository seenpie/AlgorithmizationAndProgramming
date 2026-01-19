using System.Globalization;

namespace Tasks.MonthCalendar;

public class MonthCalendarAlt : IMonthCalendarSolution
{
    private const string DateFormat = "dd.MM.yyyy";
    private const string HeaderLine = "Календарь на месяц:\n  M  T  W  T  F  S  S\n";

    public void Run()
    {
        string input = ReadInput($"Введите дату ({DateFormat.ToUpper()}):");
        string calendarVisualization = GetCalendarVisualization(input);
        Console.WriteLine(calendarVisualization);
    }

    private string ReadInput(string label)
    {
        Console.WriteLine(label);
        string input = Console.ReadLine();
        if (!string.IsNullOrWhiteSpace(input)) return input;
        Console.WriteLine("Некорректный ввод, попробуйте еще раз");
        return ReadInput(label);
    }

    public string GetCalendarVisualization(string date)
    {
        if (!DateTime.TryParseExact(date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
        {
            return "Некорректная дата";
        }

        return $"\nДата: {dateTime:D}\n\n{GetCalendarPart(dateTime)}";
    }

    private string GetCalendarPart(DateTime dateTime)
    {
        DateTime dt = dateTime.Day == 1 ? dateTime : new DateTime(dateTime.Year, dateTime.Month, 1);
        int dayOfWeek = (int)dt.DayOfWeek;
        int daysInMonth = DateTime.DaysInMonth(dt.Year, dt.Month);
        int shift = dayOfWeek == 0 ? 6 : dayOfWeek - 1;
        string[] result = new string[daysInMonth + shift];

        for (int i = 0, day = 1; i < result.Length; i++)
        {
            if (i < shift)
            {
                result[i] = "   ";
                continue;
            }
            string str = $"{day++,3}";
            if ((i + 1) % 7 == 0) str += "\n";
            result[i] = str;
        }

        return HeaderLine + string.Concat(result);
    }
}
