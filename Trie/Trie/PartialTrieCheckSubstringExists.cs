namespace CritBit;

public partial class Trie
{
    public bool CheckSubstringExists(string bitString)
    {
        if(String.IsNullOrEmpty(bitString))
            return false;//
        if (bitString.Length % 8 != 0)
            throw new ArgumentException("Bit string length must be multiple of 8");

        return CheckSubstringExistsRecursive(root, bitString);
    }

    private bool CheckSubstringExistsRecursive(TrieNode node, string remainingBits)
    {
        if (remainingBits.StartsWith(node.BitString))
        {
            string newRemaining = remainingBits.Substring(node.BitString.Length);
            if (newRemaining.Length == 0) return true;
           
            return (node.ZeroChild != null && CheckSubstringExistsRecursive(node.ZeroChild, newRemaining)) ||
                   (node.OneChild != null && CheckSubstringExistsRecursive(node.OneChild, newRemaining));
        }
        else
        {
            return node.BitString.StartsWith(remainingBits);
        }
    }

}