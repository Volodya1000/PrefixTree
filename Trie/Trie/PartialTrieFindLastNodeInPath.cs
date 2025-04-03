namespace CritBit;

public partial class Trie
{
    //public (TrieNode, int nodeStoreCount, int busyInPathCount) FindLastNodeInPath(string bitString)
    //{
    //    if (bitString.Length % 8 != 0)
    //        throw new ArgumentException("Bit string length must be multiple of 8");

    //    return FindLastNodeRecursive(root, bitString);
    //}

    //private (TrieNode, int, int) FindLastNodeRecursive(TrieNode node, string remainingBits)
    //{
    //    if (remainingBits.StartsWith(node.BitString))
    //    {
    //        string newRemaining = remainingBits.Substring(node.BitString.Length);
    //        if (newRemaining == "")
    //            return (node, node.BitString.Length, node.BitString.Length);

    //        foreach (var child in node.Children)
    //        {
    //            var result = FindLastNodeRecursive(child, newRemaining);
    //            if (result.Item1 != null)
    //                return (result.Item1, result.Item1.BitString.Length + node.BitString.Length, result.Item3 + node.BitString.Length);
    //        }
    //    }
    //    // Проверяем частичное совпадение (оставшиеся биты - префикс данных узла)
    //    else if (node.BitString.StartsWith(remainingBits))
    //    {
    //        return (node, node.BitString.Length, remainingBits.Length);
    //    }

    //    return (null, 0, 0);
    //}

    public (TrieNode, int, int) FindLastNodeInPath(string bitString)
    {
        TrieNode currentNode = root;
        string remaining = bitString;

        while (remaining.Length > 0)
        {
            bool foundChild = false;
            foreach (var child in currentNode.Children.ToList())
            {
                if (remaining.StartsWith(child.BitString))
                {
                    remaining = remaining.Substring(child.BitString.Length);
                    currentNode = child;
                    foundChild = true;
                    break;
                }
            }

            if (!foundChild)
            {
                foreach (var child in currentNode.Children)
                {
                    if (child.BitString.StartsWith(remaining))
                    {
                        return (child, child.BitString.Length, remaining.Length);
                    }
                }
                throw new InvalidOperationException("Path not found despite CheckSubstringExists being true.");
            }
        }

        return (currentNode, currentNode.BitString.Length, currentNode.BitString.Length);
    }
}
