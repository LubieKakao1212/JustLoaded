namespace JustLoaded.Serialization.Tree;

public interface ITreeCollection : ITreeNode {

    int Count { get; }

    ITreeNode this[int idx] => GetChild(idx);

    IEnumerable<ITreeNode> Children { get; }

    ITreeNode GetChild(int idx);

    /// <summary>
    /// Returns the child at <see cref="idx"/> and removes it, making it its own root
    /// </summary>
    /// <param name="idx"></param>
    /// <returns></returns>
    ITreeNode OrphanChild(int idx);
    
    void InsertChild(int idx, ITreeNode node);

    ITreeNode SwapChild(int idx, ITreeNode node);
}