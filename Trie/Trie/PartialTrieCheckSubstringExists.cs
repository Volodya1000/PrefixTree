namespace CritBit;

public partial class Trie
{
    public bool CheckSubstringExists(string bitString, int tookFromRoot)
    {
        if (string.IsNullOrEmpty(bitString))
            return false;

        if (bitString.Length % 8 != 0)
            throw new ArgumentException("Bit string length must be multiple of 8");

        if (tookFromRoot < 0 || tookFromRoot > root.BitString.Length)
            throw new ArgumentOutOfRangeException(nameof(tookFromRoot));

        // Проверяем, что взятая часть корня соответствует началу битовой строки
        string rootSubstring = root.BitString.Substring(tookFromRoot);
        if (!bitString.StartsWith(rootSubstring))
            return false;

        // Обрезаем проверенную часть корня
        string remaining = bitString.Substring(rootSubstring.Length);

        return CheckSubstringExistsRecursive(root, remaining);
    }

    private bool CheckSubstringExistsRecursive(TrieNode node, string remainingBits)
    {
        if (remainingBits.Length == 0)
            return true;

        foreach (var child in new[] { node.ZeroChild, node.OneChild })
        {
            if (child == null) continue;

            // Проверяем полное совпадение
            if (remainingBits.StartsWith(child.BitString))
            {
                string newRemaining = remainingBits.Substring(child.BitString.Length);
                if (CheckSubstringExistsRecursive(child, newRemaining))
                    return true;
            }
            // Проверяем частичное совпадение (оставшиеся биты - префикс данных узла)
            else if (child.BitString.StartsWith(remainingBits))
            {
                return true;
            }
        }

        return false;
    }

}