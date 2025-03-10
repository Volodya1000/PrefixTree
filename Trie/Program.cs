// See https:/using System;
using System.Collections.Generic;

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
            Console.WriteLine("4. Выход");
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
        Console.Write("Введите значение: ");
        string value = Console.ReadLine();
        trie.Insert(key, value);
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