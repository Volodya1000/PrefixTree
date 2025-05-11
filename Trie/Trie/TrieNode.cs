namespace CritBit;

/// <summary>
/// Узел бинарного префиксного дерева для хранения битовых строк
/// </summary>
public class TrieNode
{
    /// <summary>
    /// Часть битовой строки, хранящаяся в этом узле
    /// </summary>
    public string BitString { get; set; }

    /// <summary>
    /// Левый потомок (бит = '0')
    /// </summary>
    public TrieNode Left { get; set; }

    /// <summary>
    /// Правый потомок (бит = '1')
    /// </summary>
    public TrieNode Right { get; set; }

    /// <summary>
    /// Флаг окончания полного ключа в этом узле
    /// </summary>
    public bool IsEnd { get; set; }

    public TrieNode(string bitString)
    {
        BitString = bitString;
        Left = null;
        Right = null;
        IsEnd = false;
    }
}
