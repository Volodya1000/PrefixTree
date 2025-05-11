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
        string currentFullBits = accumulatedBits + node.BitString;

        // Формируем строку узла
        sb.Append(indent)
          .Append(last ? "└─ " : "├─ ")
          .Append(node.BitString);

        // Добавляем текстовое представление для конечных узлов
        if (node.IsEnd)
        {
            string text = BitHelper.BitStringToString(currentFullBits);
            sb.Append($"[{text}]");
        }

        sb.AppendLine();

        // Рекурсивный обход потомков (сначала ZeroChild, затем OneChild)
        string newIndent = indent + (last ? "   " : "│  ");
        List<TrieNode> children = new List<TrieNode>();

        // Собираем существующих потомков в порядке: ZeroChild -> OneChild
        if (node.ZeroChild != null) children.Add(node.ZeroChild);
        if (node.OneChild != null) children.Add(node.OneChild);

        for (int i = 0; i < children.Count; i++)
        {
            bool isLastChild = i == children.Count - 1;
            GetASCIIRecursive(children[i], newIndent, currentFullBits, isLastChild, sb);
        }
    }

    public string GetASCIIChildren(string prefix)
    {
        var sb = new StringBuilder();
        List<TrieNode> rootChildren = new List<TrieNode>();

        // Потомки корня: ZeroChild -> OneChild
        if (root.ZeroChild != null) rootChildren.Add(root.ZeroChild);
        if (root.OneChild != null) rootChildren.Add(root.OneChild);

        for (int i = 0; i < rootChildren.Count; i++)
        {
            bool isLastChild = i == rootChildren.Count - 1;
            GetASCII_ChildrenRecursive(rootChildren[i], "", prefix, isLastChild, sb, prefix);
        }

        return sb.ToString();
    }

    private void GetASCII_ChildrenRecursive(TrieNode node, string indent, string accumulatedBits, bool last, StringBuilder sb, string prefix)
    {
        string currentFullBits = accumulatedBits + node.BitString;

        sb.Append(indent)
          .Append(last ? "└─ " : "├─ ")
          .Append(node.BitString);

        if (node.IsEnd)
        {
            string text = BitHelper.BitStringToString(currentFullBits);
            sb.Append($"[{text}]");
        }

        sb.AppendLine();

        // Обход потомков
        string newIndent = indent + (last ? "   " : "│  ");
        List<TrieNode> children = new List<TrieNode>();

        if (node.ZeroChild != null) children.Add(node.ZeroChild);
        if (node.OneChild != null) children.Add(node.OneChild);

        for (int i = 0; i < children.Count; i++)
        {
            bool isLastChild = i == children.Count - 1;
            GetASCII_ChildrenRecursive(children[i], newIndent, currentFullBits, isLastChild, sb, prefix);
        }
    }


}