using System.Text;

namespace CritBit;


/// <summary>
/// Префиксное дерево для работы с битовыми строками
/// </summary>
public partial class Trie 
{
    public TrieNode root;

    public Trie()
    {
        root = new TrieNode("");
    }

    public Trie(TrieNode rootNode)=>root = rootNode;

   

}
