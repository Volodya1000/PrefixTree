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
    /// Дочерний узел для бита 0
    /// </summary>
    public TrieNode ZeroChild { get; set; }

    /// <summary>
    /// Дочерний узел для бита 1
    /// </summary>
    public TrieNode OneChild { get; set; }

    /// <summary>
    /// Флаг окончания полного ключа в этом узле
    /// </summary>
    public bool IsEnd { get; set; }

    public TrieNode(string bitString)
    {
        BitString = bitString;
        ZeroChild = null;
        OneChild = null;
        IsEnd = false;
    }
}
