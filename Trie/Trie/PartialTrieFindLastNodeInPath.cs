namespace CritBit;

public partial class Trie
{
    public (TrieNode, int lastNodeBitBusyInPathCount) FindLastNodeInPath(string bitString, int tookFromRoot)
    {
        // Проверка корректности параметра tookFromRoot
        if (tookFromRoot < 0 || tookFromRoot > root.BitString.Length)
            throw new ArgumentOutOfRangeException(nameof(tookFromRoot));

        // Вычисляем часть корня, которая уже была обработана
        string rootSubstring = root.BitString.Substring(tookFromRoot);

        // Проверяем, что bitString начинается с обработанной части корня
        if (!bitString.StartsWith(rootSubstring))
            throw new InvalidOperationException("Битовая строка не соответствует корню");

        TrieNode currentNode = root;
        string remaining = bitString.Substring(rootSubstring.Length); // Обрезаем проверенную часть
        while (remaining.Length > 0)
        {
            TrieNode zeroChild = currentNode.GetZeroChild();
            TrieNode oneChild = currentNode.GetOneChild();
            bool foundFullMatch = false;

            if (zeroChild != null && remaining.StartsWith(zeroChild.BitString))
            {
                remaining = remaining.Substring(zeroChild.BitString.Length);
                currentNode = zeroChild;
                foundFullMatch = true;
            }
            else if (oneChild != null && remaining.StartsWith(oneChild.BitString))
            {
                remaining = remaining.Substring(oneChild.BitString.Length);
                currentNode = oneChild;
                foundFullMatch = true;
            }

            if (!foundFullMatch)
            {
                // Поиск частичного совпадения (remaining - префикс битовой строки потомка)
                if (zeroChild != null && zeroChild.BitString.StartsWith(remaining))
                    return (zeroChild, remaining.Length);

                if (oneChild != null && oneChild.BitString.StartsWith(remaining))
                    return (oneChild, remaining.Length);

                throw new InvalidOperationException($"Путь недостижим: {BitStringToString(bitString)}");
                return (null, -1);
            }
        }

        // Все биты обработаны, возвращаем текущий узел
        return (currentNode, currentNode.BitString.Length);
    }
}
