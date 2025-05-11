namespace CritBit;

public partial class Trie
{
    public (TrieNode, int nodeStoreCount, int busyInPathCount) FindLastNodeInPath(string bitString)
    {
        TrieNode currentNode = root;
        string remaining = bitString;

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
            // Проверяем OneChild, если ZeroChild не подошел
            else if (currentNode.OneChild != null && remaining.StartsWith(currentNode.OneChild.BitString))
            {
                remaining = remaining.Substring(currentNode.OneChild.BitString.Length);
                currentNode = currentNode.OneChild;
                foundFullMatch = true;
            }

            if (!foundFullMatch)
            {
                // Поиск частичного совпадения (remaining - префикс битовой строки ребенка)
                if (currentNode.ZeroChild != null && currentNode.ZeroChild.BitString.StartsWith(remaining))
                {
                    return (currentNode.ZeroChild, currentNode.ZeroChild.BitString.Length, remaining.Length);
                }
                if (currentNode.OneChild != null && currentNode.OneChild.BitString.StartsWith(remaining))
                {
                    return (currentNode.OneChild, currentNode.OneChild.BitString.Length, remaining.Length);
                }

                // Если CheckSubstringExists гарантирует существование пути, эта ошибка не должна возникать
                throw new InvalidOperationException($"Путь недостижим: {BitHelper.BitStringToString(bitString)}");
            }
        }

        // Все биты обработаны - текущий узел последний
        return (currentNode, currentNode.BitString.Length, currentNode.BitString.Length);
    }
}
