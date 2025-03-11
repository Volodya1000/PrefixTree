using System.Text;

namespace Trie;

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
    public static string StringToBitString(string s)
    {
        return string.Concat(s.Select(c =>
            Convert.ToString(c, 2)
                .PadLeft(8, '0')         // Дополняем нулями слева до 8 бит
                .Substring(0, 8)         // Гарантируем ровно 8 бит на символ
        ));
    }

    /// <summary>
    /// Преобразует битовую строку обратно в текстовую форму
    /// </summary>
    /// <param name="bitString">Битовая строка кратная 8 битам</param>
    /// <returns>Декодированная строка или пустая строка при ошибке</returns>
    public static string BitStringToString(string bitString)
    {
        if (bitString.Length % 8 != 0)
            return "";

        List<byte> bytes = new List<byte>();
        for (int i = 0; i < bitString.Length; i += 8)
        {
            string byteStr = bitString.Substring(i, 8);
            bytes.Add(Convert.ToByte(byteStr, 2));
        }
        return Encoding.ASCII.GetString(bytes.ToArray());
    }
}
