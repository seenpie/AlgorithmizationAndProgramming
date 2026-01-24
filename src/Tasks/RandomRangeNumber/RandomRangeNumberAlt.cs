using Tasks.RandomRangeNumber;

namespace Tasks.RandomRangeNumber;

public class RandomRangeNumberAlt : IRandomRangeNumberSolution
{
    private const uint DefaultMinValue = 1;
    private const uint DefaultMaxValue = 100;

    public void Run()
    {
        uint d = ReadUintInputWithDefault("Введите значение d > 0", 1, true);
        uint min = ReadUintInputWithDefault("Введите значение min > 0", DefaultMinValue);
        uint max = ReadUintInputWithDefault("Введите значение max > 0", DefaultMaxValue);
        uint num = GetNumber(d, min, max);
        Console.WriteLine(num);
    }

    private uint ReadUintInputWithDefault(string prompt, uint defaultValue, bool isRequired = false)
    {
        Console.WriteLine($"{prompt} {(isRequired ? "(обязательно)" : $"(по умолчанию: {defaultValue})")}");

        string input = Console.ReadLine();

        if (uint.TryParse(input, out uint parsedInput) && parsedInput > 0)
        {
            return parsedInput;
        }

        if (isRequired)
        {
            Console.WriteLine("Ошибка ввода, попробуйте еще раз");
            return ReadUintInputWithDefault(prompt, defaultValue, isRequired);
        }

        Console.WriteLine($"Ошибка ввода, будет использовано значение по умолчанию {defaultValue}");
        return defaultValue;
    }

    public uint GetNumber(uint d, uint min, uint max)
    {
        return TryGetNumber(d, out uint result, min, max) ? result : throw new InvalidOperationException($"В диапазоне [{min}, {max}] нет чисел, делящихся на {d}.");
    }

    private bool TryGetNumber(uint d, out uint result, uint min = DefaultMinValue, uint max = DefaultMaxValue)
    {
        result = 0;
        if (d > max || d < 1 || min < 1 || max < 1) return false;

        // получаем значение(округленное вниз), которое будет максимальным
        // множителем для d (сколько раз d уберется в диапазон [d...max])
        uint maxForOperation = max / d;
        // получаем значение(округленное вверх), которое будет минимальным
        // множителем для d * minFor >= min
        // v1:
        // uint minForOperation = d >= min ? 1 : (uint)Math.Ceiling((double)min / d);
        // вариант с целочисленным делением [формула для целочисленных* ceil(a/b)=(a+b-1)/b]
        uint minForOperation = (uint)((min + (ulong)d - 1) / d);
        if (maxForOperation < minForOperation) return false;

        // получился диапазон из множителей
        // [minFor...maxFor], d можно умножить на каждое из диапазона
        // и число войдет в основной диапазон [min...max]
        uint random = GetRandomFromRange(minForOperation, maxForOperation);
        result = d * random;
        return true;
    }

    private uint GetRandomFromRange(uint from, uint to)
    {
        long toInclusive = (long)to + 1;
        return (uint)Random.Shared.NextInt64(from, toInclusive);
    }
}
