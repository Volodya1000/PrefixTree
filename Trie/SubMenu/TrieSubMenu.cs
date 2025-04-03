using System.Collections.Generic;
using static System.Console;
using static CritBit.BitHelper;
namespace CritBit;

/// <summary>
/// Класс для управления подменю работы с префиксами в CritBit дереве
/// </summary>
public static class TrieSubMenu
{
    private static Trie Root;
    public static Trie CurrentNode;
    private static string _currentPrefix = "";
    private static string _prefix1;
    private static string _prefix4;
    private static readonly List<string> _outputBuffer = new();
    public static bool EnableLogs = true;

    public static int CurrentPrefixLength => _currentPrefix.Length;

    /// <summary>
    /// Основной метод запуска подменю
    /// </summary>
    /// <param name="trie">Экземпляр CritBit дерева</param>
    public static void Run(Trie trie)
    {
        Root = trie;
        CurrentNode = trie;
        InitializePrefixes(trie);

        while (true)
        {
            ProcessOutputBuffer();
            DisplayMenu();

            var choice = ReadLine()?.Trim();
            if (ProcessChoice(choice, trie)) break;
        }
    }

    #region Инициализация и отображение
    /// <summary>
    /// Инициализация начальных префиксов
    /// </summary>
    private static void InitializePrefixes(Trie trie)
    {
        _prefix4 = trie.RightBranch(_currentPrefix);
        _prefix1 = trie.Upper(_currentPrefix, CurrentPrefixLength);
    }

    /// <summary>
    /// Обработка буфера вывода сообщений
    /// </summary>
    private static void ProcessOutputBuffer()
    {
        if(EnableLogs)
            foreach (var message in _outputBuffer)
                WriteLine(message);

        _outputBuffer.Clear();
    }

    /// <summary>
    /// Отображение меню с текущими состояниями
    /// </summary>
    private static void DisplayMenu()
    {
        WriteLine($"[{BitHelper.BitStringToString(_currentPrefix)}]");
        WriteLine($"1. {(_prefix1 != null ? $"[{BitHelper.BitStringToString(_prefix1)}]" : "нет кандидата")}");
        WriteLine("2. ...");
        WriteLine("3. ...");
        WriteLine($"4. {(_prefix4 != null ? $"[{BitHelper.BitStringToString(_prefix4)}]" : "нет кандидата")}");
        WriteLine($"5. Начать с корня");
        WriteLine("6. Главное меню");
        /*WriteLine("====== Для тестирования ====");
        WriteLine("7. Строка старше на 1");
        WriteLine("8. Среднее для двух строк");
        WriteLine("9. Проверка наличия префикса");
        */
        Write("Выберите пункт: ");
    }
    #endregion

    #region Обработка выбора
    /// <summary>
    /// Обработка выбора пользователя
    /// </summary>
    /// <returns>True если нужно выйти в главное меню</returns>
    private static bool ProcessChoice(string choice, Trie trie)
    {
        switch (choice)
        {
            case "1": HandleItem1(trie); break;
            case "2": HandleItem2(trie); break;
            case "3": HandleItem3(trie); break;
            case "4": HandleItem4(trie); break;
            case "5": StartFromRoot(trie); break;
            case "6": return true; 
            case "7": HandleSeniorString(); break;
            case "8": HandleCustomAverage(trie); break;
            case "9": HandlePrefixCheck(trie); break;
            default: _outputBuffer.Add("Неверный выбор"); break;
        }
        return false;
    }
    #endregion

    #region Обработчики действий


    private static void StartFromRoot(Trie trie)
    {
        _currentPrefix = "";
        CurrentNode = Root;
        InitializePrefixes(trie);
    }

    private static void HandleItem1(Trie trie)
    {
        _outputBuffer.Add("Общий префикс обновляем на строку 1");
        _currentPrefix = _prefix1;

        string? rightBranchForPrefix1 = trie.RightBranch(_prefix1);
        if (rightBranchForPrefix1 != null)
        {
            _outputBuffer.Add($"RightBranch для строки 1: {FormatBitString(rightBranchForPrefix1)}");
            _prefix4 = rightBranchForPrefix1;
        }
        else
            _outputBuffer.Add("RightBranch для строки 1 НЕ существует");

        string upperForPrefix1 = trie.Upper(_prefix1, CurrentPrefixLength);

        if (upperForPrefix1 != null)
        {
            _prefix1 = upperForPrefix1;
            _outputBuffer.Add($"Upper для строки 1: {FormatBitString(upperForPrefix1)}");
        }
        else
            _outputBuffer.Add("Upper для строки 1 не существует");

        _currentPrefix = ComputeComonPrefix(trie);
    }
    
