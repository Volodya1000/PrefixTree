namespace CritBit;

public partial class Trie
{
    public string? LeftBranch(int tookFromRoot)
    {
        string result = "";

        // Добавляем оставшиеся биты из корня
        if (tookFromRoot < root.BitString.Length)
            result += root.BitString.Substring(tookFromRoot);

        TrieNode child = root.ZeroChild ?? root.OneChild;
        if (child == null) return null;

        if(child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            result += child.BitString.Substring(0, k);
            return result;
        }

        int targetLength = result.Length + 8;

        while (result.Length < targetLength && child != null)
        {
            int needed = targetLength - result.Length;
            int take = Math.Min(child.BitString.Length, needed);
            result += child.BitString.Substring(0, take);

            // Всегда выбираем "левого" потомка (ZeroChild -> OneChild)
            child = child.ZeroChild ?? child.OneChild;
        }

        return result.Length > targetLength
               ? result.Substring(0, targetLength)
               : result;
    }
}
