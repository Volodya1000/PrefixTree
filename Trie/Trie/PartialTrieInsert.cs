namespace CritBit;

public partial class Trie
{
    /// <summary>
    /// Вставка битовой строки в дерево
    /// </summary>
    public void Insert(string bitString) => InsertRecursive(root, bitString);

    /// <summary>
    /// Рекурсивная вставка с автоматическим разделением узлов
    /// </summary>
    private void InsertRecursive(TrieNode node, string remainingBits)
    {
        if (remainingBits.Length == 0) return;

        char currentBit = remainingBits[0];
        TrieNode child = (currentBit == '0') ? node.ZeroChild : node.OneChild; 

        if (child != null)
        {
            int commonLength = FindCommonPrefix(child.BitString, remainingBits);
            if (commonLength == child.BitString.Length)
            {
                InsertRecursive(child, remainingBits.Substring(commonLength));
            }
            else
            {
                SplitChildNode(node, child, commonLength, remainingBits);
            }
        }
        else
        {
            TrieNode newNode = new TrieNode(remainingBits);
            newNode.IsEnd = true;
            if (currentBit == '0')
                node.ZeroChild = newNode;
            else
                node.OneChild = newNode;
        }
    }

    private void SplitChildNode(TrieNode parent, TrieNode child, int commonLen, string remaining)
    {
        string common = child.BitString.Substring(0, commonLen);
        string childRemaining = child.BitString.Substring(commonLen);
        string newRemaining = remaining.Substring(commonLen);

        TrieNode commonNode = new TrieNode(common);
        if (common[0] == '0')
            parent.ZeroChild = commonNode;
        else
            parent.OneChild = commonNode;

        child.BitString = childRemaining;
        if (childRemaining[0] == '0')
            commonNode.ZeroChild = child;
        else
            commonNode.OneChild = child;

        if (newRemaining.Length > 0)
        {
            TrieNode newNode = new TrieNode(newRemaining);
            newNode.IsEnd = true;
            if (newRemaining[0] == '0')
                commonNode.ZeroChild = newNode;
            else
                commonNode.OneChild = newNode;
        }
        else
        {
            commonNode.IsEnd = true;
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
}
