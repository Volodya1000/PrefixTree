using System.Text;

namespace Trie;

public class BinaryStringIncrementer
{
    public static string GetNextSeniorString(string input)
    {
        // Проверка валидности входной строки
        if (input.Length % 8 != 0)
            throw new ArgumentException("Длина входной строки должна быть кратна 8 битам");

        // Разбиваем битовую строку на байты
        List<byte> bytes = new List<byte>();
        for (int i = 0; i < input.Length; i += 8)
        {
            string byteStr = input.Substring(i, 8);
            bytes.Add(Convert.ToByte(byteStr, 2));
        }

        // Конвертируем байты в цифры алфавита (A=1, B=2... Z=26)
        List<int> digits = new List<int>();
        foreach (byte b in bytes)
        {
            char c = (char)b;
            if (c < 'A' || c > 'Z')
                throw new ArgumentException("Строка содержит недопустимые символы");
            digits.Add(c - 'A' + 1);
        }

        // Увеличиваем число с обработкой переносов
        int carry = 1;
        for (int i = digits.Count - 1; i >= 0; i--)
        {
            digits[i] += carry;
            carry = digits[i] > 26 ? 1 : 0;
            digits[i] = digits[i] > 26 ? digits[i] - 26 : digits[i];
        }

        // Добавляем новый разряд при необходимости
        if (carry == 1)
            digits.Insert(0, 1);

        // Конвертируем обратно в битовую строку
        StringBuilder result = new StringBuilder();
        foreach (int digit in digits)
        {
            char c = (char)('A' + digit - 1);
            result.Append(Convert.ToString(c, 2).PadLeft(8, '0'));
        }

        return result.ToString();
    }
}

/*
Проверка формата: Убеждаемся, что длина входной строки кратна 8 битам.

Разбор байтов: Разбиваем битовую строку на 8-битные сегменты и конвертируем их в байты.

Конвертация в цифры: Переводим каждый символ в числовое представление (A=1, B=2... Z=26).

Инкремент с переносом: Увеличиваем число, обрабатывая переносы как в обычной арифметике, но в 26-ричной системе.

Расширение результата: При переполнении добавляем новый старший разряд (например, Z → AA).

Обратная конвертация: Преобразуем результат обратно в битовую строку с сохранением 8-битного формата для каждого символа.
 */
