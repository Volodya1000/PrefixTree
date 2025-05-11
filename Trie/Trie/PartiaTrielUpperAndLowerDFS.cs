namespace CritBit;

public partial class Trie
{

    //17.04
    public string LowerApril(string key, int tookFromRoot)
    {
        string result = "";
        string keyFirst8 = key.Substring(0, 8);
        int canTakeFromRoot = root.BitString.Length - tookFromRoot;

        // Если можно взять достаточно бит из корня
        if (canTakeFromRoot >= 8)
        {
            int k = CalculateK(canTakeFromRoot);
            return root.BitString.Substring(tookFromRoot, k);
        }

        string currentPath = "";
        if (tookFromRoot < root.BitString.Length)
            currentPath = root.BitString.Substring(tookFromRoot);

        // Выбираем сначала ZeroChild, затем OneChild
        TrieNode child = root.ZeroChild ?? root.OneChild;
        if (child == null) return null;

        // Если у потомка достаточно бит
        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            return child.BitString.Substring(0, k);
        }

        // Рекурсивный поиск
        LowerRecursive(child, currentPath, keyFirst8, ref result);
        return result;
    }

    private void LowerRecursive(TrieNode node, string currentString, string key, ref string result)
    {
        // Добавляем биты из текущего узла (но не более 8)
        int take = Math.Min(node.BitString.Length, 8 - currentString.Length);
        currentString += node.BitString.Substring(0, take);

        // Проверка достижения 8 бит
        if (currentString.Length == 8)
        {
            if (string.Compare(currentString, key) < 0)
            {
                if (result == null || string.Compare(currentString, result) > 0)
                    result = currentString;
            }
            return;
        }

        // Рекурсивный обход: сначала ZeroChild, затем OneChild
        if (node.ZeroChild != null)
            LowerRecursive(node.ZeroChild, currentString, key, ref result);

        if (node.OneChild != null)
            LowerRecursive(node.OneChild, currentString, key, ref result);
    }

    public string UpperApril(string key, int tookFromRoot)
    {
        string result = "";
        string keyFirst8 = key.Substring(0, 8);
        int canTakeFromRoot = root.BitString.Length - tookFromRoot;

        if (canTakeFromRoot >= 8)
        {
            int k = CalculateK(canTakeFromRoot);
            return root.BitString.Substring(tookFromRoot, k);
        }

        string currentPath = "";
        if (tookFromRoot < root.BitString.Length)
            currentPath = root.BitString.Substring(tookFromRoot);

        // Выбираем сначала OneChild, затем ZeroChild
        TrieNode child = root.OneChild ?? root.ZeroChild;
        if (child == null) return null;

        if (child.BitString.Length >= 8)
        {
            int k = CalculateK(child.BitString.Length);
            return child.BitString.Substring(0, k);
        }

        UpperRecursive(child, currentPath, keyFirst8, ref result);
        return result;
    }

    private void UpperRecursive(TrieNode node, string currentString, string key, ref string result)
    {
        int take = Math.Min(node.BitString.Length, 8 - currentString.Length);
        currentString += node.BitString.Substring(0, take);

        if (currentString.Length == 8)
        {
            if (string.Compare(currentString, key) > 0)
            {
                if (result == null || string.Compare(currentString, result) < 0)
                    result = currentString;
            }
            return;
        }

        // Рекурсивный обход: сначала OneChild, затем ZeroChild
        if (node.OneChild != null)
            UpperRecursive(node.OneChild, currentString, key, ref result);

        if (node.ZeroChild != null)
            UpperRecursive(node.ZeroChild, currentString, key, ref result);
    }

    private int CalculateK(int length)
    {
        int k = (length / 8) * 8;
        return Math.Clamp(k, 8, 96);
    }


}
