namespace Tasks.VowelOrConsonant;

public class VowelOrConsonantAlternative : IVowelOrConsonantSolution
{
    public void Run()
    {
        char input = char.Parse(Console.ReadLine());

        Console.WriteLine(DetermineLetter(input));
    }

    public string DetermineLetter(char l)
    {
        char lowerL = char.ToLower(l);

        bool isRussian = (lowerL >= 'а' && lowerL <= 'я') || lowerL == 'ё';
        if (!isRussian) return "Error";

        switch (lowerL)
        {
            // case 'ь':
            // case 'ъ':
            //     return "ни гласная, ни согласная";
            case 'а':
            case 'о':
            case 'у':
            case 'ы':
            case 'э':
            case 'я':
            case 'ю':
            case 'ё':
            case 'и':
            case 'е':
                return "Гласная";
            default:
                return "Согласная";
        }
    }
}
