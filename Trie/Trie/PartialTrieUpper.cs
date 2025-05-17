using System.Reflection.Metadata.Ecma335;

namespace CritBit;

public partial class Trie
{
    public string? Upper(string key, int tookFromRoot)
    {
        string keyPrefix = !String.IsNullOrEmpty(key) ?key.GetByte(): key;
        int availableBits = root.BitString.Length - tookFromRoot;
        if (availableBits >= 8)
        {
            int k = CalculateK(availableBits);
            string substring = root.BitString.Substring(tookFromRoot, k);
            return string.Compare(substring, keyPrefix) > 0 ? substring: null;
        }

        string currentPath = tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

        string res=null;
        if (root.GetZeroChild() != null)
            res = GetFromChildUpper(root.GetZeroChild(), currentPath, keyPrefix);
        if (res == null&& root.GetOneChild()!=null)
            res = GetFromChildUpper(root.GetOneChild(), currentPath, keyPrefix);
        return res;
    }

    private string? GetFromChildUpper(TrieNode child, string currentPath, string keyPrefix)
    {
        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length); 
            string substring = child.BitString.Substring(0, k);
            return string.Compare(substring, keyPrefix) > 0 ? substring : null;
        }

        string? result = null;
        UpperDFS(child, currentPath, keyPrefix, ref result);
        return result;
    }

    private bool UpperDFS(TrieNode node, string current, string key, ref string result)
    {
        int take = Math.Min(node.BitString.Length, 8 - current.Length);
        current += node.BitString.Substring(0, take);

        if (current.Length == 8 &&
            current.CompareTo(key) > 0 &&
            (result == null || current.CompareTo(result) < 0))
        {
            result = current;
            return true;
        }
        if (node.GetZeroChild() != null)
        {
            bool zeroResult = UpperDFS(node.GetZeroChild(), current, key, ref result);
            if (zeroResult)
                return true;
        }
        if(node.GetOneChild() != null)
            return UpperDFS(node.GetOneChild(), current, key, ref result);
        return false;
    }
}
