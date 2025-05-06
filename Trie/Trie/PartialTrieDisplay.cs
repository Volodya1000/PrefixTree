using System.Text;

namespace CritBit;

public partial class Trie
{

    public string GetASCII()
    {
        var sb = new StringBuilder();
        GetASCIIRecursive(root, "", "", true, sb);
        return sb.ToString();
    }

    private void GetASCIIRecursive(TrieNode node, string indent, string accumulatedBits, bool last, StringBuilder sb)
    {
        // Накопленная битовая строка по пути от корня
        string currentFullBits = accumulatedBits + node.BitString;

        // Формируем строку узла
        sb.Append(indent);
        sb.Append(last ? "└─ " : "├─ ");
        sb.Append(node.BitString);

        // Для конечных узлов добавляем текстовое представление
        if (node.IsEnd)
        {
            string text = BitHelper.BitStringToString(currentFullBits);
            sb.Append($"[{text}]");
        }

        sb.AppendLine();

        // Рекурсивный обход дочерних узлов
        string newIndent = indent + (last ? "   " : "│  ");

        var sortedChildren = node.Children.OrderBy(c => c.BitString).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            bool isLastChild = i == sortedChildren.Count - 1;
            GetASCIIRecursive(sortedChildren[i], newIndent, currentFullBits, isLastChild, sb);
        }
    }

    // <summary>
    /// Визуализация дерева в консоли с ASCII-графикой для потомков корня
    /// </summary>
    public string GetASCIIChildren(string prefix)
    {
        var sb = new StringBuilder();
        var sortedChildren = root.Children.OrderBy(c => c.BitString).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            bool isLastChild = i == sortedChildren.Count - 1;
            GetASCII_ChildrenRecursive(sortedChildren[i], "", prefix, isLastChild, sb, prefix);
        }

        return sb.ToString();
    }


    private void GetASCII_ChildrenRecursive(TrieNode node, string indent, string accumulatedBits, bool last, StringBuilder sb,string prefix)
    {
        // Накопленная битовая строка по пути от корня
        string currentFullBits = accumulatedBits + node.BitString;

        // Формируем строку узла
        sb.Append(indent);
        sb.Append(last ? "└─ " : "├─ ");
        sb.Append(node.BitString);

        // Для конечных узлов добавляем текстовое представление
        if (node.IsEnd)
        {
            string text = BitHelper.BitStringToString(currentFullBits);
            sb.Append($"[{text}]");
        }

        sb.AppendLine();

        // Рекурсивный обход дочерних узлов
        string newIndent = indent + (last ? "   " : "│  ");

        var sortedChildren = node.Children.OrderBy(c => c.BitString).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            bool isLastChild = i == sortedChildren.Count - 1;
            GetASCII_ChildrenRecursive(sortedChildren[i], newIndent, currentFullBits, isLastChild, sb,prefix);
        }
    }


}