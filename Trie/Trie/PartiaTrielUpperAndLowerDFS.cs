namespace CritBit;

public partial class Trie
{

    /// <summary>
    /// Находит ближайшую строку, которая больше заданной и имеет длину, кратную 8
    /// </summary>
    public string Upper__(string key, int prefixLength)
    {
        if (!IsValidByteString(key))
            throw new ArgumentException("Key length must be multiple of 8 bits");

        string result = null;
        UpperRecursive(root, "", key, prefixLength, ref result);
        return result;
    }

    /// <summary>
    /// Рекурсивный обход дерева для поиска строки, которая больше заданной
    /// </summary>
    private void UpperRecursive(TrieNode node, string currentString, string key, int prefixLength, ref string result)
    {
        // Если длина текущей строки кратна 8 и больше ключа, обновляем результат
        if (currentString.Length % 8 == 0 && currentString.Length >= prefixLength + 8 && string.Compare(currentString, key) > 0)
        {
            if (result == null || string.Compare(currentString, result) < 0)
            {
                result = currentString;
            }
        }

        // Обходим все дочерние узлы
        foreach (var child in node.Children)
        {
            UpperRecursive(child, currentString + child.BitString, key, prefixLength, ref result);
        }
    }

    /// <summary>
    /// Находит ближайшую строку, которая меньше заданной и имеет длину, кратную 8
    /// </summary>
    public string Lower_(string bitString, int prefixLength)
    {
        if (!IsValidByteString(bitString))
            throw new ArgumentException("Key length must be multiple of 8 bits");

        string result = null;
        LowerRecursive(root, "", bitString, prefixLength, ref result);
        return result;
    }

    /// <summary>
    /// Рекурсивный обход дерева для поиска строки, которая меньше заданной
    /// </summary>
    private void LowerRecursive(TrieNode node, string currentString, string bitString, int prefixLength, ref string result)
    {
        // Если длина текущей строки кратна 8 и меньше ключа, обновляем результат
        if (currentString.Length % 8 == 0 && currentString.Length >= prefixLength + 8 && string.Compare(currentString, bitString) < 0)
        {
            if (result == null || string.Compare(currentString, result) > 0)
            {
                result = currentString;
            }
        }

        // Обходим все дочерние узлы
        foreach (var child in node.Children)
        {
            LowerRecursive(child, currentString + child.BitString, bitString, prefixLength, ref result);
        }
    }

}
