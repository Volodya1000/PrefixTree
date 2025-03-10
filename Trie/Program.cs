// Узел префиксного дерева
public class TrieNode
{
    // Символ, который представляет этот узел (null для корня)
    public char? KeyChar { get; }

    // Отсортированные дочерние узлы (автоматически сортируются по ключам)
    public SortedDictionary<char, TrieNode> Children { get; } = new SortedDictionary<char, TrieNode>();

    // Флаг окончания ключа
    public bool IsEndOfKey { get; set; }

    // Значение, связанное с ключом (если IsEndOfKey = true)
    public string Value { get; set; }

    public TrieNode(char? keyChar = null)
    {
        KeyChar = keyChar;
    }
}

public class Trie
{
    // Корневой узел дерева (не содержит символа)
    private readonly TrieNode root = new TrieNode();

    /// <summary>
    /// Вставка ключа и значения в дерево
    /// </summary>
    public void Insert(string key, string value)
    {
        TrieNode current = root;
        foreach (char c in key)
        {
            // Поиск или создание дочернего узла
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

    /// <summary>
    /// Поиск точного совпадения ключа
    /// </summary>
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

    // Реализация алгоритма поиска верхнего элемента (максимальный ключ МЕНЬШЕ заданного)

    /// <summary>
    /// Поиск наибольшего ключа, который строго меньше заданного
    /// </summary>
    public string Upper(string key)
    {
        return Upper(root, key, 0, null);
    }

    /// <summary>
    /// Рекурсивный поиск верхнего элемента
    /// </summary>
    /// <param name="node">Текущий узел</param>
    /// <param name="key">Искомый ключ</param>
    /// <param name="index">Текущий индекс в ключе</param>
    /// <param name="currentBest">Текущий наилучший результат</param>
    private string Upper(TrieNode node, string key, int index, string currentBest)
    {
        if (node == null) return currentBest;

        // Обновляем текущий лучший результат, если:
        // - узел является концом ключа
        // - мы еще не прошли весь искомый ключ (чтобы исключить полное совпадение)
        if (node.IsEndOfKey && index < key.Length)
        {
            currentBest = node.Value;
        }

        // Если дошли до конца ключа
        if (index == key.Length)
        {
            // Ищем максимальный ключ в поддереве, исключая точное совпадение
            return FindMaxKey(node, excludeKey: key);
        }

        char currentChar = key[index];

        // Рекурсивный вызов для продолжения поиска по точному совпадению символов
        if (node.Children.TryGetValue(currentChar, out TrieNode child))
        {
            string result = Upper(child, key, index + 1, currentBest);
            if (result != null) return result;
        }

        // Поиск в ветках с символами МЕНЬШЕ текущего (в обратном порядке для оптимизации)
        foreach (var kvp in node.Children.Reverse())
        {
            if (kvp.Key < currentChar)
            {
                // Поиск максимального ключа в поддереве
                string candidate = FindMaxKey(kvp.Value);
                if (candidate != null && candidate.CompareTo(key) < 0)
                {
                    return candidate;
                }
            }
        }

        return currentBest;
    }

    // Реализация алгоритма поиска нижнего элемента (минимальный ключ БОЛЬШЕ заданного)

    /// <summary>
    /// Поиск наименьшего ключа, который строго больше заданного
    /// </summary>
    public string Lower(string key)
    {
        return Lower(root, key, 0, null, key);
    }

    /// <summary>
    /// Рекурсивный поиск нижнего элемента
    /// </summary>
    /// <param name="originalKey">Оригинальный ключ для сравнения</param>
    private string Lower(TrieNode node, string key, int index, string currentBest, string originalKey)
    {
        if (node == null) return currentBest;

        // Если узел является концом ключа и его значение БОЛЬШЕ оригинального
        if (node.IsEndOfKey)
        {
            string nodeKey = node.Value;
            if (nodeKey.CompareTo(originalKey) > 0)
            {
                // Обновляем текущий лучший результат, если нашли меньшее значение
                if (currentBest == null || nodeKey.CompareTo(currentBest) < 0)
                {
                    currentBest = nodeKey;
                }
            }
        }

        if (index < key.Length)
        {
            char currentChar = key[index];

            // Продолжаем поиск по точному совпадению символов
            if (node.Children.TryGetValue(currentChar, out TrieNode child))
            {
                currentBest = Lower(child, key, index + 1, currentBest, originalKey);
            }

            // Поиск в ветках с символами БОЛЬШЕ текущего
            foreach (var kvp in node.Children)
            {
                if (kvp.Key > currentChar)
                {
                    // Поиск минимального ключа в поддереве
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
            // После полного прохода ключа ищем минимальный ключ в поддереве
            string candidate = FindMinKey(node);
            if (candidate != null && candidate.CompareTo(originalKey) > 0)
            {
                currentBest = candidate;
            }
        }

        return currentBest;
    }

    /// <summary>
    /// Поиск максимального ключа в поддереве
    /// </summary>
    /// <param name="excludeKey">Ключ, который нужно исключить из поиска</param>
    private string FindMaxKey(TrieNode node, string excludeKey = null)
    {
        if (node == null) return null;

        string maxKey = null;
        // Если узел является концом ключа и не равен исключенному ключу
        if (node.IsEndOfKey && node.Value != excludeKey)
        {
            maxKey = node.Value;
        }

        // Обход детей в ОБРАТНОМ порядке для оптимизации поиска максимума
        foreach (var child in node.Children.Reverse())
        {
            string candidate = FindMaxKey(child.Value, excludeKey);
            if (candidate != null)
            {
                // Выбираем максимальный ключ между текущим и найденным
                if (maxKey == null || candidate.CompareTo(maxKey) > 0)
                {
                    maxKey = candidate;
                }
            }
        }
        return maxKey;
    }

    /// <summary>
    /// Поиск минимального ключа в поддереве
    /// </summary>
    private string FindMinKey(TrieNode node)
    {
        if (node == null) return null;

        string minKey = null;
        if (node.IsEndOfKey)
        {
            minKey = node.Value;
        }

        // Обход детей в ПРЯМОМ порядке для оптимизации поиска минимума
        foreach (var child in node.Children)
        {
            string candidate = FindMinKey(child.Value);
            if (candidate != null)
            {
                // Выбираем минимальный ключ между текущим и найденным
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
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Вставить элемент");
            Console.WriteLine("2. Найти элемент");
            Console.WriteLine("3. Показать структуру дерева");
            Console.WriteLine("4. Найти верхний элемент");
            Console.WriteLine("5. Найти нижний элемент");
            Console.WriteLine("6. Выход");
            Console.Write("Выберите действие: ");

            switch (Console.ReadLine())
            {
                case "1": InsertElement(); break;
                case "2": SearchElement(); break;
                case "3": PrintTrie(trie.GetRoot()); break;
                case "4": FindUpper(); break;
                case "5": FindLower(); break;
                case "6": exit = true; break;
                default: Console.WriteLine("Ошибка: неверный выбор"); break;
            }
        }
    }

    static void InsertElement()
    {
        Console.Write("Введите ключ: ");
        string key = Console.ReadLine();
        trie.Insert(key, key);
        Console.WriteLine("Элемент успешно добавлен\n");
    }

    static void SearchElement()
    {
        Console.Write("Введите ключ для поиска: ");
        string result = trie.Search(Console.ReadLine());
        Console.WriteLine(result != null
            ? $"Найдено: {result}\n"
            : "Ключ не найден\n");
    }

    static void FindUpper()
    {
        Console.Write("Введите ключ: ");
        string result = trie.Upper(Console.ReadLine());
        Console.WriteLine(result != null
            ? $"Верхний элемент: {result}\n"
            : "Верхний элемент не найден\n");
    }

    static void FindLower()
    {
        Console.Write("Введите ключ: ");
        string result = trie.Lower(Console.ReadLine());
        Console.WriteLine(result != null
            ? $"Нижний элемент: {result}\n"
            : "Нижний элемент не найден\n");
    }

    /// <summary>
    /// Рекурсивная печать структуры дерева в виде дерева
    /// </summary>
    static void PrintTrie(TrieNode node, string indent = "", bool isLast = true, bool isRoot = true)
    {
        // Форматирование вывода узла
        string nodeStr = isRoot ? "Root" : node.KeyChar?.ToString() ?? "";
        if (node.IsEndOfKey) nodeStr += $" ({node.Value})";

        Console.Write(indent);
        if (!isRoot) Console.Write(isLast ? "└── " : "├── ");
        Console.WriteLine(nodeStr);

        // Рекурсивный обход детей
        string newIndent = indent + (isLast ? "    " : "│   ");
        int i = 0;
        foreach (var child in node.Children.Values)
        {
            PrintTrie(child, newIndent, i == node.Children.Count - 1, false);
            i++;
        }
    }
}