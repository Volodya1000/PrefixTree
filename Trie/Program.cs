namespace Trie;

class Program
{
    static void Main(string[] args)
    {
        Trie trie = new Trie();

        var _testData = GenerateTestData();

        foreach (var item in _testData)
        {
            trie.Insert(BitHelper.StringToBitString(item));
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Добавить слово");
            Console.WriteLine("2. Найти слово");
            Console.WriteLine("3. Показать дерево");
            Console.WriteLine("4. Найти верхнюю границу");
            Console.WriteLine("5. Найти нижнюю границу");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите пункт: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.Write("Введите слово для добавления: ");
                    InsertCommand(trie);
                    break;
                case "2":
                    Console.Write("Введите слово для поиска: ");
                    SearchCommand(trie);
                    break;
                case "3":
                    trie.Display();
                    break;
                case "4":
                    Console.Write("Введите слово для верхней границы: ");
                    UpperCommand(trie);
                    break;
                case "5":
                    Console.Write("Введите слово для нижней границы: ");
                    LowerCommand(trie);
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void InsertCommand(Trie trie)
    {
        string word = Console.ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        trie.Insert(bits);
        Console.WriteLine($"Добавлено: {word} ({bits})");
    }

    static void SearchCommand(Trie trie)
    {
        string word = Console.ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        bool found = trie.Search(bits);
        Console.WriteLine($"Результат поиска: {found}");
    }

    static void UpperCommand(Trie trie)
    {
        string word = Console.ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        string upper = trie.Upper(bits);
        Console.WriteLine(upper == null
            ? "Верхняя граница не найдена"
            : $"Верхняя граница: {BitHelper.BitStringToString(upper)} ({upper})");
    }

    static void LowerCommand(Trie trie)
    {
        string word = Console.ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        string lower = trie.Lower(bits);
        Console.WriteLine(lower == null
            ? "Нижняя граница не найдена"
            : $"Нижняя граница: {BitHelper.BitStringToString(lower)} ({lower})");
    }

    private  static List<string> GenerateTestData()
    {
        return new List<string>
        {
            // 1-char
            "A", "B", "C", "D", "E",
                
            // 2-chars
            "AB", "AC", "BA", "BB", "BC", "CA", "CB",
            "DA", "DB", "EA", "EB",
                
            // 3-chars
            "ABC", "ABD", "ACA", "ACB", "BAD", "BBA",
            "BCA", "BCB", "CAB", "CBA", "DAB", "EAB",
                
            // 4-chars
            "ABCD", "ABDA", "ACAA", "BACA", "BBAC",
            "BCAA", "CABB", "CBAA", "DABA", "EABC",
                
            // 5-chars
            "ABCDE", "ABDAC", "BACDE", "BCAAB", "CABDE",
            "CBAAA", "DABEA", "EABCD", "EBACD"
        };
    }

}



