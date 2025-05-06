namespace CritBit;

public partial class Trie
{

    //17.04
    public string LowerApril(string key,int tookFromRoot)
    {
        string resultStart = "";
        int shouldFindLength;

        string result = "";

        //в качестве ключа учитываются только первые 8 бит ключа
        string keyFirstBait = key.Substring(0, 8);

        //сколько еще можно взять из корня
        int canTakeFromRoot = root.BitString.Length - tookFromRoot; 
        if (tookFromRoot != root.BitString.Length)
            resultStart += root.BitString.Substring(tookFromRoot);

        //Смотрим сколько бит в первом наследнике 
        if (canTakeFromRoot >= 8)
        {
            int k = CalculateK(canTakeFromRoot);
            string resultFromRoot = root.BitString.Substring(tookFromRoot, k);
            return resultFromRoot;
        }
        else if (canTakeFromRoot == 0)
        {
            //Получаем первого наследника
            TrieNode firstChild = root.Children.FirstOrDefault(c => c.BitString.StartsWith("0"))
               ?? root.Children.FirstOrDefault(c => c.BitString.StartsWith("1"));

            if (firstChild == null) return null;

            int firstChildLength = firstChild.BitString.Length;

            //Смотрим сколько бит в млпдшем наследнике  корня
            if (firstChildLength >= 8)
            {
                int k = CalculateK(firstChildLength);
                string resultFromFirstChild = firstChild.BitString.Substring(0, k);
                return resultFromFirstChild;
            }
            else
            { //обход в глубину добираем до длины 8
                resultStart = firstChild.BitString; //берём биты до конца корня

                LowerRecursive(root, resultStart, keyFirstBait, ref result);

                return result;
            }
        }
        else
        {
            resultStart = root.BitString.Substring(tookFromRoot); //берём биты до конца корня
                                                                  //обход в глубину добираем до длины 8

            LowerRecursive(root, resultStart, keyFirstBait, ref result);

            return result;
        }
    }



    /// <summary>
    /// Рекурсивный обход дерева для поиска строки, которая наибольшая меньшая для key
    /// </summary>
    private void LowerRecursive(TrieNode node, string currentString, string key, ref string result)
    {
        // Если длина текущей строки кратна 8 и текущая строка меньше ключа, обновляем результат
        if (currentString.Length == 8)
        {
            if (string.Compare(currentString, key) < 0)
            {
                if (result == null || string.Compare(currentString, result) > 0)
                {
                    result = currentString;
                }
            }
            //если currentString >key то обходить в глубину нет смысла
            //так как ищем наибольшую МЕНЬШУЮ и поэтом удля оптимизации выходим
            else
                return;
        }

        // Обходим все дочерние узлы
        foreach (var child in node.Children)
        {
            int shouldTake = Math.Min(child.BitString.Length,8- currentString.Length);
            LowerRecursive(child, currentString + child.BitString.Substring(0, shouldTake), key, ref result);
        }
    }




    public string UpperApril(string key, int tookFromRoot)
    {
        string resultStart = "";
        int shouldFindLength;

        string result = "";

        //в качестве ключа учитываются только первые 8 бит ключа
        string keyFirstBait = key.Substring(0, 8);

        //сколько еще можно взять из корня
        int canTakeFromRoot = root.BitString.Length - tookFromRoot;
        if (tookFromRoot != root.BitString.Length)
            resultStart += root.BitString.Substring(tookFromRoot);

        //Смотрим сколько бит в первом наследнике 
        if (canTakeFromRoot >= 8)
        {
            int k = CalculateK(canTakeFromRoot);
            string resultFromRoot = root.BitString.Substring(tookFromRoot, k);
            return resultFromRoot;
        }
        else if (canTakeFromRoot == 0)
        {
            //Получаем первого наследника
            TrieNode firstChild = root.Children.FirstOrDefault(c => c.BitString.StartsWith("1"))
               ?? root.Children.FirstOrDefault(c => c.BitString.StartsWith("0"));

            if (firstChild == null) return null;

            int firstChildLength = firstChild.BitString.Length;

            //Смотрим сколько бит в млпдшем наследнике  корня
            if (firstChildLength >= 8)
            {
                int k = CalculateK(firstChildLength);
                string resultFromFirstChild = firstChild.BitString.Substring(0, k);
                return resultFromFirstChild;
            }
            else
            { //обход в глубину добираем до длины 8
                resultStart = firstChild.BitString; //берём биты до конца корня

                UpperRecursive(root, resultStart, keyFirstBait, ref result);

                return result;
            }
        }
        else
        {
            resultStart = root.BitString.Substring(tookFromRoot); //берём биты до конца корня
                                                                  //обход в глубину добираем до длины 8

            UpperRecursive(root, resultStart, keyFirstBait, ref result);

            return result;
        }
    }



    /// <summary>
    /// Рекурсивный обход дерева для поиска строки, которая наибольшая меньшая для key
    /// </summary>
    private void UpperRecursive(TrieNode node, string currentString, string key, ref string result)
    {
        // Если длина текущей строки кратна 8 и текущая строка меньше ключа, обновляем результат
        if (currentString.Length == 8)
        {
            if (string.Compare(currentString, key) > 0)
            {
                if (result == null || string.Compare(currentString, result) <0)
                {
                    result = currentString;
                }
            }
            //если currentString >key то обходить в глубину нет смысла
            //так как ищем наибольшую МЕНЬШУЮ и поэтом удля оптимизации выходим
            else
                return;
        }

        // Обходим все дочерние узлы
        foreach (var child in node.Children.OrderByDescending(c => c.BitString).ToList())
        {
            int shouldTake = Math.Min(child.BitString.Length, 8 - currentString.Length);
            UpperRecursive(child, currentString + child.BitString.Substring(0, shouldTake), key, ref result);
        }
    }


}
