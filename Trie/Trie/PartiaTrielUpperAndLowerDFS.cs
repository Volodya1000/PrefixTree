using static CritBit.BitHelper;
namespace CritBit;

public partial class Trie
{
    public string LowerApril(string key, int tookFromRoot)
        => FindBoundary(key, tookFromRoot, isUpper: false);

    public string UpperApril(string key, int tookFromRoot)
        => FindBoundary(key, tookFromRoot, isUpper: true);

    private string FindBoundary(string key, int tookFromRoot, bool isUpper)
    {
        // Проверка возможности взять биты из корня
        string rootResult = GetRootCandidate(tookFromRoot);
        if (rootResult != null) return rootResult;

        // Формирование начального пути
        string currentPath = GetInitialPath(tookFromRoot);

        // Выбор первого потомка (ZeroChild для Lower, OneChild для Upper)
        TrieNode child = GetPriorityChild(root, isUpper);
        if (child == null) return null;

        // Проверка достаточно ли бит у потомка
        string childResult = GetChildCandidate(child);
        if (childResult != null) return childResult;

        // Рекурсивный поиск
        string result = "";
        Traverse(child, currentPath, key.Substring(0, 8), ref result, isUpper);
        return result;
    }

    // Общие вспомогательные методы
    private string GetRootCandidate(int tookFromRoot)
    {
        int availableBits = root.BitString.Length - tookFromRoot;
        if (availableBits >= 8)
            return root.BitString.Substring(tookFromRoot, CalculateK(availableBits));
        return null;
    }

    private string GetInitialPath(int tookFromRoot)
        => tookFromRoot < root.BitString.Length
            ? root.BitString.Substring(tookFromRoot)
            : "";

    private TrieNode GetPriorityChild(TrieNode node, bool isUpper)
        => isUpper
            ? node.OneChild ?? node.ZeroChild
            : node.ZeroChild ?? node.OneChild;

    private string GetChildCandidate(TrieNode child)
        => child.BitString.Length >= 8
            ? child.BitString.Substring(0, CalculateK(child.BitString.Length))
            : null;

    private void Traverse(TrieNode node, string current, string key, ref string result, bool isUpper)
    {
        // Добавление бит с учетом лимита в 8
        int take = Math.Min(node.BitString.Length, 8 - current.Length);
        current += node.BitString.Substring(0, take);

        // Проверка завершения
        if (current.Length == 8)
        {
            UpdateResult(current, key, ref result, isUpper);
            return;
        }

        // Рекурсивный обход потомков в заданном порядке
        var (first, second) = isUpper
            ? (node.OneChild, node.ZeroChild)
            : (node.ZeroChild, node.OneChild);

        if (first != null) Traverse(first, current, key, ref result, isUpper);
        if (second != null) Traverse(second, current, key, ref result, isUpper);
    }

    private void UpdateResult(string candidate, string key, ref string result, bool isUpper)
    {
        int comparisonWithKey = string.Compare(candidate, key);
        bool isCandidateValid = isUpper ? comparisonWithKey > 0 : comparisonWithKey < 0;

        if (!isCandidateValid) return;

        if (string.IsNullOrEmpty(result))
        {
            result = candidate;
            return;
        }

        int comparisonWithResult = string.Compare(candidate, result);
        bool shouldUpdate = isUpper ? comparisonWithResult < 0 : comparisonWithResult > 0;

        if (shouldUpdate)
            result = candidate;
    }
}
