namespace CritBit;

public partial class Trie
{
    public bool CheckSubstringExists(string bitString)
    {
        if (bitString.Length % 8 != 0)
            throw new ArgumentException("Bit string length must be multiple of 8");

        return CheckSubstringExistsRecursive(root, bitString);
    }

    private bool CheckSubstringExistsRecursive(TrieNode node, string remainingBits)
    {
        // Проверяем полное совпадение части узла с началом оставшихся бит
        if (remainingBits.StartsWith(node.BitString))
        {
            string newRemaining = remainingBits.Substring(node.BitString.Length);
            if (newRemaining.Length == 0)
                return true;

            // Рекурсивно проверяем дочерние узлы
            foreach (var child in node.Children)
            {
                if (CheckSubstringExistsRecursive(child, newRemaining))
                    return true;
            }
            return false;
        }
        // Проверяем частичное совпадение (оставшиеся биты - префикс данных узла)
        else
        {
            return node.BitString.StartsWith(remainingBits);
        }
    }

}