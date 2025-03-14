using static System.Console;
namespace Trie;

public static class MiddlePrefixСomputer
{
    // Константы x, y и z
    private const int x = 3;
    private const int y = 4;
    private const int z = 2;

    public static string ComputeMiddlePrefix(string bitString1, string bitString2)
    {
        WriteLine($"=== Отладка ===");

        // Определение старшей и младшей строки по лексикографическому порядку
        string senior, junior;
        if (string.Compare(bitString1, bitString2) > 0)
        {
            senior = bitString1;
            junior = bitString2;
        }
        else
        {
            senior = bitString2;
            junior = bitString1;
        }
        WriteLine($"Старшая: {BitHelper.BitStringToString(senior)} ({senior})");
        WriteLine($"Младшая: {BitHelper.BitStringToString(junior)} ({junior})");


        // Вычисление общего префикса
        int commonPrefixLength = FindCommonPrefixLength(senior, junior);
        string commonPrefix = senior.Substring(0, commonPrefixLength);


        WriteLine($"Общий префикс: {BitHelper.BitStringToString(commonPrefix)} ({commonPrefix})");

        // Постфиксы
        string seniorPostfix = senior.Substring(commonPrefixLength);

        WriteLine($"Постфикс старшей: {BitHelper.BitStringToString(seniorPostfix)} ({seniorPostfix})");

        string juniorPostfix = junior.Substring(commonPrefixLength);

        WriteLine($"Постфикс младшей: {BitHelper.BitStringToString(juniorPostfix)} ({juniorPostfix})");

        if (string.IsNullOrEmpty(seniorPostfix) && string.IsNullOrEmpty(juniorPostfix))
        {
            WriteLine("  Оба постфикса пустые, возвращаем общий префикс.");
            return commonPrefix; // Оба постфикса пустые
        }
        else if (string.IsNullOrEmpty(juniorPostfix))
        {
            // Младший постфикс пустой
            WriteLine("\nОБРАБОТКА СТАРШЕГО ПОСТФИКСА:");
            string higherFirstByte = seniorPostfix.Substring(0, 8);
            int higherValue = Convert.ToInt32(higherFirstByte, 2);
            WriteLine($"Первый байт: {higherFirstByte} ({higherValue} dec)");

            int medianValue = (int)Math.Floor((double)higherValue * x / y);
            WriteLine($"Расчёт: {higherValue} * {x}/{y} = {medianValue}");

            byte correctedByte = EnsureAsciiLetter((byte)medianValue);
            WriteLine($"Корректировка: {medianValue} → {correctedByte} ({BitHelper.BitStringToString(Convert.ToString(correctedByte, 2).PadLeft(8, '0'))})");

            return commonPrefix + Convert.ToString(correctedByte, 2).PadLeft(8, '0');
        }
        else if (string.IsNullOrEmpty(seniorPostfix))
        {
            // Старший постфикс пустой
            WriteLine("\nОБРАБОТКА МЛАДШЕГО ПОСТФИКСА:");
            string lowerFirstByte = juniorPostfix.Substring(0, 8);
            int lowerValue = Convert.ToInt32(lowerFirstByte, 2);
            double factor = 1 + (double)z / y;
            WriteLine($"Фактор 1+z/y = {factor}");
            int medianValue = (int)Math.Floor(lowerValue * factor);
            WriteLine($"Расчёт: {lowerValue} * {factor} = {medianValue}");

            byte correctedByte = EnsureAsciiLetter((byte)medianValue);
            WriteLine($"Корректировка: {medianValue} → {correctedByte} ({BitHelper.BitStringToString(Convert.ToString(correctedByte, 2).PadLeft(8, '0'))})");

            return commonPrefix + Convert.ToString(correctedByte, 2).PadLeft(8, '0');
        }
        else
        {
            // Оба постфикса не пустые
            WriteLine($"  Оба постфикса непустые, берём возвращаем конкатанацию префикса и среднего между постфиксами ");

            char firstSeniorChar = GetCharFromBits(seniorPostfix.Substring(0, 8));
            char firstJuniorChar = GetCharFromBits(juniorPostfix.Substring(0, 8));
            char averageChar = (char)((firstSeniorChar + firstJuniorChar) / 2);

            string average = GetBitsFromChar(averageChar);
            WriteLine($"Cреднее между постфиксами: {BitHelper.BitStringToString(average)} ({average})");
            return commonPrefix + average;
        }
    }

    private static int FindCommonPrefixLength(string str1, string str2)
    {
        int length = Math.Min(str1.Length, str2.Length);
        for (int i = 0; i < length; i++)
        {
            if (str1[i] != str2[i])
            {
                return i - (i % 8); // Возвращаем длину кратную 8 битам
            }
        }
        return length - (length % 8); // Полный общий префикс кратный 8 битам
    }

    private static char GetCharFromBits(string bits)
    {
        byte value = Convert.ToByte(bits, 2);
        return (char)value;
    }

    private static string GetBitsFromChar(char c)
    {
        byte value = (byte)c;
        return Convert.ToString(value, 2).PadLeft(8, '0');
    }

    private static byte EnsureAsciiLetter(byte value)
    {
        // Проверка на A-Z или a-z
        if ((value >= 0x41 && value <= 0x5A) || (value >= 0x61 && value <= 0x7A))
        {
            return value;
        }

        // Попытка преобразовать в верхний регистр
        byte upper = (byte)(value & 0xDF);
        if (upper >= 0x41 && upper <= 0x5A)
        {
            return upper;
        }

        // Замена небуквенных символов на 'A'
        return 0x41;
    }
}