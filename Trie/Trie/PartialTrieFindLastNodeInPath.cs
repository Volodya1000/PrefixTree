namespace CritBit;

public partial class Trie
{
    public (TrieNode, int nodeStoreCount, int busyInPathCount) FindLastNodeInPath(string bitString)
    {
        // Текущий узел, начинаем с корня дерева
        TrieNode currentNode = root;
        // Оставшаяся часть битовой строки для обработки
        string remaining = bitString;

        // Пока есть необработанные биты в строке
        while (remaining.Length > 0)
        {
            bool foundChild = false;

            // Проверяем всех детей текущего узла
            foreach (var child in currentNode.Children.ToList())
            {
                // Если дочерний узел полностью совпадает с началом remaining
                if (remaining.StartsWith(child.BitString))
                {
                    // Уменьшаем remaining на длину битовой строки дочернего узла
                    remaining = remaining.Substring(child.BitString.Length);
                    // Переходим в этот дочерний узел
                    currentNode = child;
                    foundChild = true;
                    break;
                }
            }

            // Если не нашли полного совпадения с детьми
            if (!foundChild)
            {
                // Проверяем частичное совпадение (remaining - префикс битовой строки ребенка)
                foreach (var child in currentNode.Children)
                {
                    if (child.BitString.StartsWith(remaining))
                    {
                        // Возвращаем:
                        // 1. Найденный дочерний узел (последний в пути)
                        // 2. Общее количество бит в этом узле (длина его BitString)
                        // 3. Количество занятых бит (длина remaining - сколько бит из узла использовано)
                        return (child, child.BitString.Length, remaining.Length);
                    }
                }
                // Теоретически недостижимо, так как CheckSubstringExists гарантирует существование
                Console.WriteLine($"Недостижим путь: {BitHelper.BitStringToString(bitString)} ({bitString})");
                throw new InvalidOperationException("Путь недостижим");
            }
        }

        // Если обработали все биты (remaining пуст) - текущий узел последний
        // В этом случае все биты узла полностью заняты в пути
        return (currentNode, currentNode.BitString.Length, currentNode.BitString.Length);
    }
}
