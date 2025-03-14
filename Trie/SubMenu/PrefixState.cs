namespace Trie.SubMenu;

/// <summary>
/// Отвечает за хранение состояния подменю и валидацию данных
/// </summary>
public class PrefixState 
{
    public string Current { get; private set; } = string.Empty;
    public string UpperBound { get; private set; } = string.Empty;
    public string LowerBound { get; private set; } = string.Empty;

    /// <summary>
    /// Обновляет состояние с проверкой валидности префиксов
    /// </summary>
    public void UpdateState(string current, string upper, string lower)
    {
        ValidatePrefix(current);
        ValidatePrefix(upper);
        ValidatePrefix(lower);

        Current = current;
        UpperBound = upper;
        LowerBound = lower;
    }

    private void ValidatePrefix(string prefix)
    {
        if (prefix.Length % 8 != 0)
            throw new ArgumentException("Invalid prefix length");
    }
}