    private static void HandleItem4(Trie trie)
    {
        _outputBuffer.Add("Сохраняем строку 4 в строку 1");
        _outputBuffer.Add("Общий префикс меняем на строку 4");
        _prefix1 = _prefix4;

        _currentPrefix = _prefix4;

        string? rightBranchForPrefix4 = trie.RightBranch(_prefix4);
        if (rightBranchForPrefix4 != null)
        {
            _outputBuffer.Add($"RightBranch для строки 4 существует: {FormatBitString(rightBranchForPrefix4)}");
            _prefix4 = rightBranchForPrefix4;
        }
        else
            _outputBuffer.Add("RightBranch для строки 1 НЕ существует");

        string upperForPrefix4 = trie.Upper(_prefix1, CurrentPrefixLength);

        if (upperForPrefix4 != null)
        { 
            _prefix1 = upperForPrefix4;
            _outputBuffer.Add($"Upper для строки 4: {FormatBitString(upperForPrefix4)}");
        }

        _currentPrefix = ComputeComonPrefix(trie);
    }

    private static void HandleItem2(Trie trie)
    {
        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(_prefix1, _prefix4, trie, roundUp: true, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(mid)}");

        var lowerMid = trie.Lower(mid,CurrentPrefixLength);

        if (trie.CheckSubstringExists(lowerMid))
        {
            _outputBuffer.Add($"Префикс Lower для средней существует: {FormatBitString(lowerMid)}");
            _prefix4 = lowerMid;
            _currentPrefix = ComputeComonPrefix(trie);
        }
        else
        {
            _outputBuffer.Add("Префикс Lower для средней НЕ существует");
            ResetPrefixes();
        }
    }

    /// <summary> Вычисление нижней средней </summary>
    private static void HandleItem3(Trie trie)
    {
        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(_prefix1, _prefix4, trie, roundUp: false, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(mid)}");

        if (trie.CheckSubstringExists(mid))
        {
            _outputBuffer.Add("Префикс средней существует");
            _prefix1 = mid;
        }
        else
        {
            _outputBuffer.Add("Префикс средней НЕ существует");
            var upperMid = trie.Upper(mid, CurrentPrefixLength);

            if (trie.CheckSubstringExists(upperMid))
            {
                _outputBuffer.Add($"Префикс Upper для средней существует: {FormatBitString(upperMid)}");
                _prefix1 = upperMid;
                _currentPrefix = ComputeComonPrefix(trie);
            }
            else
            {
                _outputBuffer.Add("Префикс Upper для средней НЕ существует");
                ResetPrefixes();
            }
        }
    }
    private static string ComputeComonPrefix(Trie trie)
    {
        _outputBuffer.Add("Обновление общего префикса");
        int commomPrefixLength= FindCommonPrefixLength(_prefix1, _prefix4);
        string rezult = _prefix1.Substring(0,commomPrefixLength);
        return rezult;
    }


    /// <summary> Генерация старшей строки </summary>
    private static void HandleSeniorString()
    {
        var input = GetValidPrefix();
        var next = BinaryStringIncrementer.GetNextSeniorString(input);
        _outputBuffer.Add($"{BitHelper.BitStringToString(next)} ({next})");
    }

    /// <summary> Вычисление среднего для произвольных строк </summary>
    private static void HandleCustomAverage(Trie trie)
    {
        var first = GetValidPrefix();
        var second = GetValidPrefix();
        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(first, second, trie, true, out string logs);
        _outputBuffer.Add(logs);

        _outputBuffer.Add($"Среднее: {BitHelper.BitStringToString(mid)} ({mid})");
    }

    /// <summary> Проверка наличия префикса </summary>
    private static void HandlePrefixCheck(Trie trie)
    {
        var prefix = GetValidPrefix();
        var exists = trie.CheckSubstringExists(prefix);
        _outputBuffer.Add(exists ? "Префикс существует" : "Префикс НЕ существует");
    }
    #endregion

    #region Вспомогательные методы
    


    /// <summary> Обновление префиксов после изменения текущего </summary>
    //private static void UpdatePrefixes(Trie trie)
    //{
    //    _prefix1 = trie.RightBranch(_currentPrefix)?? _currentPrefix;
    //    _prefix4 = trie.Lower(_currentPrefix);
    //}

    /// <summary> Сброс префиксов при ошибках </summary>
    private static void ResetPrefixes()
    {
        _outputBuffer.Add("Приравниваем строку 1 и строку 4 к общему префиксу");
        _prefix1 = _currentPrefix;
        _prefix4 = _currentPrefix;
    }

    /// <summary>
    /// Получение валидного префикса от пользователя
    /// </summary>
    private static string GetValidPrefix()
    {
        while (true)
        {
            Write("Введите префикс (например, A): ");
            var input = ReadLine()?.Trim();
            var bits = BitHelper.StringToBitString(input ?? "");

            if (bits.Length % 8 == 0)
                return bits;

            _outputBuffer.Add("Ошибка: длина должна быть кратна 8 битам");
        }
    }
    #endregion
}
