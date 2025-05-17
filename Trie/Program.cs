global using static CritBit.BitHelper;
global using static System.Console;
global using static CritBit.Consts;
using System.Text;
namespace CritBit;

class Program
{
    static void Main(string[] args)
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Trie trie = new Trie();

        var _testData = GenerateTestData1();

        foreach (var item in _testData)
        {
            trie.Insert(BitHelper.StringToBitString(item));
        }

        while (true)
        {
            WriteLine("\nМеню:");
            WriteLine("1. Добавить слово");
            WriteLine("2. Показать дерево");
            WriteLine("3. RightBranch");
            WriteLine("4. Lower(наибольшая меньшая)");
            WriteLine("5. Upper (наименьшая большая)");
            WriteLine("6. Подменю поиска");
            WriteLine("7. Выход");
            WriteLine("8. FindLastNodeInPath");
            Write("Выберите пункт: ");

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    Write("Введите ключ для добавления: ");
                    InsertCommand(trie);
                    break;
                case "2":
                    WriteLine(trie.GetASCII());
                    break;
                case "3":
                    Write("Введите ключ для RightBranch: ");
                    RightBranchCommand(trie);
                    break;
                case "4":
                    Write("Введите ключ для нижней границы: ");
                    LowerCommand(trie);
                    break;
                case "5":
                    Write("Введите ключ для верхней границы: ");
                    UpperCommand(trie);
                    break;
                case "6":
                    var trieSubMenu = new 
                     TrieSubMenuPostfix(trie);
                    trieSubMenu.Run();
                    break;
                case "7":
                    return;
                case "8":
                    FindLastNodeInPathCommand(trie);
                         break;//HandlePrefixCheck(trie);
                default:
                    WriteLine("Неверный выбор!");
                    break;
            }
        }
    }

    static void FindLastNodeInPathCommand(Trie trie)
    {
        string word = ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        ( var node,int busyInPathCount)= trie.FindLastNodeInPath(bits,0);
        WriteLine($"Хранит: {node.BitString.Length} Занято: {busyInPathCount}");
    }

    static void InsertCommand(Trie trie)
    {
        string word = ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        trie.Insert(bits);
        WriteLine($"Добавлено: {word} ({bits})");
    }

    static void RightBranchCommand(Trie trie)
    {
        string word = ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        string upper = trie.RightBranch(0);
        WriteLine(upper == null
            ? "RightBranch не найдена"
            : $"RightBranch: {BitHelper.BitStringToString(upper)} ({upper})");
    }

    static void LowerCommand(Trie trie)
    {
        string word = ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        string lower = trie.Lower(bits,0);
        WriteLine(lower == null
            ? "Нижняя граница не найдена"
            : $"Нижняя граница: {BitHelper.BitStringToString(lower)} ({lower})");
    }

    static void UpperCommand(Trie trie)
    {
        string word = ReadLine().Trim();
        string bits = BitHelper.StringToBitString(word);
        string lower = trie.Upper(bits,0);
        WriteLine(lower == null
            ? "Верхняя граница не найдена"
            : $"Верхняя граница: {BitHelper.BitStringToString(lower)} ({lower})");
    }

    /// <summary> Проверка наличия префикса </summary>
     static void HandlePrefixCheck(Trie trie)
    {
        var prefix = ReadLine().Trim();
        var exists = trie.CheckSubstringExists(prefix,0);
        WriteLine(exists ? "Префикс существует" : "Префикс НЕ существует");
    }

    /// <summary>
    /// Получение валидного префикса от пользователя
    /// </summary>
    private static string GetValidPrefix()
    {
        while (true)
        {
            Write("Введите префикс: ");
            var input = ReadLine()?.Trim();
            var bits = BitHelper.StringToBitString(input ?? "");

            if (bits.Length % 8 == 0)
                return bits;

            WriteLine("Ошибка: длина должна быть кратна 8 битам");
        }
    }

    private static List<string> GenerateTestData1()
    {
        return new List<string>
        {
            "ABA","ABBBBBBBBBB","ABD","ABE"
        };
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
            "BCA", "BCB", "CAB", "CBA", "DAB", "EAB", "CAC",

            // 4-chars
            "ABCD", "ABDA", "ACAA", "BACA", "BBAC",
            "BCAA", "CABB", "CBAA", "DABA", "EABC",

            // 5-chars
            "ABCDE", "ABDAC", "BACDE", "BCAAB", "CABDE",
            "CBAAA", "DABEA", "EABCD", "EBACD",

            //,

            "BBBBBBBBBBBBB",
            
            "BBCCCC"
            ,"BBBBBBBBBBBBA",
            "BBBBBBB"
            //"CAB","CAC","CABB","CABDE"
        };
    }

}

