using System.Text;

namespace CritBit;


/// <summary>
/// Префиксное дерево для работы с битовыми строками
/// </summary>
public partial class Trie 
{
    public TrieNode root;

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
    public string Upper_(string bitString)
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
    public string Lower1(string bitString)
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
            //string lowerBit = Lower(currentFullBits);
            //string upperBit = Upper(currentFullBits,);
            //string lower = BitHelper.BitStringToString(lowerBit);
            //string upper = BitHelper.BitStringToString(upperBit);
            //Console.Write($"Lower:{lower} ");
            //Console.Write($"Upper:{upper}");
        }

        Console.WriteLine();

        // Рекурсивный обход дочерних узлов
        string newIndent = indent + (last ? "   " : "│  ");


        //for (int i = 0; i < node.Children.Count; i++)
        var sortedChildren = node.Children.OrderBy(c => c.BitString).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            bool isLastChild = i == node.Children.Count - 1;
            DisplayRecursive(sortedChildren[i], newIndent, currentFullBits, isLastChild);
        }
    }



    //======================
    //Рабочий вариант 31 04
    public string Lower(string bitString,int prefixLength)
    {
        if (!IsValidByteString(bitString))
            throw new ArgumentException("Key length must be multiple of 8 bits");

        int maxAllowedLength = bitString.Length + 96;

        var filtered = GetAllSubstringsMultipleOf8()
            .Where(s => s.Length > prefixLength + 8 &&
                       string.Compare(s, bitString) < 0)
            .OrderByDescending(s => s)
            .FirstOrDefault();
        return filtered;
        /*string bestCandidate = null;
        FindBestCandidate(root, "", bitString, ref bestCandidate);

        if (bestCandidate == null)
            return null;

        // Находим максимально возможное продолжение префикса
        string result = FindMaxExtension(bestCandidate, bitString.Length);

        return result != null && result.Length > bitString.Length ? result : null;
        */
    }

    private void FindBestCandidate(TrieNode node, string currentPrefix, string target, ref string bestCandidate)
    {
        string newPrefix = currentPrefix + node.BitString;

        // Сравниваем только до длины целевой строки
        int compareLength = Math.Min(newPrefix.Length, target.Length);
        int compareResult = string.Compare(
            newPrefix.Substring(0, compareLength),
            target.Substring(0, compareLength)
        );

        if (compareResult < 0)
        {
            if (newPrefix.Length <= target.Length )//|| IsPathExist(newPrefix))
            {
                // Обновляем кандидат если он больше предыдущего
                if (bestCandidate == null || string.Compare(newPrefix, bestCandidate) > 0)
                    bestCandidate = newPrefix;
            }
        }

        // Рекурсивно проверяем дочерние узлы в обратном порядке
        foreach (var child in node.Children.OrderByDescending(c => c.BitString))
        {
            FindBestCandidate(child, newPrefix, target, ref bestCandidate);
        }
    }

    private string FindMaxExtension(string prefix, int targetLength)
    {
        TrieNode node = FindLeaf(prefix);
        if (node == null) return null;

        StringBuilder result = new StringBuilder(prefix);

        // Идем по максимальным дочерним узлам пока не достигнем нужной длины
        while (result.Length <= targetLength + 96)
        {
            var children = node.Children.OrderByDescending(c => c.BitString).ToList();
            if (children.Count == 0) break;

            // Выбираем последний дочерний узел (максимальный)
            TrieNode maxChild = children[0];
            result.Append(maxChild.BitString);
            node = maxChild;
        }

        // Обрезаем до минимальной требуемой длины
        int requiredLength = targetLength + 8 - (targetLength % 8); // здест неправильно Исправить на условие из определения и добавить возможность идти по наследникам в случае если длинна не достаточна
        return result.Length >= requiredLength ?
            result.ToString(0, requiredLength) :
            null;
    }

    //public string Lower(string bitString)
    //{
    //    string bestCandidate = null;
    //    FindBestCandidate(root, "", bitString, ref bestCandidate);

    //    if (bestCandidate == null)
    //        return null;

    //    TrieNode currentNode = FindNode(bestCandidate);
    //    if (currentNode == null)
    //        return null;

    //    int x = CalculateX(currentNode);
    //    int k = CalculateK(x);

    //    int targetLength = bitString.Length + k;
    //    string result = ExtendPrefix(currentNode, bestCandidate, targetLength);

    //    if (result == null )//|| !IsPathExist(result))
    //        return null;

    //    return result;
    //}

    //private void FindBestCandidate(TrieNode node, string currentPrefix, string target, ref string bestCandidate)
    //{
    //    string newPrefix = currentPrefix + node.BitString;
    //    int compareResult = ComparePrefixes(newPrefix, target);

    //    if (compareResult < 0)
    //    {
    //        UpdateBestCandidate(newPrefix, ref bestCandidate, target);
    //    }
    //    else if (compareResult == 0)
    //    {
    //        foreach (var child in node.Children.OrderBy(c => c.BitString))
    //        {
    //            FindBestCandidate(child, newPrefix, target, ref bestCandidate);
    //        }
    //    }

    //    if (node.IsEnd && ComparePrefixes(newPrefix, target) < 0)
    //    {
    //        UpdateBestCandidate(newPrefix, ref bestCandidate, target);
    //    }
    //}

    //private int ComparePrefixes(string a, string b)
    //{
    //    int minLen = Math.Min(a.Length, b.Length);
    //    for (int i = 0; i < minLen; i++)
    //    {
    //        if (a[i] < b[i]) return -1;
    //        if (a[i] > b[i]) return 1;
    //    }
    //    return a.Length.CompareTo(b.Length);
    //}

    //private void UpdateBestCandidate(string newCandidate, ref string bestCandidate, string target)
    //{
    //    if (bestCandidate == null)
    //    {
    //        bestCandidate = newCandidate;
    //        return;
    //    }

    //    int currentCompare = ComparePrefixes(newCandidate, bestCandidate);
    //    if (currentCompare > 0 && ComparePrefixes(newCandidate, target) < 0)
    //    {
    //        bestCandidate = newCandidate;
    //    }
    //}


    /// <summary>
    /// Находит лист в дереве. Не любую подстроку, а только лист 
    /// </summary>
    /// <param name="prefix"></param>
    /// <returns></returns>
    public TrieNode? FindLeaf(string prefix)
    {
        return FindLeafRecursive(root, prefix, 0);
    }

    private TrieNode? FindLeafRecursive(TrieNode node, string prefix, int pos)
    {
        if (pos == prefix.Length)
            return node;

        foreach (var child in node.Children)
        {
            if (prefix.Length - pos < child.BitString.Length)
                continue;

            bool match = true;
            for (int i = 0; i < child.BitString.Length; i++)
            {
                if (child.BitString[i] != prefix[pos + i])
                {
                    match = false;
                    break;
                }
            }

            if (match)
            {
                var result = FindLeafRecursive(child, prefix, pos + child.BitString.Length);
                if (result != null)
                    return result;
            }
        }

        return null;
    }

   



    // LOWER IMPLEMENTATION
    public string Lower__(string key)
    {
        if (!IsValidKey(key)) return null;

        int requiredLength = key.Length + 8;
        var maxSPath = FindMaxSPath(key);

        if (maxSPath == null) return null;

        for (int i = maxSPath.Count - 1; i >= 0; i--)
        {
            string currentPrefix = GetPathString(maxSPath.Take(i + 1));
            var extension = FindLowerExtension(
                maxSPath[i],
                requiredLength - currentPrefix.Length,
                currentPrefix,
                key
            );

            if (extension != null && (currentPrefix + extension).Length == requiredLength)
                return currentPrefix + extension;
        }

        return null;
    }

    private List<TrieNode> FindMaxSPath(string key)
    {
        var stack = new Stack<(TrieNode Node, List<TrieNode> Path)>();
        stack.Push((root, new List<TrieNode> { root }));

        List<TrieNode> maxPath = null;
        int maxLength = -1;

        while (stack.Count > 0)
        {
            var current = stack.Pop();
            string pathStr = GetPathString(current.Path);

            if (IsPrefixLess(pathStr, key) && pathStr.Length < key.Length)
            {
                if (pathStr.Length > maxLength)
                {
                    maxLength = pathStr.Length;
                    maxPath = current.Path;
                }

                foreach (var child in current.Node.Children
                    .OrderByDescending(c => c.BitString))
                {
                    var newPath = new List<TrieNode>(current.Path) { child };
                    stack.Push((child, newPath));
                }
            }
        }

        return maxPath;
    }

    private string FindLowerExtension(
        TrieNode node,
        int remainingBits,
        string currentPrefix,
        string key
    )
    {
        if (remainingBits < 0) return null;
        if (remainingBits == 0)
            return IsValidLowerCandidate(currentPrefix, key) ? "" : null;

        foreach (var child in node.Children
            .OrderByDescending(c => c.BitString))
        {
            if (child.BitString.Length > remainingBits) continue;

            string newPrefix = currentPrefix + child.BitString;
            if (!IsPrefixValidLower(newPrefix, key)) continue;

            string extension = FindLowerExtension(
                child,
                remainingBits - child.BitString.Length,
                newPrefix,
                key
            );

            if (extension != null)
                return child.BitString + extension;
        }

        return node.IsEnd && IsValidLowerCandidate(currentPrefix, key) ? "" : null;
    }

    // Рабочий вариант 31 03
    public string Upper__(string key,int prefixLength)
    {
        if (!IsValidByteString(key))
            throw new ArgumentException("Key length must be multiple of 8 bits");

       

        var filtered = GetAllSubstringsMultipleOf8()
            .Where(s => s.Length >= prefixLength + 8 &&
                       string.Compare(s, key) > 0)
            .OrderBy(s => s)
            .FirstOrDefault();
        return filtered;

       
    }


    private string FindUpperExtension(
        TrieNode node,
        int remainingBits,
        string currentPrefix,
        string key
    )
    {
        if (remainingBits < 0) return null;
        if (remainingBits == 0)
            return IsValidUpperCandidate(currentPrefix, key) ? "" : null;

        foreach (var child in node.Children
            .OrderBy(c => c.BitString.Length)
            .ThenBy(c => c.BitString))
        {
            if (child.BitString.Length > remainingBits) continue;

            string newPrefix = currentPrefix + child.BitString;
            if (!IsPrefixValidUpper(newPrefix, key)) continue;

            string extension = FindUpperExtension(
                child,
                remainingBits - child.BitString.Length,
                newPrefix,
                key
            );

            if (extension != null)
                return child.BitString + extension;
        }

        return null;
    }

    #region Helper Methods

    private bool IsValidKey(string key)
    {
        return key != null && key.Length % 8 == 0;
    }

    private string GetPathString(IEnumerable<TrieNode> path)
    {
        return string.Concat(path.Select(n => n.BitString));
    }

    private bool IsPrefixLess(string a, string b)
    {
        int minLen = Math.Min(a.Length, b.Length);
        for (int i = 0; i < minLen; i++)
        {
            if (a[i] < b[i]) return true;
            if (a[i] > b[i]) return false;
        }
        return a.Length < b.Length;
    }

    private bool IsPrefixGreaterOrEqual(string a, string b)
    {
        int minLen = Math.Min(a.Length, b.Length);
        for (int i = 0; i < minLen; i++)
        {
            if (a[i] > b[i]) return true;
            if (a[i] < b[i]) return false;
        }
        return a.Length >= b.Length;
    }

    private bool IsValidLowerCandidate(string candidate, string key)
    {
        return candidate.Length == key.Length + 8 &&
               candidate.StartsWith(key.Substring(0, Math.Min(candidate.Length, key.Length))) &&
               string.Compare(candidate, 0, key, 0, key.Length) < 0;
    }

    private bool IsValidUpperCandidate(string candidate, string key)
    {
        return candidate.Length == key.Length + 8 &&
               string.Compare(candidate, 0, key, 0, key.Length) >= 0;
    }

    private bool IsPrefixValidLower(string prefix, string key)
    {
        if (prefix.Length <= key.Length)
            return IsPrefixLess(prefix, key);

        return IsPrefixLess(prefix.Substring(0, key.Length), key);
    }

    private bool IsPrefixValidUpper(string prefix, string key)
    {
        if (prefix.Length <= key.Length)
            return IsPrefixGreaterOrEqual(prefix, key);

        return IsPrefixGreaterOrEqual(prefix.Substring(0, key.Length), key);
    }

    #endregion
}
