namespace CritBit;

public partial class Trie
{
    public string LeftBranch(int tookFromRoot)
    {
        TrieNode current = root;
        //    = FindNode(bitString);
        //if (current == null) return null;

        string result = "";

        if (tookFromRoot != root.BitString.Length)
            result += root.BitString.Substring(tookFromRoot);

        //Получаем первого наследника
        TrieNode firstChild = current.Children.FirstOrDefault(c => c.BitString.StartsWith("0"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("1"));

        if (firstChild == null) return null;

        int firstChildLength = firstChild.BitString.Length;

        //Смотрим сколько бит в первом наследнике 
        if (firstChildLength >= 8)
        {
            int k = CalculateK(firstChildLength);
            result += firstChild.BitString.Substring(0, k);
            return result;
        }

        while (result.Length < 8)
        {
            TrieNode nextChild = current.Children
           .FirstOrDefault(c => c.BitString.StartsWith("0"))
           ?? current.Children.FirstOrDefault(c => c.BitString.StartsWith("1"));

            if (nextChild == null) break;

            int available = nextChild.BitString.Length;
            int needed = 8 - result.Length;
            int take = Math.Min(available, needed);

            result += nextChild.BitString.Substring(0, take);
            current = nextChild;
        }
        return result;
    }

}
