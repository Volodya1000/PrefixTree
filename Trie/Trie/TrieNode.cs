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
    private TrieNode ZeroChild;

    /// <summary>
    /// Дочерний узел для бита 1
    /// </summary>
    private TrieNode OneChild;


    public TrieNode ?GetOneChild() => OneChild;
    public void SetOneChild(TrieNode child) => OneChild = child;

    public TrieNode ?GetZeroChild() => ZeroChild;
    public void SetZeroChild(TrieNode child) => ZeroChild = child;

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
