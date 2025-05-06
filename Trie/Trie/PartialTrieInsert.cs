namespace CritBit;

public partial class Trie
{
    /// <summary>
    /// Вставка битовой строки в дерево
    /// </summary>
    /// <param name="bitString">Вставляемая битовая строка</param>
    public void Insert(string bitString)
    {
        InsertRecursive(root, bitString);
    }

    /// <summary>
    /// Рекурсивная вставка с автоматическим разделением узлов
    /// </summary>
    private void InsertRecursive(TrieNode node, string remainingBits)
    {
        // Поиск общего префикса с существующими дочерними узлами
        foreach (var child in node.Children.ToList())
        {
            int commonLength = FindCommonPrefix(child.BitString, remainingBits);

            if (commonLength > 0)
            {
                if (commonLength == child.BitString.Length)
                {
                    // Если общий префикс полность совпадает - переходим к ребенку
                    InsertRecursive(child, remainingBits.Substring(commonLength));
                    return;
                }
                else
                {
                    // Разделяем узел при частичном совпадении
                    SplitChildNode(node, child, commonLength, remainingBits);
                    return;
                }
            }
        }

        // Создаем новый узел если нет совпадений
        TrieNode newChild = new TrieNode(remainingBits);
        newChild.IsEnd = true;
        node.Children.Add(newChild);
    }

    /// <summary>
    /// Разделение узла при частичном совпадении префиксов
    /// </summary>
    private void SplitChildNode(TrieNode parentNode, TrieNode childNode, int commonLength, string remainingBits)
    {
        // Разделяем битовую строку узла
        string commonPart = childNode.BitString.Substring(0, commonLength);
        string childRemaining = childNode.BitString.Substring(commonLength);
        string newRemaining = remainingBits.Substring(commonLength);

        // Создаем новый узел для общего префикса
        parentNode.Children.Remove(childNode);
        TrieNode newCommonNode = new TrieNode(commonPart);
        parentNode.Children.Add(newCommonNode);

        // Перенастраиваем оригинальный узел
        childNode.BitString = childRemaining;
        newCommonNode.Children.Add(childNode);

        // Добавляем новый узел для оставшихся битов
        if (newRemaining.Length > 0)
        {
            TrieNode newChild = new TrieNode(newRemaining);
            newChild.IsEnd = true;
            newCommonNode.Children.Add(newChild);
        }
        else
        {
            newCommonNode.IsEnd = true;
        }
    }


    /// <summary>
    /// Поиск длины общего префикса для двух строк
    /// </summary>
    private int FindCommonPrefix(string a, string b)
    {
        int minLength = Math.Min(a.Length, b.Length);
        for (int i = 0; i < minLength; i++)
        {
            if (a[i] != b[i])
                return i;
        }
        return minLength;
    }
}
