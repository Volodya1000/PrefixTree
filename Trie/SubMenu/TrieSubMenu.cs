using System.Numerics;
using static System.Console;

namespace Trie;
/// <summary>
/// Класс, реализующий подменю вычисления средней строки
/// </summary>
public static class TrieSubMenu
{
    // Константы для вычисления средней строки
    const int X = 3;
    const int Y = 4;
    const int Z = 1;

    /// <summary>
    /// Запуск подменю для вычисления средней строки
    /// </summary>
    public static void Run(Trie trie)
    {
        WriteLine("\n--- Подменю вычисления средней строки ---");
        // Запрос текущего префикса (должен быть кратен 8 битам)
        string current  = GetValidPrefix();

        // Вычисляем "старшую на 1" строку и кандидатов для пунктов 2 и 5
        string nextStr = BinaryStringIncrementer.GetNextSeniorString(current);
        string candidate2 = trie.Lower(nextStr); // наименьшая среди тех, что больше nextStr
        string candidate5 = trie.Upper(nextStr); // наибольшая среди тех, что меньше nextStr

        while (true)
        {
            WriteLine("\nПодменю:");
            WriteLine($"1. Текущий префикс: {BitHelper.BitStringToString(current)} ({current})");
            WriteLine($"2. [Вверх] {(candidate2 != null ? BitHelper.BitStringToString(candidate2) + " (" + candidate2 + ")" : "нет кандидата")}");
            WriteLine("3. ... (изменить пункт 5)");
            WriteLine("4. ... (изменить пункт 2)");
            WriteLine($"5. [Вниз] {(candidate5 != null ? BitHelper.BitStringToString(candidate5) + " (" + candidate5 + ")" : "нет кандидата")}");
            WriteLine("6. Главное меню");
            WriteLine("====== Для тестирования ==== ");
            WriteLine("7. Строка старше на 1");
            WriteLine("8. Среднее для двух строк");
            Write("Выберите пункт подменю: ");

            var choice = ReadLine();
            switch (choice)
            {
                case "1":
                    WriteLine($"Текущий префикс: {BitHelper.BitStringToString(current)} ({current})");
                    break;
                case "2":
                    if (candidate2 != null)
                    {
                        current = candidate2;
                        WriteLine("Новый текущий префикс принят из пункта 2.");
                        nextStr = BinaryStringIncrementer.GetNextSeniorString(current);
                        candidate2 = trie.Lower(nextStr);
                        candidate5 = trie.Upper(nextStr);
                    }
                    else
                    {
                        WriteLine("Кандидат для пункта 2 не найден.");
                    }
                    break;
                case "3":
                    {
                        // Пункты 1 и 2 остаются, пересчитываем пункт 5 на основе среднего(current, candidate2)
                        string mid = MiddlePrefixСomputer.ComputeMiddlePrefix(current, candidate2);
                        candidate5 = trie.Lower(mid);
                        WriteLine("Пункт 5 обновлён на основе среднего текущего и пункта 2.");
                    }
                    break;
                case "4":
                    {
                        // Пункты 1 и 5 остаются, пересчитываем пункт 2 на основе среднего(current, candidate5)
                        string mid = MiddlePrefixСomputer.ComputeMiddlePrefix(current, candidate5);
                        candidate2 = trie.Upper(mid);
                        WriteLine("Пункт 2 обновлён на основе среднего текущего и пункта 5.");
                    }
                    break;
                case "5":
                    if (candidate5 != null)
                    {
                        current = candidate5;
                        WriteLine("Новый текущий префикс принят из пункта 5.");
                        nextStr = BinaryStringIncrementer.GetNextSeniorString(current);
                        candidate2 = trie.Lower(nextStr);
                        candidate5 = trie.Upper(nextStr);
                    }
                    else
                    {
                        WriteLine("Кандидат для пункта 5 не найден.");
                    }
                    break;
                case "6":
                    WriteLine("Выход в главное меню.");
                    return;
                case "7":
                    string input = GetValidPrefix();
                    string nextSeniorString = BinaryStringIncrementer.GetNextSeniorString(input);
                    WriteLine($"{BitHelper.BitStringToString(nextSeniorString)} ({nextSeniorString})");
                    WriteLine();
                    break;
                case "8":
                    string first = GetValidPrefix();
                    string second = GetValidPrefix();
                    string midle=MiddlePrefixСomputer.ComputeMiddlePrefix(first, second);
                    WriteLine($"==== Конец отладки =====");

                    WriteLine($"Среднее: {BitHelper.BitStringToString(midle)} ({midle})");
                    break;
                default:
                    WriteLine("Неверный выбор подменю.");
                    break;
            }
        }
    }

    
    public static string GetValidPrefix()
    {
        string current;
        while (true)
        {
            Write("Введите текущий префикс (например, A): ");
            string input = ReadLine()?.Trim();
            if (string.IsNullOrEmpty(input))
            {
                WriteLine("Префикс не может быть пустым.");
                continue;
            }
            current = BitHelper.StringToBitString(input);
            if (current.Length % 8 == 0)
                break;
            WriteLine("Строка должна быть кратна 8 битам.");
        }
        return current;
    }
}