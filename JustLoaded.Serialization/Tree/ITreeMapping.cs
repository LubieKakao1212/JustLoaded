namespace JustLoaded.Serialization.Tree;

public interface ITreeMapping : ITreeCollection {
    
    ITreeNode this[string key] => GetChild(key);
    
    IEnumerable<KeyValuePair<string, ITreeNode>> ChildrenByKey { get; }
    
    ITreeNode GetChild(string key);

    /// <summary>
    /// Returns the child under <see cref="key"/> and removes it, making it its own root
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    ITreeNode OrphanChild(string key);
    
    void InsertChild(string key, ITreeNode node);
    void InsertChild(string key, int idx, ITreeNode node);

    ITreeNode SwapChild(string key, ITreeNode node);
    
}