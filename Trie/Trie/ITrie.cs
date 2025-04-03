namespace CritBit;

public interface ITrie
{
    void Insert(string bitString);
    string Upper(string bitString);
    string Lower(string bitString);
}
