namespace CritBit;

public partial class Trie
{
    public string Lower(string key, int tookFromRoot)
    {
        // Проверяем, есть ли в корне подходящий кандидат
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

        TrieNode child = root.ZeroChild ?? root.OneChild;

        if (child == null) return null;

        if (child.BitString.Length >= 8)
            return child.BitString.Substring(0, CalculateK(child.BitString.Length));

        string result = null;
        LowerDFS(child, currentPath, key.Substring(0, 8), ref result);
        return result;
    }

    private void LowerDFS(TrieNode node, string current, string key, ref string result)
    {
        int take = Math.Min(node.BitString.Length, 8 - current.Length);
        current += node.BitString.Substring(0, take);

        if (current.Length == 8)
        {
            if (string.Compare(current, key) < 0)
            {
                if (result == null || string.Compare(current, result) > 0)
                    result = current;
            }
            return;
        }

        if (node.ZeroChild != null) LowerDFS(node.ZeroChild, current, key, ref result);
        if (node.OneChild != null) LowerDFS(node.OneChild, current, key, ref result);
    }

}
