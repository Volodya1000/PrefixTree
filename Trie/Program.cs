public class TrieNode
{
    public char? KeyChar { get; }
    public SortedDictionary<char, TrieNode> Children { get; } = new SortedDictionary<char, TrieNode>();
    public bool IsEndOfKey { get; set; }
    public string Value { get; set; }

    public TrieNode(char? keyChar = null)
    {
        KeyChar = keyChar;
    }
}

public class Trie
{
    private readonly TrieNode root = new TrieNode();

    public void Insert(string key, string value)
    {
        TrieNode current = root;
        foreach (char c in key)
        {
            if (!current.Children.TryGetValue(c, out TrieNode child))
            {
                child = new TrieNode(c);
                current.Children[c] = child;
            }
            current = child;
        }
        current.IsEndOfKey = true;
        current.Value = value;
    }

    public string Search(string key)
    {
        TrieNode current = root;
        foreach (char c in key)
        {
            if (!current.Children.TryGetValue(c, out TrieNode child))
            {
                return null;
            }
            current = child;
        }
        return current.IsEndOfKey ? current.Value : null;
    }

    public TrieNode GetRoot() => root;

    public string Upper(string key)
    {
        return Upper(root, key, 0, null);
    }

    private string Upper(TrieNode node, string key, int index, string currentBest)
    {
        if (node == null) return currentBest;

        // Обновляем currentBest только для ключей, которые являются префиксом текущего пути
        if (node.IsEndOfKey && index < key.Length)
        {
            currentBest = node.Value;
        }

        if (index == key.Length)
        {
            // Ищем максимальный ключ в поддереве, исключая точное совпадение
            return FindMaxKey(node, excludeKey: key);
        }

        char currentChar = key[index];

        // Рекурсивный вызов для точного совпадения символа
        if (node.Children.TryGetValue(currentChar, out TrieNode child))
        {
            string result = Upper(child, key, index + 1, currentBest);
            if (result != null) return result;
        }

        // Поиск в ветках с меньшими символами
        foreach (var kvp in node.Children.Reverse())
        {
            if (kvp.Key < currentChar)
            {
                string candidate = FindMaxKey(kvp.Value);
                if (candidate != null && candidate.CompareTo(key) < 0)
                {
                    return candidate;
                }
            }
        }

        return currentBest;
    }

    public string Lower(string key)
    {
        return Lower(root, key, 0, null, key);
    }

    private string Lower(TrieNode node, string key, int index, string currentBest, string originalKey)
    {
        if (node == null) return currentBest;

        if (node.IsEndOfKey)
        {
            string nodeKey = node.Value;
            if (nodeKey.CompareTo(originalKey) > 0) // Только строго большие ключи
            {
                if (currentBest == null || nodeKey.CompareTo(currentBest) < 0)
                {
                    currentBest = nodeKey;
                }
            }
        }

        if (index < key.Length)
        {
            char currentChar = key[index];

            if (node.Children.TryGetValue(currentChar, out TrieNode child))
            {
                currentBest = Lower(child, key, index + 1, currentBest, originalKey);
            }

            // Поиск в ветках с большими символами
            foreach (var kvp in node.Children)
            {
                if (kvp.Key > currentChar)
                {
                    string candidate = FindMinKey(kvp.Value);
                    if (candidate != null && candidate.CompareTo(originalKey) > 0)
                    {
                        if (currentBest == null || candidate.CompareTo(currentBest) < 0)
                        {
                            currentBest = candidate;
                        }
                    }
                }
            }
        }
        else
        {
            // После полного прохода ключа ищем минимальный в поддереве
            string candidate = FindMinKey(node);
            if (candidate != null && candidate.CompareTo(originalKey) > 0)
            {
                currentBest = candidate;
            }
        }

        return currentBest;
    }

    private string FindMaxKey(TrieNode node, string excludeKey = null)
    {
        if (node == null) return null;

        string maxKey = null;
        if (node.IsEndOfKey && node.Value != excludeKey)
        {
            maxKey = node.Value;
        }

        foreach (var child in node.Children.Reverse())
        {
            string candidate = FindMaxKey(child.Value, excludeKey);
            if (candidate != null)
            {
                if (maxKey == null || candidate.CompareTo(maxKey) > 0)
                {
                    maxKey = candidate;
                }
            }
        }
        return maxKey;
    }

    private string FindMinKey(TrieNode node)
    {
        if (node == null) return null;

        string minKey = null;
        if (node.IsEndOfKey)
        {
            minKey = node.Value;
        }

        foreach (var child in node.Children)
        {
            string candidate = FindMinKey(child.Value);
            if (candidate != null)
            {
                if (minKey == null || candidate.CompareTo(minKey) < 0)
                {
                    minKey = candidate;
                }
            }
        }
        return minKey;
    }
}

class Program
{
    static Trie trie = new Trie();

    static void Main(string[] args)
    {
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("1. Вставить элемент");
            Console.WriteLine("2. Найти элемент");
            Console.WriteLine("3. Показать дерево");
            Console.WriteLine("4. Найти верхний элемент");
            Console.WriteLine("5. Найти нижний элемент");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    InsertElement();
                    break;
                case "2":
                    SearchElement();
                    break;
                case "3":
                    PrintTrie(trie.GetRoot());
                    break;
                case "4":
                    FindUpper();
                    break;
                case "5":
                    FindLower();
                    break;
                case "6":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }

    static void InsertElement()
    {
        Console.Write("Введите ключ: ");
        string key = Console.ReadLine();
        trie.Insert(key, key);
        Console.WriteLine("Элемент вставлен.\n");
    }

    static void SearchElement()
    {
        Console.Write("Введите ключ для поиска: ");
        string key = Console.ReadLine();
        string value = trie.Search(key);
        Console.WriteLine(value != null
            ? $"Найдено значение: {value}\n"
            : "Ключ не найден.\n");
    }

    static void FindUpper()
    {
        Console.Write("Введите ключ для поиска верхнего элемента: ");
        string key = Console.ReadLine();
        string value = trie.Upper(key);
        Console.WriteLine(value != null
            ? $"Верхний элемент: {value}\n"
            : "Верхний элемент не найден.\n");
    }

    static void FindLower()
    {
        Console.Write("Введите ключ для поиска нижнего элемента: ");
        string key = Console.ReadLine();
        string value = trie.Lower(key);
        Console.WriteLine(value != null
            ? $"Нижний элемент: {value}\n"
            : "Нижний элемент не найден.\n");
    }

    static void PrintTrie(TrieNode node, string indent = "", bool isLast = true, bool isRoot = true)
    {
        string current = isRoot ? "Root" : node.KeyChar?.ToString() ?? "";
        string valuePart = node.IsEndOfKey ? $" ({node.Value})" : "";

        Console.Write(indent);
        if (!isRoot) Console.Write(isLast ? "└── " : "├── ");
        Console.Write(current + valuePart);
        Console.WriteLine();

        string newIndent = indent + (isLast ? "    " : "│   ");
        int i = 0;
        var children = new List<TrieNode>(node.Children.Values);

        foreach (var child in children)
        {
            bool childIsLast = i == children.Count - 1;
            PrintTrie(child, newIndent, childIsLast, false);
            i++;
        }
    }
}