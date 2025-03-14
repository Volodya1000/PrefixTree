namespace Trie.SubMenu;

public class IndependentPrefixCalculator
{
    private const int X = 3;
    private const int Y = 4;
    private const int Z = 2;
    private const int ByteSize = 8;

    /// <summary>
    /// Вычисляет среднюю битовую строку по правилам:
    /// 1. Находит максимальный общий префикс кратный 8 битам
    /// 2. Обрабатывает оставшиеся части по бизнес-правилам
    /// 3. Возвращает новую валидную битовую строку
    /// </summary>
    public string CalculateAverage(string a, string b)
    {
        ValidateInput(a);
        ValidateInput(b);

        var commonPrefix = FindCommonPrefix(a, b);
        var (postA, postB) = SplitPostfixes(a, b, commonPrefix.Length);

        return commonPrefix + ProcessPostfixes(postA, postB);
    }

    /// <summary>
    /// Валидация входных данных
    /// </summary>
    private void ValidateInput(string bits)
    {
        if (string.IsNullOrEmpty(bits) || bits.Length % ByteSize != 0)
            throw new ArgumentException("Invalid bit string length");
    }

    /// <summary>
    /// Поиск общего префикса кратного размеру байта
    /// </summary>
    private string FindCommonPrefix(string a, string b)
    {
        // 1. Вычисление максимально возможной длины общего префикса
        int maxCommon = Math.Min(a.Length, b.Length) // Берём минимальную длину из двух строк
                         / ByteSize                  // Делим на размер байта (8)
                         * ByteSize;                 // Умножаем обратно, получая значение, кратное 
                                                     // 2. Поиск точной длины совпадающих битов
        int common = 0;
        while (common < maxCommon          // Пока не достигли максимума
               && a[common] == b[common])  // И биты совпадают
        {
            common++; // Увеличиваем счётчик совпадающих битов
        }

        return a[..(common / ByteSize * ByteSize)]; // Обрезаем до ближайшего кратного 8 снизу
    }

    /// <summary>
    /// Разделение строк на префикс и постфиксы
    /// </summary>
    private (string postA, string postB) SplitPostfixes(string a, string b, int prefixLength)
    {
        return (
            a.Substring(prefixLength),
            b.Substring(prefixLength)
        );
    }

    /// <summary>
    /// Основная логика обработки постфиксов
    /// </summary>
    private string ProcessPostfixes(string postA, string postB)
    {
        if (postA.Length == 0 && postB.Length == 0)
            return string.Empty;

        if (postA.Length == 0)
            return HandleEmptyFirstPostfix(postB);

        if (postB.Length == 0)
            return HandleEmptySecondPostfix(postA);

        return HandleBothPostfixes(postA, postB);
    }

    /// <summary>
    /// Обработка случая когда первый постфикс пустой
    /// </summary>
    private string HandleEmptyFirstPostfix(string postB)
    {
        string firstByte = postB.Substring(0, Math.Min(ByteSize, postB.Length));
        int value = Convert.ToInt32(firstByte.PadRight(ByteSize, '0'), 2);
        int avg = (value * X) / Y;
        return ConvertToByteString(avg) + ProcessPostfixes("", postB[ByteSize..]);
    }

    /// <summary>
    /// Обработка случая когда второй постфикс пустой
    /// </summary>
    private string HandleEmptySecondPostfix(string postA)
    {
        string firstByte = postA.Substring(0, Math.Min(ByteSize, postA.Length));
        int value = Convert.ToInt32(firstByte.PadRight(ByteSize, '0'), 2);
        int avg = (value * (Y + Z)) / Y;
        return ConvertToByteString(avg) + ProcessPostfixes(postA[ByteSize..], "");
    }

    /// <summary>
    /// Обработка случая когда оба постфикса не пустые
    /// </summary>
    private string HandleBothPostfixes(string postA, string postB)
    {
        string aByte = postA.Substring(0, Math.Min(ByteSize, postA.Length));
        string bByte = postB.Substring(0, Math.Min(ByteSize, postB.Length));

        int aVal = Convert.ToInt32(aByte.PadRight(ByteSize, '0'), 2);
        int bVal = Convert.ToInt32(bByte.PadRight(ByteSize, '0'), 2);

        int avg = aVal == bVal
            ? aVal
            : (aVal < bVal
                ? (aVal * X + bVal * (Y - X)) / Y
                : (aVal * (Y + Z) - bVal * Z) / Y);

        return ConvertToByteString(avg) + ProcessPostfixes(
            postA[ByteSize..],
            postB[ByteSize..]
        );
    }

    /// <summary>
    /// Конвертация числа в 8-битную строку
    /// </summary>
    private string ConvertToByteString(int value)
    {
        return Convert.ToString(value, 2)
            .PadLeft(ByteSize, '0')
            [..ByteSize];
    }
}