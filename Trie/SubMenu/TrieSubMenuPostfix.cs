using static System.Console;
using static CritBit.BitHelper;

namespace CritBit;

/// <summary>
/// Класс для управления подменю работы с префиксами в CritBit дереве
/// </summary>
public class TrieSubMenuPostfix
{
    private Trie Root;
    private Trie CurrentNode;
    private string _currentPrefix = "";
    private readonly List<string> _outputBuffer = new();
    public bool EnableLogs = true;

    private string _postfix1;//часть строки 1 после префикса

    private string _postfix4;//часть строки 4 после префикса

    private int currentNodeBitStoreCount;

    private int previusCurrentPreffixLength=> previusCurrentPreffix.Length;

    private string previusCurrentPreffix;

    public int CurrentPrefixLength => _currentPrefix.Length;

    public TrieSubMenuPostfix(Trie trie)
    {
        CurrentNode = trie;
        Root = trie;
        InitializePrefixes();
    }

    /// <summary>
    /// Основной метод запуска подменю
    /// </summary>
    public void Run()
    {
        while (true)
        {
            ProcessOutputBuffer();
            DisplayMenu();

            var choice = ReadLine()?.Trim();
            if (ProcessChoice(choice)) break;
        }
    }

    #region Инициализация и отображение
    /// <summary>
    /// Инициализация начальных префиксов
    /// </summary>
    private void InitializePrefixes()
    {
        _postfix4 = CurrentNode.RightBranch(_currentPrefix);
        _postfix1 = CurrentNode.LeftBranch(0);//CurrentNode.Upper(_currentPrefix, CurrentPrefixLength);
    }

    /// <summary>
    /// Обработка буфера вывода сообщений
    /// </summary>
    private void ProcessOutputBuffer()
    {
        if (EnableLogs)
            foreach (var message in _outputBuffer)
                WriteLine(message);

        _outputBuffer.Clear();
    }

    /// <summary>
    /// Отображение меню с текущими состояниями
    /// </summary>
    private void DisplayMenu()
    {
        WriteLine($"[{BitStringToString(_currentPrefix)}]");
        WriteLine($"1. {(_postfix1 != null ? $"[{BitStringToString(ConcatCurrentPrefixWithPostfix(_postfix1))}]" : "нет кандидата")}");
        WriteLine("2. ...");
        WriteLine("3. ...");
        WriteLine($"4. {(_postfix4 != null ? $"[{BitStringToString(ConcatCurrentPrefixWithPostfix(_postfix4))}]" : "нет кандидата")}");
        WriteLine($"5. Начать с корня");
        WriteLine("6. Главное меню");
        WriteLine("7. Показать поддерево");
        Write("Выберите пункт: ");
    }
    #endregion

    #region Обработка выбора
    /// <summary>
    /// Обработка выбора пользователя
    /// </summary>
    /// <returns>True если нужно выйти в главное меню</returns>
    private bool ProcessChoice(string choice)
    {
        switch (choice)
        {
            case "1": HandleItem1(); break;
            case "2": HandleItem2(); break;
            case "3": HandleItem3(); break;
            case "4": HandleItem4(); break;
            case "5": StartFromRoot(); break;
            case "6": return true; 
            case "7": WriteLine(CurrentNode.GetASCIIChildren(_currentPrefix)); break;
            default: _outputBuffer.Add("Неверный выбор"); break;
        }
        return false;
    }
    #endregion

    #region Обработчики действий
    private void StartFromRoot()
    {
        _currentPrefix = "";
        previusCurrentPreffix = "";
        CurrentNode = Root;
        InitializePrefixes();
    }

    private void HandleItem1()
    {
        previusCurrentPreffix = _currentPrefix;


        _outputBuffer.Add("Общий префикс обновляем на строку 1");
        _currentPrefix = ConcatCurrentPrefixWithPostfix(_postfix1);
        UpdateCurrentNode(_currentPrefix);

        string? rightBranchForPrefix1 = CurrentNode.RightBranch(tookFromRoot: currentNodeBitStoreCount);
        if (rightBranchForPrefix1 != null)
        {
            _outputBuffer.Add($"RightBranch для строки 1: {FormatBitString(ConcatCurrentPrefixWithPostfix(rightBranchForPrefix1))}");
            _postfix4 = rightBranchForPrefix1;
        }
        else
            _outputBuffer.Add("RightBranch для строки 1 НЕ существует");

        string upperForPrefix1 = CurrentNode.LeftBranch(tookFromRoot: currentNodeBitStoreCount);

        if (upperForPrefix1 != null)
        {
            _postfix1 = upperForPrefix1;
            _outputBuffer.Add($"Upper для строки 1: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperForPrefix1))}");
        }
        else
            _outputBuffer.Add("Upper для строки 1 не существует");


        ComputeComonPrefixAndUpdateCurrentNode();


    }

