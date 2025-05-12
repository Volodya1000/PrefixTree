namespace CritBit;

public partial class Trie
{
    public string Lower(string key, int tookFromRoot)
    {
        int availableBits = root.BitString.Length - tookFromRoot;
        if (availableBits >= 8)
        {
            int k = CalculateK(availableBits);
            return root.BitString.Substring(tookFromRoot, k);
        }

        string currentPath = tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

        string keyPrefix = key.Substring(0, 8);

        string res = null;
        if (root.OneChild != null)
            res = GetFromChildLower(root.OneChild, currentPath, keyPrefix);

        if (root.ZeroChild != null)
        {
            var zeroRez = GetFromChildLower(root.ZeroChild, currentPath, keyPrefix);
            if (zeroRez != null)
                res = zeroRez;

        }
        return res;
    }


    private string? GetFromChildLower(TrieNode child, string currentPath, string keyPrefix)
    {
        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            string substring = child.BitString.Substring(0, k);
            if (string.Compare(substring, keyPrefix) < 0)
            {
                return substring;
            }
        }

        string? result = null;
        TraverseUpper(child, currentPath, keyPrefix, ref result);
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
