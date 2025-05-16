namespace CritBit;

public partial class Trie
{

    public string? RightBranch(int tookFromRoot)
    {
        TrieNode child = root.GetOneChild() ?? root.GetZeroChild();
        if (child == null) return null;

        string result = "";
        // Добавляем оставшиеся биты из корня
        if (tookFromRoot < root.BitString.Length)
            result += root.BitString.Substring(tookFromRoot);

        if (child.BitString.Length >= 8)
        {
            int k = Math.Min(child.BitString.Length, POSTFIX_LIMIT);
            result += child.BitString.Substring(0, k);
            return result;
        }
        else
        {

            int targetLength = result.Length + 8;
            while (result.Length < targetLength && child != null)
            {
                int needed = targetLength - result.Length;
                int take = Math.Min(child.BitString.Length, needed);
                result += child.BitString.Substring(0, take);
                // Всегда сначала выбираем правого потомка 
                child = child.GetOneChild() ?? child.GetZeroChild();
            }
        }
        return  result;
    }
}
