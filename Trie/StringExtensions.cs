namespace CritBit;

public static class StringExtensions
{
    public static string GetByte(this string str, int count = 1)
    {
        if (str == null)
            throw new ArgumentNullException(nameof(str));

        int requiredLength = 8 * count;
        if (str.Length < requiredLength)
            throw new ArgumentException($"Длина строки должна быть не менее {requiredLength} для получения {count} байт.");

        return str.Substring(0, requiredLength);
    }
}
