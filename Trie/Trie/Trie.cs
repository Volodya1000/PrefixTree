namespace Trie;


/// <summary>
/// Префиксное дерево для работы с битовыми строками
/// </summary>
public class Trie : ITrie
{
    private TrieNode root;

    public Trie()
    {
        root = new TrieNode("");
    }

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

    /// <summary>
    /// Поиск точного совпадения битовой строки в дереве
    /// </summary>
    public bool Search(string bitString)
    {
        return SearchRecursive(root, bitString);
    }

    private bool SearchRecursive(TrieNode node, string remainingBits)
    {
        if (remainingBits.StartsWith(node.BitString))
        {
            string newRemaining = remainingBits.Substring(node.BitString.Length);
            if (newRemaining == "")
                return node.IsEnd;

            foreach (var child in node.Children)
            {
                if (SearchRecursive(child, newRemaining))
                    return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Находит верхнюю границу (максимальную битовую строку меньшую заданной)
    /// </summary>
    public string Upper(string bitString)
    {
        return FindUpperBound(root, "", bitString);
    }

    private string FindUpperBound(TrieNode node, string currentPath, string target)
    {
        currentPath += node.BitString;
        string bestMatch = null;

        // Проверка текущего узла как кандидата
        if (node.IsEnd && IsValidByteString(currentPath))
        {
            if (currentPath.CompareTo(target) < 0)
            {
                bestMatch = currentPath;
            }
        }

        //обход детей в обратном порядке
        foreach (var child in node.Children.OrderByDescending(c => c.BitString))
        {
            string childResult = FindUpperBound(child, currentPath, target);

            if (childResult == null) continue;

            // Обновление лучшего результата
            if (bestMatch == null || childResult.CompareTo(bestMatch) > 0)
            {
                bestMatch = childResult;

                // Ранний выход: найден максимальный возможный результат
                if (childResult.CompareTo(node.Children.First().BitString) >= 0)
                {
                    break;
                }
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Находит нижнюю границу (минимальную битовую строку большую заданной)
    /// </summary>
    public string Lower(string bitString)
    {
        return FindLowerBound(root, "", bitString);
    }

    private string FindLowerBound(TrieNode node, string currentPath, string target)
    {
        currentPath += node.BitString;
        string bestMatch = null;

        // Проверка текущего узла как кандидата
        if (node.IsEnd && IsValidByteString(currentPath))
        {
            if (currentPath.CompareTo(target) > 0)
            {
                bestMatch = currentPath;
            }
        }

        //обход детей в прямом порядке
        foreach (var child in node.Children.OrderBy(c => c.BitString))
        {
            string childResult = FindLowerBound(child, currentPath, target);

            if (childResult == null) continue;

            // Обновление лучшего результата
            if (bestMatch == null || childResult.CompareTo(bestMatch) < 0)
            {
                bestMatch = childResult;

                // Ранний выход: найден минимальный возможный результат
                if (childResult.CompareTo(node.Children.Last().BitString) <= 0)
                {
                    break;
                }
            }
        }

        return bestMatch;
    }

    /// <summary>
    /// Проверка валидности битовой строки (длина кратна 8 битам)
    /// </summary>
    private bool IsValidByteString(string bitString)
    {
        return bitString.Length % 8 == 0;
    }


    /// <summary>
    /// Визуализация дерева в консоли с ASCII-графикой
    /// </summary>
    public void Display()
    {
        DisplayRecursive(root, "", "", true);
    }

    private void DisplayRecursive(TrieNode node, string indent, string accumulatedBits, bool last)
    {
        // Накопленная битовая строка по пути от корня
        string currentFullBits = accumulatedBits + node.BitString;

        // Формируем строку узла
        Console.Write(indent);
        Console.Write(last ? "└─ " : "├─ ");
        Console.Write(node.BitString);

        // Для конечных узлов добавляем текстовое представление
        if (node.IsEnd)
        {
            string text = BitHelper.BitStringToString(currentFullBits);
            Console.Write($"[{text}]");
        }

        Console.WriteLine();

        // Рекурсивный обход дочерних узлов
        string newIndent = indent + (last ? "   " : "│  ");
        for (int i = 0; i < node.Children.Count; i++)
        {
            bool isLastChild = i == node.Children.Count - 1;
            DisplayRecursive(node.Children[i], newIndent, currentFullBits, isLastChild);
        }
    }
}
