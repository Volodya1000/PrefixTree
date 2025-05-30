﻿namespace CritBit;

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

        string ? res=null;
        TrieNode? zeroChild = root.GetZeroChild();
        if (zeroChild != null)
            res = GetFromChildUpper(zeroChild, currentPath, keyPrefix);
        if (res == null)
        {
            TrieNode? oneChild = root.GetOneChild();
            if(oneChild!=null)
                res = GetFromChildUpper(oneChild, currentPath, keyPrefix);
        }
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
        TrieNode? zeroChild = node.GetZeroChild();
        if (zeroChild != null)
        {
            bool zeroResult = UpperDFS(zeroChild, current, key, ref result);
            if (zeroResult)
                return true;
        }
        TrieNode? oneChild = node.GetOneChild();
        if (oneChild != null)
            return UpperDFS(oneChild, current, key, ref result);
        return false;
    }
}