    private void HandleItem4()
    {
        previusCurrentPreffix = _currentPrefix;



        _outputBuffer.Add("Сохраняем строку 4 в строку 1");
        _postfix1 = _postfix4;


        _outputBuffer.Add("Общий префикс меняем на строку 4");
        _currentPrefix = ConcatCurrentPrefixWithPostfix(_postfix4);
        UpdateCurrentNode(_currentPrefix);


        string? rightBranchForPrefix4 = CurrentNode.RightBranch(tookFromRoot: currentNodeBitStoreCount);
        if (rightBranchForPrefix4 != null)
        {
            _outputBuffer.Add($"RightBranch для строки 4 существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(rightBranchForPrefix4))}");
            _postfix4 = rightBranchForPrefix4;
        }
        else
            _outputBuffer.Add("RightBranch для строки 1 НЕ существует");

        string upperForPrefix4 = CurrentNode.LeftBranch(tookFromRoot: currentNodeBitStoreCount);

        if (upperForPrefix4 != null)
        {
            _postfix1 = upperForPrefix4;
            _outputBuffer.Add($"Upper для строки 4: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperForPrefix4))}");
        }

        ComputeComonPrefixAndUpdateCurrentNode();
    }

    

    private void HandleItem2()
    {

        previusCurrentPreffix = _currentPrefix;


        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(_postfix1, _postfix4, CurrentNode, roundUp: true, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(ConcatCurrentPrefixWithPostfix(mid))}");

        var lowerMid = CurrentNode.LowerApril(mid, CurrentPrefixLength);

        if (!String.IsNullOrEmpty(lowerMid) )//&& CurrentNode.CheckSubstringExists(lowerMid))
        {
            _outputBuffer.Add($"Префикс Lower для средней существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(lowerMid))}");
            _postfix4 = lowerMid;
            // _currentPrefix = ComputeComonPrefix();

            //UpdateCurrentNode(_currentPrefix);
            ComputeComonPrefixAndUpdateCurrentNode();
        }
        else
        {
            _outputBuffer.Add("Префикс Lower для средней НЕ существует");
            Set1and4asCurrent();
        }
    }

    /// <summary> Вычисление нижней средней </summary>
    private void HandleItem3()
    {
        previusCurrentPreffix = _currentPrefix;



        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(_postfix1, _postfix4, CurrentNode, roundUp: true, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(ConcatCurrentPrefixWithPostfix(mid))}");

        if (!String.IsNullOrEmpty(mid)&& CurrentNode.CheckSubstringExists(mid))
        {
            _outputBuffer.Add("Префикс средней существует");
            _postfix1 = mid;
        }
        else
        {
            _outputBuffer.Add("Префикс средней НЕ существует");
            var upperMid = CurrentNode.UpperApril(mid, CurrentPrefixLength);

            if (CurrentNode.CheckSubstringExists(upperMid))
            {
                _outputBuffer.Add($"Префикс Upper для средней существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperMid))}");
                _postfix1 = upperMid;

                //_currentPrefix = ComputeComonPrefix();

                //UpdateCurrentNode(_currentPrefix);
                ComputeComonPrefixAndUpdateCurrentNode();
            }
            else
            {
                _outputBuffer.Add("Префикс Upper для средней НЕ существует");
                Set1and4asCurrent();
            }
        }
    }

    private string ComputeComonPrefix()
    {
        

        //_outputBuffer.Add("Обновление общего префикса");
        //int commomPrefixLength = FindCommonPrefixLength(ConcatCurrentPrefixWithPostfix(_postfix1), ConcatCurrentPrefixWithPostfix(_postfix4));
        //string rezult = (_currentPrefix+_postfix1).Substring(0, commomPrefixLength);
        //return rezult;

        _outputBuffer.Add("Обновление общего префикса");
        int commomPrefixLength = FindCommonPrefixLength(_postfix1, _postfix4);
        string rezult = _currentPrefix + _postfix1.Substring(0, commomPrefixLength);

        if(!String.IsNullOrEmpty(_postfix1))
            _postfix1= _postfix1.Substring(commomPrefixLength);
        if (!String.IsNullOrEmpty(_postfix4))
            _postfix4 = _postfix4.Substring(commomPrefixLength);


        return  rezult;
    }

    private void ComputeComonPrefixAndUpdateCurrentNode()
    {
        string savePrefix = _currentPrefix;
        _currentPrefix = ComputeComonPrefix();

        if (savePrefix != _currentPrefix)
        {

            int commomPrefixLength = FindCommonPrefixLength(savePrefix, _currentPrefix);
            string rezult = _currentPrefix.Substring(commomPrefixLength);
            previusCurrentPreffix = "";
            UpdateCurrentNode(rezult);
        }
    }

    #endregion

    #region Вспомогательные методы
    private void Set1and4asCurrent()
    {
        _outputBuffer.Add("Приравниваем строку 1 и строку 4 к общему префиксу");
        _postfix1 = "";
        _postfix4 = "";
    }

    //private string ConcatCurrentPrefixWithPostfix(string postfix) => previusCurrentPreffix + postfix;//.Substring(CurrentPrefixLength);

    private string ConcatCurrentPrefixWithPostfix(string postfix) => _currentPrefix + postfix;//.Substring(CurrentPrefixLength);


    private void UpdateCurrentNode(string bitString)
    {
        CurrentNode = new Trie(FindCurrentPrefixNode(bitString.Substring(previusCurrentPreffixLength)));


    }

    private TrieNode FindCurrentPrefixNode(string bitString)
    {
        (TrieNode newCurrentNode, _, currentNodeBitStoreCount) = CurrentNode.FindLastNodeInPath(bitString);
        return newCurrentNode;
    }
    #endregion
}