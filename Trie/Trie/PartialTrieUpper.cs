namespace CritBit;

public partial class Trie
{
    public string? Upper(string key, int tookFromRoot)
    {
        int availableBits = root.BitString.Length - tookFromRoot;
        if (availableBits >= 8)
        {
            int k = CalculateK(availableBits);
            return root.BitString.Substring(tookFromRoot, k);
        }

        // Инициализируем текущий путь, оставшиеся биты корня после tookFromRoot
        string currentPath = tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

        TrieNode child = root.OneChild ?? root.ZeroChild;

        if (child == null) return null;

        if (child.BitString.Length >= 8)
            return child.BitString.Substring(0, CalculateK(child.BitString.Length));

        string result = null;
        TraverseUpper(child, currentPath, key.Substring(0, 8), ref result);
        return result;
    }

    private void TraverseUpper(TrieNode node, string current, string key, ref string result)
    {
        int take = Math.Min(node.BitString.Length, 8 - current.Length);
        current += node.BitString.Substring(0, take);

        if (current.Length == 8)
        {
            if (string.Compare(current, key) > 0)
            {
                if (result == null || string.Compare(current, result) < 0)
                    result = current;
            }
            return;
        }

        if (node.OneChild != null) TraverseUpper(node.OneChild, current, key, ref result);
        if (node.ZeroChild != null) TraverseUpper(node.ZeroChild, current, key, ref result);
    }
}
