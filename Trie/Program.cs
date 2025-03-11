using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BitTrie
{
    class Program
    {
        static void Main(string[] args)
        {
            Trie trie = new Trie();
            Console.WriteLine("Bitwise Trie Console Application");

            while (true)
            {
                Console.WriteLine("\nМеню:");
                Console.WriteLine("1. Добавить слово");
                Console.WriteLine("2. Найти слово");
                Console.WriteLine("3. Показать дерево");
                Console.WriteLine("4. Найти верхнюю границу");
                Console.WriteLine("5. Найти нижнюю границу");
                Console.WriteLine("6. Выход");
                Console.Write("Выберите пункт: ");

                var choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Введите слово для добавления: ");
                        InsertCommand(trie);
                        break;
                    case "2":
                        Console.Write("Введите слово для поиска: ");
                        SearchCommand(trie);
                        break;
                    case "3":
                        trie.Display();
                        break;
                    case "4":
                        Console.Write("Введите слово для верхней границы: ");
                        UpperCommand(trie);
                        break;
                    case "5":
                        Console.Write("Введите слово для нижней границы: ");
                        LowerCommand(trie);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор!");
                        break;
                }
            }
        }

        static void InsertCommand(Trie trie)
        {
            string word = Console.ReadLine().Trim();
            string bits = BitHelper.StringToBitString(word);
            trie.Insert(bits);
            Console.WriteLine($"Добавлено: {word} ({bits})");
        }

        static void SearchCommand(Trie trie)
        {
            string word = Console.ReadLine().Trim();
            string bits = BitHelper.StringToBitString(word);
            bool found = trie.Search(bits);
            Console.WriteLine($"Результат поиска: {found}");
        }

        static void UpperCommand(Trie trie)
        {
            string word = Console.ReadLine().Trim();
            string bits = BitHelper.StringToBitString(word);
            string upper = trie.Upper(bits);
            Console.WriteLine(upper == null
                ? "Верхняя граница не найдена"
                : $"Верхняя граница: {BitHelper.BitStringToString(upper)} ({upper})");
        }

        static void LowerCommand(Trie trie)
        {
            string word = Console.ReadLine().Trim();
            string bits = BitHelper.StringToBitString(word);
            string lower = trie.Lower(bits);
            Console.WriteLine(lower == null
                ? "Нижняя граница не найдена"
                : $"Нижняя граница: {BitHelper.BitStringToString(lower)} ({lower})");
        }
    }

    /// <summary>
    /// Вспомогательный класс для работы с битовыми строками
    /// </summary>
    public static class BitHelper
    {
        /// <summary>
        /// Преобразует строку в битовую последовательность (8 бит на символ)
        /// </summary>
        /// <param name="s">Входная строка</param>
        /// <returns>Битовая строка (например "A" -> "01000001")</returns>
        public static string StringToBitString(string s)
        {
            return string.Concat(s.Select(c =>
                Convert.ToString(c, 2)
                    .PadLeft(8, '0')         // Дополняем нулями слева до 8 бит
                    .Substring(0, 8)         // Гарантируем ровно 8 бит на символ
            ));
        }

        /// <summary>
        /// Преобразует битовую строку обратно в текстовую форму
        /// </summary>
        /// <param name="bitString">Битовая строка кратная 8 битам</param>
        /// <returns>Декодированная строка или пустая строка при ошибке</returns>
        public static string BitStringToString(string bitString)
        {
            if (bitString.Length % 8 != 0)
                return "";

            List<byte> bytes = new List<byte>();
            for (int i = 0; i < bitString.Length; i += 8)
            {
                string byteStr = bitString.Substring(i, 8);
                bytes.Add(Convert.ToByte(byteStr, 2));
            }
            return Encoding.ASCII.GetString(bytes.ToArray());
        }
    }

    /// <summary>
    /// Узел префиксного дерева для хранения битовых строк
    /// </summary>
    public class TrieNode
    {
        /// <summary>
        /// Часть битовой строки, хранящаяся в этом узле
        /// </summary>
        public string BitString { get; set; }

        /// <summary>
        /// Дочерние узлы
        /// </summary>
        public List<TrieNode> Children { get; set; }

        /// <summary>
        /// Флаг окончания полного ключа в этом узле
        /// </summary>
        public bool IsEnd { get; set; }

        public TrieNode(string bitString)
        {
            BitString = bitString;
            Children = new List<TrieNode>();
            IsEnd = false;
        }
    }

    /// <summary>
    /// Префиксное дерево для работы с битовыми строками
    /// </summary>
    public class Trie
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
        /// Поиск верхней границы (наибольшая строка меньше заданной)
        /// </summary>
        public string Upper(string bitString)
        {
            var candidates = new List<string>();
            FindCandidates(root, "", bitString, candidates);

            return candidates
                .Where(b => b.Length % 8 == 0) // Только полные байты
                .OrderBy(b => b)                // Сортировка для сравнения
                .LastOrDefault(b => b.CompareTo(bitString) < 0);
        }

        /// <summary>
        /// Поиск нижней границы (наименьшая строка больше заданной)
        /// </summary>
        public string Lower(string bitString)
        {
            var candidates = new List<string>();
            FindCandidates(root, "", bitString, candidates);

            return candidates
                .Where(b => b.Length % 8 == 0) // Только полные байты
                .OrderBy(b => b)                // Сортировка для сравнения
                .FirstOrDefault(b => b.CompareTo(bitString) > 0);
        }

        /// <summary>
        /// Рекурсивный сбор всех завершенных битовых строк в дереве
        /// </summary>
        private void FindCandidates(TrieNode node, string currentPath, string queryBits, List<string> candidates)
        {
            currentPath += node.BitString;

            // Добавляем в кандидаты если узел завершен и длина кратна 8 битам
            if (node.IsEnd && currentPath.Length % 8 == 0)
            {
                candidates.Add(currentPath);
            }

            // Рекурсивный обход детей с сортировкой для упорядоченного поиска
            foreach (var child in node.Children.OrderBy(c => c.BitString))
            {
                FindCandidates(child, currentPath, queryBits, candidates);
            }
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
                Console.Write($" * [{text}]");
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
}
/*

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

*/