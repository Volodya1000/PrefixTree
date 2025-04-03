namespace CritBit;

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
