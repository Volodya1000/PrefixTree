using System.Text;

namespace CritBit;

/// <summary>
/// Вспомогательный класс для работы с битовыми строками
/// </summary>
public static class BitHelper
{
    /// <summary>
    /// Преобразует строку в битовую последовательность (8 бит на символ)
    /// </summary>
    /// <param name="s">Входная строка</param>
    /// <returns>Битовая строка (например "A" -> "01000001")</returns>
    public static string StringToBitString(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            //throw new ArgumentException("Строка не может быть пустой.", nameof(input));
            return "";
        }

        byte[] bytes = Encoding.GetEncoding(1251).GetBytes(input);
        StringBuilder binaryBuilder = new StringBuilder();

        foreach (byte b in bytes)
        {
            string binaryByte = Convert.ToString(b, 2).PadLeft(8, '0');
            binaryBuilder.Append(binaryByte);
        }

        return binaryBuilder.ToString();
    }

    /// <summary>
    /// Преобразует битовую строку обратно в текстовую форму
    /// </summary>
    /// <param name="bitString">Битовая строка кратная 8 битам</param>
    /// <returns>Декодированная строка или пустая строка при ошибке</returns>
    public static string BitStringToString(string binaryString)
    {
        if(string.IsNullOrEmpty(binaryString))
        {
           // throw new ArgumentException("Бинарная строка не может быть пустой.", nameof(binaryString));
            return "";
        }

        if (binaryString.Length % 8 != 0)
        {
            throw new ArgumentException("Длина бинарной строки должна быть кратна 8.", nameof(binaryString));
        }

        byte[] bytes = new byte[binaryString.Length / 8];
        for (int i = 0; i < bytes.Length; i++)
        {
            string byteStr = binaryString.Substring(i * 8, 8);
            bytes[i] = Convert.ToByte(byteStr, 2);
        }

        return Encoding.GetEncoding(1251).GetString(bytes);
    }

    /// <summary>
    /// Преобразует строку в целое число.
    /// </summary>
    /// <param name="input">Входная строка.</param>
    /// <returns>Целое число.</returns>
    public static int StringToInt(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentException("Строка не может быть пустой.", nameof(input));
        }

        if (!int.TryParse(input, out int result))
        {
            throw new FormatException("Строка не может быть преобразована в целое число.");
        }

        return result;
    }

    /// <summary>
    /// Преобразует бинарную строку в целое число.
    /// </summary>
    /// <param name="binaryString">Бинарная строка.</param>
    /// <returns>Целое число.</returns>
    public static int BinaryToInt(string binaryString)
    {
        if (string.IsNullOrEmpty(binaryString))
        {
            throw new ArgumentException("Бинарная строка не может быть пустой.", nameof(binaryString));
        }

        if (binaryString.Length % 8 != 0)
        {
            throw new ArgumentException("Длина бинарной строки должна быть кратна 8.", nameof(binaryString));
        }

        int result = 0;
        for (int i = 0; i < binaryString.Length; i += 8)
        {
            string byteStr = binaryString.Substring(i, 8);
            result = (result << 8) | Convert.ToInt32(byteStr, 2);
        }

        return result;
    }

    /// <summary>
    /// Преобразует целое число в строку.
    /// </summary>
    /// <param name="number">Целое число.</param>
    /// <returns>Строка.</returns>
    public static string FromInt(int number)
    {
        return number.ToString();
    }
    /// <summary>
    /// Преобразует целое число в бинарную строку.
    /// </summary>
    /// <param name="number">Целое число.</param>
    /// <returns>Бинарная строка.</returns>
    public static string IntToBinary(int number)
    {
        byte[] bytes = BitConverter.GetBytes(number);
        StringBuilder binaryBuilder = new StringBuilder();

        foreach (byte b in bytes)
        {
            string binaryByte = Convert.ToString(b, 2).PadLeft(8, '0');
            binaryBuilder.Append(binaryByte);
        }
        return binaryBuilder.ToString();
    }

    public static string FormatBitString(string bitString)
    {
        return $"{BitStringToString(bitString)} ({bitString})";
    }

    public static int FindCommonPrefixLength(string str1, string str2)
    {
        if (string.IsNullOrEmpty(str1) || string.IsNullOrEmpty(str2))
            return 0;

        int maxLength = Math.Min(str1.Length, str2.Length);
        for (int i = 0; i < maxLength; i++)
        {
            if (str1[i] != str2[i])
                return i - (i % 8);
        }
        return maxLength;
    }


    /// <summary>
    /// Возвращает постфикс надстроки, следующий за подстрокой.
    /// </summary>
    public static string GetPostfix(string mainString, string subString)
    {
        // Проверяем, содержится ли подстрока в надстроке
        int index = mainString.IndexOf(subString);

        // Если подстрока найдена
        if (index != -1)
        {
            // Возвращаем постфикс
            return mainString.Substring(index + subString.Length);
        }
        else
        {
            // Если подстрока не найдена, возвращаем пустую строку или сообщение об ошибке
            return string.Empty; // или throw new ArgumentException("Подстрока не найдена");
        }
    }

    public static string CutRightZeros(string bitString)
    {
        int trailingZeorCount = bitString.Reverse().TakeWhile(c=>c == '0').Count();

        int removeCount = trailingZeorCount - (trailingZeorCount % 8);

        if (removeCount > 0)
            return bitString.Remove(bitString.Length-removeCount, removeCount);
        else return bitString;
    }
}
