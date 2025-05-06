using System.Text;
using static System.Console;
using static CritBit.BitHelper;

namespace CritBit;
public static class MiddlePrefixComputer
{
    private static int _prefixLength;

    public static string ComputeMiddlePrefix(string bitString1, 
        string bitString2, Trie trie, 
        bool roundUp,
        out string logs)
    {
        var logBuilder = new StringBuilder();
        logBuilder.AppendLine("\n=== вычисление среднего префикса ===");

        var (high, low) = OrderStrings(bitString1, bitString2);
        LogOrder(high, low, logBuilder);

        var commonPrefix = GetCommonPrefix(high, low, out int prefixLength);
        LogCommonPrefix(commonPrefix, logBuilder);

        var (highPostfix, lowPostfix) = GetPostfixes(high, low, prefixLength);
        LogPostfixes(highPostfix, lowPostfix, logBuilder);

        string result = HandlePostfixCases(
            commonPrefix,
            highPostfix,
            lowPostfix,
            trie,
            roundUp,
            logBuilder
        );

        logs = logBuilder.ToString();
        return result;
    }


    private static (string high, string low) OrderStrings(string a, string b)
    {
        return string.Compare(a, b) > 0 ? (a, b) : (b, a);
    }

    private static void LogOrder(string high, string low, StringBuilder logBuilder)
    {
        logBuilder.AppendLine($"Старшая строка: {FormatBitString(high)}");
        logBuilder.AppendLine($"Младшая строка:  {FormatBitString(low)}");
    }

    private static string GetCommonPrefix(string a, string b, out int prefixLength)
    {
        prefixLength = FindCommonPrefixLength(a, b);
        _prefixLength=prefixLength;
        return a.Substring(0, prefixLength);
    }

    private static void LogCommonPrefix(string commonPrefix, StringBuilder logBuilder)
    {
        logBuilder.AppendLine($"Общий префикс: {FormatBitString(commonPrefix)}");
    }

    private static (string, string) GetPostfixes(string high, string low, int prefixLength)
    {
        return (high.Substring(prefixLength), low.Substring(prefixLength));
    }

    private static void LogPostfixes(string highPostfix, string lowPostfix, StringBuilder logBuilder)
    {
        logBuilder.AppendLine($"Постфикс старшей: {FormatBitString(highPostfix)}");
        logBuilder.AppendLine($"Постфикс младшей: {FormatBitString(lowPostfix)}");
    }

    private static string HandlePostfixCases(
        string commonPrefix,
        string highPostfix,
        string lowPostfix,
        Trie trie,
        bool roundUp,
        StringBuilder logBuilder)
    {
        return (highPostfix, lowPostfix) switch
        {
            ("", "") => HandleBothEmpty(commonPrefix, logBuilder),
            (_, "") => HandleLowEmpty(commonPrefix, highPostfix, trie, roundUp, logBuilder),
            ("", _) => HandleHighEmpty(commonPrefix, lowPostfix, trie, roundUp, logBuilder),
            _ => HandleBothNonEmpty(commonPrefix, highPostfix, lowPostfix, roundUp, logBuilder)
        };
    }

    private static string HandleBothEmpty(string commonPrefix, StringBuilder logBuilder)
    {
        logBuilder.AppendLine("Оба постфикса пусты, возвращаем общий префикс");
        return commonPrefix;
    }

    private static string HandleLowEmpty(
        string commonPrefix,
        string highPostfix,
        Trie trie,
        bool roundUp,
        StringBuilder logBuilder)
    {
        logBuilder.AppendLine("\nПостфикс младшей строки пуст:");
        var upperNeighbor = trie.UpperApril(commonPrefix, _prefixLength);
        logBuilder.AppendLine($"Наименьший больший для префикса: {FormatBitString(upperNeighbor)}");

        var neighborSuffix = GetNeighborSuffix(commonPrefix, upperNeighbor);
        logBuilder.AppendLine($"Первый байт после общего префикса у наименьшей большей: {FormatBitString(neighborSuffix)}");

        return CalculateAndCombine(
            commonPrefix,
            neighborSuffix,
            GetFirstByte(highPostfix),
            roundUp,
            "между постфиксом старшей и наименьшей большей",
            logBuilder
        );
    }

    private static string HandleHighEmpty(
        string commonPrefix,
        string lowPostfix,
        Trie trie,
        bool roundUp,
        StringBuilder logBuilder)
    {
        logBuilder.AppendLine("\nПостфикс старшей строки пуст:");
        var lowerNeighbor = trie.LowerApril(commonPrefix, _prefixLength);
        logBuilder.AppendLine($"Наибольший меньший префикс: {FormatBitString(lowerNeighbor)}");

        var neighborSuffix = GetNeighborSuffix(commonPrefix, lowerNeighbor);
        logBuilder.AppendLine($"Первый байт после общего префикса у наибольшей меньшей: {FormatBitString(neighborSuffix)}");

        return CalculateAndCombine(
            commonPrefix,
            neighborSuffix,
            GetFirstByte(lowPostfix),
            roundUp,
            "между постфиксом младшей и наибольшей меньшей",
            logBuilder
        );
    }

    private static string HandleBothNonEmpty(
        string commonPrefix,
        string highPostfix,
        string lowPostfix,
        bool roundUp,
        StringBuilder logBuilder)
    {
        logBuilder.AppendLine("Оба постфикса не пустые");
        return CalculateAndCombine(
            commonPrefix,
            GetFirstByte(highPostfix),
            GetFirstByte(lowPostfix),
            roundUp,
            "между обоими постфиксами",
            logBuilder
        );
    }

    private static string GetNeighborSuffix(string commonPrefix, string neighbor)
    {
        int prefixLength = FindCommonPrefixLength(neighbor, commonPrefix);
        return neighbor.Substring(prefixLength, 8);
    }

    private static string CalculateAndCombine(
        string commonPrefix,
        string first,
        string second,
        bool roundUp,
        string context,
        StringBuilder logBuilder)
    {
        int firstValue = BitHelper.BinaryToInt(first);
        int secondValue = BitHelper.BinaryToInt(second);

        var (average, roundingDesc) = CalculateAverage(firstValue, secondValue, roundUp);
        string averageBinary = CutRightZeros(BitHelper.IntToBinary(average));

        logBuilder.AppendLine($"Вычисление среднего {roundingDesc} {context}:");
        logBuilder.AppendLine($"- Первое значение:  {FormatBitString(first)} ({firstValue})");
        logBuilder.AppendLine($"- Второе значение: {FormatBitString(second)} ({secondValue})");
        logBuilder.AppendLine($"- Среднее значение: {FormatBitString(averageBinary)} ({average})");

        return commonPrefix + averageBinary;
    }

    private static (int value, string desc) CalculateAverage(int a, int b, bool roundUp)
    {
        double average = (a + b) / 2.0;
        int result = roundUp ? (int)Math.Ceiling(average) : (int)Math.Floor(average);
        string desc = roundUp ? "(с округлением вверх)" : "(с округлением вниз)";
        return (result, desc);
    }

    private static string GetFirstByte(string postfix)
    {
        return postfix.Length >= 8 ? postfix.Substring(0, 8) : postfix.PadRight(8, '0');
    }
}
