using System.Text;

namespace CritBit;

public partial class Trie
{
    public string? RightBranch(string bitString)
    {
        TrieNode current = FindNode(bitString);
        if (current == null) return null;

        string result = bitString;

        //Получаем первого наследника
        TrieNode firstChild= current.Children.FirstOrDefault(c => c.BitString.StartsWith("1"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("0"));

        if (firstChild == null) return null;

        int firstChildLength = firstChild.BitString.Length;

        //Смотрим сколько бит в первом наследнике 
        if (firstChildLength >= 8)
        {
            int k = CalculateK(firstChildLength);
            result += firstChild.BitString.Substring(0,k);
            return result;
        }

        int targetResultLength = bitString.Length + 8;

        while (result.Length < targetResultLength)
        {
            TrieNode nextChild = current.Children
           .FirstOrDefault(c => c.BitString.StartsWith("1"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("0"));

            if (nextChild == null) break;

            int available = nextChild.BitString.Length;
            int needed = targetResultLength - result.Length;
            int take = Math.Min(available, needed);

            result+=nextChild.BitString.Substring(0, take);
            current = nextChild;
        }
        return result;
    }

    private int CalculateK(int x)
    {
        int k = (int)Math.Floor(x / 8.0) * 8;
        return Math.Clamp(k, 8, 96);
    }

    private TrieNode FindNode(string bitString)
    {
        return FindNodeRecursive(root, bitString);
    }

    private TrieNode FindNodeRecursive(TrieNode node, string remainingBits)
    {
        if (!remainingBits.StartsWith(node.BitString)) return null;

        string newRemaining = remainingBits.Substring(node.BitString.Length);
        if (newRemaining == "") return node;

        foreach (var child in node.Children)
        {
            var found = FindNodeRecursive(child, newRemaining);
            if (found != null) return found;
        }
        return null;
    }

}
