namespace CritBit;

public partial class Trie
{
    public (TrieNode, int nodeStoreCount, int busyInPathCount) FindLastNodeInPath(string bitString, int tookFromRoot)
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
            bool foundFullMatch = false;

            // Проверяем ZeroChild
            if (currentNode.ZeroChild != null && remaining.StartsWith(currentNode.ZeroChild.BitString))
            {
                remaining = remaining.Substring(currentNode.ZeroChild.BitString.Length);
                currentNode = currentNode.ZeroChild;
                foundFullMatch = true;
            }
            // Проверяем OneChild, если ZeroChild не подошёл
            else if (currentNode.OneChild != null && remaining.StartsWith(currentNode.OneChild.BitString))
            {
                remaining = remaining.Substring(currentNode.OneChild.BitString.Length);
                currentNode = currentNode.OneChild;
                foundFullMatch = true;
            }

            if (!foundFullMatch)
            {
                // Поиск частичного совпадения (remaining - префикс битовой строки потомка)
                if (currentNode.ZeroChild != null && currentNode.ZeroChild.BitString.StartsWith(remaining))
                    return (currentNode.ZeroChild, currentNode.ZeroChild.BitString.Length, remaining.Length);

                if (currentNode.OneChild != null && currentNode.OneChild.BitString.StartsWith(remaining))
                    return (currentNode.OneChild, currentNode.OneChild.BitString.Length, remaining.Length);

                throw new InvalidOperationException($"Путь недостижим: {BitHelper.BitStringToString(bitString)}");
            }
        }

        // Все биты обработаны, возвращаем текущий узел
        return (currentNode, currentNode.BitString.Length, currentNode.BitString.Length);
    }
}
