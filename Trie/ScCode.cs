using System.Xml.Linq;

namespace Trie;

// Базовый класс для SC-элемента
public abstract class ScElement
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public abstract bool IsEmpty();
    public abstract object GetValue();
    public abstract void Delete();
}

// Класс для SC-узла
public class ScNode : ScElement
{
    private object _value;

    public ScNode(object value)
    {
        _value = value ?? throw new ArgumentNullException(nameof(value), "Значение SC-узла не может быть пустым.");
    }

    public override bool IsEmpty()
    {
        return _value == null;
    }

    public override object GetValue()
    {
        return _value;
    }

    public override void Delete()
    {
        _value = null;
        Console.WriteLine($"SC-узел с ID {Id} удален.");
    }
    public static bool IsScNode(ScElement element)
    {
        return element is ScNode;
    }
}

// Класс для SC-дуги
public class ScArc : ScElement
{
    public ScElement Source { get; private set; }
    public ScElement Target { get; private set; }

    public ScArc(ScElement source, ScElement target)
    {
        Source = source ?? throw new ArgumentNullException(nameof(source), "Источник SC-дуги не может быть null.");
        Target = target ?? throw new ArgumentNullException(nameof(target), "Цель SC-дуги не может быть null.");
    }
    public override bool IsEmpty()
    {
        return Source == null || Target == null;
    }

    public override object GetValue()
    {
        return $"Дуга от {Source.Id} к {Target.Id}";
    }

    public override void Delete()
    {
        Source = null;
        Target = null;
        Console.WriteLine($"SC-дуга с ID {Id} удалена.");
    }

    public static bool IsScArc(ScElement element)
    {
        return element is ScArc;
    }

    public ScElement GetSource()
    {
        return Source;
    }

    public ScElement GetTarget()
    {
        return Target;
    }
}

// Класс для группы SC-элементов
public class ScElementGroup
{
    private readonly HashSet<ScElement> _elements = new HashSet<ScElement>();

    public void AddElement(ScElement element)
    {
        if (element == null)
        {
            throw new ArgumentNullException(nameof(element), "SC-элемент не может быть null.");
        }
        _elements.Add(element);
    }

    public HashSet<ScElement> Intersect(ScElementGroup otherGroup)
    {
        return new HashSet<ScElement>(_elements.Intersect(otherGroup._elements));
    }

    public HashSet<ScElement> Union(ScElementGroup otherGroup)
    {
        return new HashSet<ScElement>(_elements.Union(otherGroup._elements));
    }

    public HashSet<ScElement> Sum(ScElementGroup otherGroup)
    {
        return new HashSet<ScElement>(_elements.Except(otherGroup._elements).Concat(otherGroup._elements.Except(_elements)));
    }
    public ScElement GetFirstElement()
    {
        return _elements.FirstOrDefault();
    }

    public IEnumerable<ScElement> GetElements()
    {
        return _elements;
    }
}

//public class Example
//{
//    public static void Main(string[] args)
//    {
//        // Пример создания sc цепочки

//        //Создание ячеек
//        ScNode cell1 = new ScNode("");
//        ScNode cell2 = new ScNode(cell1);
//        ScNode cell3 = new ScNode(cell2);

//        //создание содержимого ячеек
//        ScNode content1 = new ScNode("A");
//        ScNode content2 = new ScNode("B");
//        ScNode content3 = new ScNode("C");

//        //дуги
//        ScArc arc1 = new ScArc(cell1, content1);
//        ScArc arc2 = new ScArc(cell2, content2);
//        ScArc arc3 = new ScArc(cell3, content3);
//    }
//}

