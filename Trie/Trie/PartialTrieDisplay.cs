namespace CritBit;

public partial class Trie
{

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


        //for (int i = 0; i < node.Children.Count; i++)
        var sortedChildren = node.Children.OrderBy(c => c.BitString).ToList();

        for (int i = 0; i < sortedChildren.Count; i++)
        {
            bool isLastChild = i == node.Children.Count - 1;
            DisplayRecursive(sortedChildren[i], newIndent, currentFullBits, isLastChild);
        }
    }

}