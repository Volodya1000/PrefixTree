namespace CritBit;

public partial class Trie
{
    public string? Lower(string key, int tookFromRoot)
    {
        string keyPrefix = key.Substring(0, 8);
        int availableBits = root.BitString.Length - tookFromRoot;
        if (availableBits >= 8)
        {
            int k = CalculateK(availableBits);
            string substring = root.BitString.Substring(tookFromRoot, k);
            return string.Compare(substring, keyPrefix) > 0 ? substring : null;
        }

        string currentPath = tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

        string? res = null;
        TrieNode? oneChild = root.GetOneChild();
        if (oneChild != null)
            res = GetFromChildLower(oneChild, currentPath, keyPrefix);
        if (res == null)
        {
            TrieNode? zeroChild = root.GetZeroChild();
            if (zeroChild != null)
                res = GetFromChildLower(zeroChild, currentPath, keyPrefix);
        }
        return res;
    }

    private string? GetFromChildLower(TrieNode child, string currentPath, string keyPrefix)
    {
        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            string substring = child.BitString.Substring(0, k);
            return string.Compare(substring, keyPrefix) < 0 ? substring : null;
        }

        string? result = null;
        LowerDFS(child, currentPath, keyPrefix, ref result);
        return result;
    }

    private bool LowerDFS(TrieNode node, string current, string key, ref string result)
    {
        int take = Math.Min(node.BitString.Length, 8 - current.Length);
        current += node.BitString.Substring(0, take);

        if (current.Length == 8 &&
            current.CompareTo(key) < 0 &&
            (result == null || current.CompareTo(result) > 0))
        {
            result = current;
            return true;
        }
        TrieNode? oneChild = node.GetOneChild();
        if (oneChild != null)
        {
            bool zeroResult = LowerDFS(oneChild, current, key, ref result);
            if (zeroResult)
                return true;
        }
        TrieNode? zeroChild = node.GetZeroChild();
        if (zeroChild != null)
            return LowerDFS(zeroChild, current, key, ref result);
        return false;
    }
}


