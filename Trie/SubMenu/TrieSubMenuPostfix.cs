namespace CritBit;

/// <summary>
/// Класс для управления подменю работы с префиксами в CritBit дереве
/// </summary>
public class TrieSubMenuPostfix
{
    //===Для логирования
    private readonly List<string> _outputBuffer = new();
    public bool EnableLogs = true;
    //======


    //====== Для хранения состояния меню
    private Trie Root;

    private Trie CurrentNode;

    private string currentPrefix = "";

    private string postfix1;//часть строки 1 после префикса

    private string postfix4;//часть строки 4 после префикса

    private int currentNodeBitStoreCount;

    public int CurrentPrefixLength => currentPrefix.Length;

    //======

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
        postfix4 = CurrentNode.RightBranch(0);
        postfix1 = CurrentNode.Upper("",0);
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
        WriteLine($"[{BitStringToString(currentPrefix)}]");
        WriteLine($"1. {(postfix1 != null ? $"[{BitStringToString(ConcatCurrentPrefixWithPostfix(postfix1))}]" : "нет кандидата")}");
        WriteLine("2. ...");
        WriteLine("3. ...");
        WriteLine($"4. {(postfix4 != null ? $"[{BitStringToString(ConcatCurrentPrefixWithPostfix(postfix4))}]" : "нет кандидата")}");
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
            case "7": WriteLine(CurrentNode.GetASCIIChildren(currentPrefix)); break;
            default: _outputBuffer.Add("Неверный выбор"); break;
        }
        return false;
    }
    #endregion

    #region Обработчики действий
    private void StartFromRoot()
    {
        currentPrefix = "";
        CurrentNode = Root;
        currentNodeBitStoreCount = 0;
        InitializePrefixes();
    }

    private void HandleItem1()
    {
        if(IsLeaf(CurrentNode.root)) return;


        currentPrefix = ConcatCurrentPrefixWithPostfix(postfix1);
        _outputBuffer.Add($"Общий префикс обновляем на строку 1:{FormatBitString(currentPrefix)}");
       
        UpdateCurrentNode(currentPrefix);

        if (IsLeaf(CurrentNode.root)) return;

        string? rightBranchForPrefix1 = CurrentNode.RightBranch(tookFromRoot: currentNodeBitStoreCount);
        if (!String.IsNullOrEmpty(rightBranchForPrefix1))
        {
            _outputBuffer.Add($"RightBranch для строки 1: {FormatBitString(ConcatCurrentPrefixWithPostfix(rightBranchForPrefix1))}");
            postfix4 = rightBranchForPrefix1;
        }
        else
        {
            postfix4 = "";
            _outputBuffer.Add("RightBranch для строки 1 НЕ существует");
        }

        string? upperForPrefix1 = CurrentNode.Upper("", tookFromRoot: currentNodeBitStoreCount);

        if (!String.IsNullOrEmpty(upperForPrefix1))
        {
            postfix1 = upperForPrefix1;
            _outputBuffer.Add($"Upper для строки 1: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperForPrefix1))}");
        }
        else
        {
            postfix1 = "";
            _outputBuffer.Add("Upper для строки 1 не существует");
        }
         ComputeComonPrefixAndUpdateCurrentNode();
    }

    private void HandleItem4()
    {
        if (IsLeaf(CurrentNode.root)) return;

        _outputBuffer.Add("Сохраняем строку 4 в строку 1");
        postfix1 = postfix4;

       
        currentPrefix = ConcatCurrentPrefixWithPostfix(postfix4);
        _outputBuffer.Add($"Общий префикс меняем на строку 4: {FormatBitString(currentPrefix)}");

        UpdateCurrentNode(currentPrefix);

        if (IsLeaf(CurrentNode.root)) return;


        string? rightBranchForPrefix4 = CurrentNode.RightBranch(tookFromRoot: currentNodeBitStoreCount);
        if (!String.IsNullOrEmpty(rightBranchForPrefix4))
        {
            _outputBuffer.Add($"RightBranch для строки 4 существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(rightBranchForPrefix4))}");
            postfix4 = rightBranchForPrefix4;
        }
        else
        {
            postfix4 = "";
            _outputBuffer.Add("RightBranch для строки 4 НЕ существует");
        }

        string? upperForPrefix4 = CurrentNode.Upper("", tookFromRoot: currentNodeBitStoreCount);

        if (!String.IsNullOrEmpty(upperForPrefix4))
        {
            postfix1 = upperForPrefix4;
            _outputBuffer.Add($"Upper для строки 4: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperForPrefix4))}");
        }
        else
        {
            postfix1 = "";
            _outputBuffer.Add("Upper для строки 4 не существует");
        }
        ComputeComonPrefixAndUpdateCurrentNode();
    }

    

    private void HandleItem2()
    {
        if (postfix1 == postfix4) return;
        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(postfix1, postfix4, CurrentNode, roundUp: true, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(ConcatCurrentPrefixWithPostfix(mid))}");

        var lowerMid = CurrentNode.Lower(mid, CurrentPrefixLength);

        if (!String.IsNullOrEmpty(lowerMid))
        {
            _outputBuffer.Add($"Префикс Lower для средней существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(lowerMid))}");
            postfix4 = lowerMid;

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
        if (postfix1 == postfix4) return;
        var mid = MiddlePrefixComputer.ComputeMiddlePrefix(postfix1, postfix4, CurrentNode, roundUp: true, out string logs);
        _outputBuffer.Add(logs);
        _outputBuffer.Add($"Средняя: {FormatBitString(ConcatCurrentPrefixWithPostfix(mid))}");

        if (!String.IsNullOrEmpty(mid)&& CurrentNode.CheckSubstringExists(mid, tookFromRoot: currentNodeBitStoreCount))
        {
            _outputBuffer.Add("Префикс средней существует");
            postfix1 = mid;
        }
        else
        {
            _outputBuffer.Add("Префикс средней НЕ существует");
            var upperMid = CurrentNode.Upper(mid, CurrentPrefixLength);

            if (!String.IsNullOrEmpty(upperMid))
            {
                _outputBuffer.Add($"Префикс Upper для средней существует: {FormatBitString(ConcatCurrentPrefixWithPostfix(upperMid))}");
                postfix1 = upperMid;

                ComputeComonPrefixAndUpdateCurrentNode();
            }
            else
            {
                _outputBuffer.Add("Префикс Upper для средней НЕ существует");
                Set1and4asCurrent();
            }
        }
    }
    #endregion

    #region Вспомогательные методы

    private string ComputeComonPrefix()
    {
        _outputBuffer.Add("Обновление общего префикса");
        int commomPrefixLength = FindCommonPrefixLength(postfix1, postfix4);
        string rezult = currentPrefix + postfix1.Substring(0, commomPrefixLength);

        if (!String.IsNullOrEmpty(postfix1))
            postfix1 = postfix1.Substring(commomPrefixLength);
        if (!String.IsNullOrEmpty(postfix4))
            postfix4 = postfix4.Substring(commomPrefixLength);


        return rezult;
    }

    private void ComputeComonPrefixAndUpdateCurrentNode()
    {
        string savePrefix = currentPrefix;
        currentPrefix = ComputeComonPrefix();

        if (savePrefix != currentPrefix)
        {

            int commomPrefixLength = FindCommonPrefixLength(savePrefix, currentPrefix);
            string rezult = currentPrefix.Substring(commomPrefixLength);
            UpdateCurrentNode(rezult);
        }
    }

    private void Set1and4asCurrent()
    {
        _outputBuffer.Add("Приравниваем строку 1 и строку 4 к общему префиксу");
        postfix1 = "";
        postfix4 = "";
    }

    private string ConcatCurrentPrefixWithPostfix(string postfix) => currentPrefix + postfix;

   
    private void UpdateCurrentNode(string bitString)
    {
        string bitStringPostfix = bitString.Substring(currentNodeBitStoreCount);
        (TrieNode newCurrentNode, currentNodeBitStoreCount) = CurrentNode.FindLastNodeInPath(bitStringPostfix, tookFromRoot: currentNodeBitStoreCount);
        if (IsLeaf(newCurrentNode))
        {
            currentPrefix = currentPrefix + newCurrentNode.BitString.Substring(currentNodeBitStoreCount);
             Set1and4asCurrent();
        }
        CurrentNode = new Trie(newCurrentNode);
    }
    #endregion
}