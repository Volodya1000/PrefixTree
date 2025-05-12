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

        string currentPath = tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

        string keyPrefix = key.Substring(0, 8);

        string res=null;
        if (root.OneChild != null)
             res = GetFromChilUpper(root.OneChild, currentPath, keyPrefix);

        if (root.ZeroChild != null)
        {
            var zeroRez= GetFromChilUpper(root.ZeroChild, currentPath, keyPrefix);
            if (zeroRez != null)
                res = zeroRez;

        }
        return res;
    }

    private string? GetFromChilUpper(TrieNode child, string currentPath, string keyPrefix)
    {
        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            string substring = child.BitString.Substring(0, k);
            if (string.Compare(substring, keyPrefix) > 0)
                return substring;
        }

        string? result = null;
        TraverseUpper(child, currentPath, keyPrefix, ref result);
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
