namespace CritBit;

public partial class Trie
{
    /// <summary>
    /// Обход префиксного дерева в лексикографическом порядке
    /// </summary>
    public List<string> TraverseInLexicalOrder()
    {
        var result = new List<string>();
        TraverseRecursive(root, "", result);
        return result;
    }

    /// <summary>
    /// Рекурсивный обход дерева
    /// </summary>
    private void TraverseRecursive(TrieNode node, string currentString, List<string> result)
    {
        if (node.IsEnd)
        {
            result.Add(currentString);
        }

        // Сортируем дочерние узлы по лексикографическому порядку
        var sortedChildren = node.Children.OrderBy(c => c.BitString).ToList();

        foreach (var child in sortedChildren)
        {
            TraverseRecursive(child, currentString + child.BitString, result);
        }
    }




    /// <summary>
    /// Возвращает все подстроки в дереве, длина которых кратна 8
    /// </summary>
    public List<string> GetAllSubstringsMultipleOf8()
    {
        var result = new List<string>();
        GetAllSubstringsMultipleOf8Recursive(root, "", result);
        return result;
    }

    /// <summary>
    /// Рекурсивный обход дерева для сбора всех подстрок, длина которых кратна 8
    /// </summary>
    private void GetAllSubstringsMultipleOf8Recursive(TrieNode node, string currentString, List<string> result)
    {
        // Если длина текущей строки кратна 8, добавляем её в результат
        if (currentString.Length % 8 == 0 && !string.IsNullOrEmpty(currentString))
        {
            result.Add(currentString);
        }

        // Обходим все дочерние узлы
        foreach (var child in node.Children)
        {
            GetAllSubstringsMultipleOf8Recursive(child, currentString + child.BitString, result);
        }
    }
}
