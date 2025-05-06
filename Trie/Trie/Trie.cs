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

    public Trie(TrieNode rootNode)=>root = rootNode;

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




    //======================
    //Рабочий вариант 31 04
    public string ? Lower_work(string bitString,int prefixLength)
    {
        if (!IsValidByteString(bitString))
            throw new ArgumentException("Key length must be multiple of 8 bits");

        int maxAllowedLength = bitString.Length + 96;

        var filtered = GetAllSubstringsMultipleOf8()
            .Where(s => s.Length > prefixLength + 8 &&
                       string.Compare(s.Substring(prefixLength), bitString) < 0)
            .OrderByDescending(s => s)
            .FirstOrDefault();
        if (filtered == null) return null;
        if (filtered.Length < prefixLength + 8) return null;
        return filtered.Substring(0, prefixLength+8);
    }

    //public string? Lower(string bitString,int a)
    //{
    //    if (!IsValidByteString(bitString))
    //        throw new ArgumentException("Key length must be multiple of 8 bits");

    //    int maxAllowedLength = bitString.Length + 96;

    //    return GetAllSubstringsMultipleOf8()
    //        .Where(s => 
    //                   string.Compare(s, bitString)< 0)
    //        .OrderBy(s => s)
    //        .FirstOrDefault();

       
    //}

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

   

    // Рабочий вариант 31 03
    //public string Upper(string key,int prefixLength)
    //{
    //    if (!IsValidByteString(key))
    //        throw new ArgumentException("Key length must be multiple of 8 bits");

       

    //    var filtered = GetAllSubstringsMultipleOf8()
    //        .Where(s => s.Length >= prefixLength + 8 &&
    //                   string.Compare(s.Substring(prefixLength), key) > 0)
    //        .OrderBy(s => s)
    //        .FirstOrDefault();
    //    if (filtered == null) return null;
    //    if (filtered.Length < prefixLength + 8) return null;
    //    return filtered.Substring(0, prefixLength + 8);
    //}


    /*
    1. Преверить все  ли биты взяты из корня. 
    2. Если нет, то резултат равен недостающим битам
    2.1. Взять 8 бит по левой ветке Выход
    3. Если в первой после корня больше либо равно 8 то взять k иначе взять до 8 Выход
    

     */
    public string LeftBranch(int tookFromRoot)
    {
        TrieNode current = root;
        //    = FindNode(bitString);
        //if (current == null) return null;

        string result = "";

        if (tookFromRoot != root.BitString.Length)
            result += root.BitString.Substring(tookFromRoot);

        //Получаем первого наследника
        TrieNode firstChild = current.Children.FirstOrDefault(c => c.BitString.StartsWith("0"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("1"));

        if (firstChild == null) return null;

        int firstChildLength = firstChild.BitString.Length;

        //Смотрим сколько бит в первом наследнике 
        if (firstChildLength >= 8)
        {
            int k = CalculateK(firstChildLength);
            result += firstChild.BitString.Substring(0, k);
            return result;
        }

        while (result.Length < 8)
        {
            TrieNode nextChild = current.Children
           .FirstOrDefault(c => c.BitString.StartsWith("0"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("1"));

            if (nextChild == null) break;

            int available = nextChild.BitString.Length;
            int needed = 8 - result.Length;
            int take = Math.Min(available, needed);

            result += nextChild.BitString.Substring(0, take);
            current = nextChild;
        }
        return result;
    }


}
