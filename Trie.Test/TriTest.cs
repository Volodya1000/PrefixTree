namespace Trie.Test;

public class TrieTests
{
    private readonly ITrie _trie;
    private readonly List<string> _testData;

    public TrieTests()
    {
        _trie = new Trie();
        _testData = GenerateTestData();

        foreach (var item in _testData)
        {
            _trie.Insert(BitHelper.StringToBitString(item));
        }
    }

    private List<string> GenerateTestData()
    {
        return new List<string>
        {
            // 1-char
            "A", "B", "C", "D", "E",

            // 2-chars
            "AB", "AC", "BA", "BB", "BC", "CA", "CB",
            "DA", "DB", "EA", "EB",

            // 3-chars
            "ABC", "ABD", "ACA", "ACB", "BAD", "BBA",
            "BCA", "BCB", "CAB", "CBA", "DAB", "EAB",

            // 4-chars
            "ABCD", "ABDA", "ACAA", "BACA", "BBAC",
            "BCAA", "CABB", "CBAA", "DABA", "EABC",

            // 5-chars
            "ABCDE", "ABDAC", "BACDE", "BCAAB", "CABDE",
            "CBAAA", "DABEA", "EABCD", "EBACD"
        };
    }

    [Theory]
    [InlineData("E", "DB")]       // Максимальный 5-char
    [InlineData("AB", "A")]          // Существующий 2-char
    [InlineData("ABC", "AB")]        // Средний 3-char
    [InlineData("ABDA", "ABD")]      // Частичное совпадение
    [InlineData("ABCDE", "ABCD")]    // Полное совпадение 5-char
    [InlineData("AZ", "ACB")]        // Несуществующий
    [InlineData("EBAC", "EB")]       // Частичный путь
    [InlineData("ZZZZZ", "EBACD")]   // Больше максимального
    public void Upper_ShouldHandleValidCases(string input, string expected)
    {
        var result = _trie.Upper(ToBitString(input));
        Assert.Equal(expected, FromBitString(result));
    }

    [Theory]
    [InlineData("A")]          // Минимальный элемент
    public void Upper_ShouldReturnNullForSpecificCases(string input)
    {
        var result = _trie.Upper(ToBitString(input));
        Assert.Null(result);
    }


    [Theory]
    [InlineData("AB", "ABC")]        // 2-char → 3-char
    [InlineData("ABA", "ABC")]      // Несуществующий 3-char
    [InlineData("ABDA", "ABDAC")]    // 4-char → 5-char
    [InlineData("DAB", "DABA")]      // 3-char → 4-char
    [InlineData("E", "EA")]          // 1-char → 2-char
    public void Lower_ShouldHandleValidCases(string input, string expected)
    {
        var result = _trie.Lower(ToBitString(input));
        Assert.Equal(expected, FromBitString(result));
    }

    [Theory]
    [InlineData("EBACD")]      // Максимальный элемент
    [InlineData("ZZ")]         // Несуществующий
    public void Lower_ShouldReturnNullForSpecificCases(string input)
    {
        var result = _trie.Lower(ToBitString(input));
        Assert.Null(result);
    }

    [Fact]
    public void ShouldHandleOverlengthKeys()
    {
        var longKey = new string('A', 10);
        var upperResult = _trie.Upper(ToBitString(longKey));
        var lowerResult = _trie.Lower(ToBitString(longKey));

        Assert.Equal("A", FromBitString(upperResult));
        Assert.Equal("AB", FromBitString(lowerResult));
    }

    private static string ToBitString(string s) =>
        BitHelper.StringToBitString(s);

    private static string FromBitString(string bits) =>
        BitHelper.BitStringToString(bits);
}
