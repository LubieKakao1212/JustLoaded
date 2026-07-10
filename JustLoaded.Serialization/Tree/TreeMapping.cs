using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public class TreeMapping : TreeNodeBase, ITreeMapping {

    public int Count => _children.Count;

    public IEnumerable<ITreeNode> Children => _children.Select(pair => pair.Value);

    public IEnumerable<KeyValuePair<string, ITreeNode>> ChildrenByKey => _children;

    private readonly List<KeyValuePair<string, ITreeNode>> _children = new();

    #region Collection
    
    public ITreeNode GetChild(int idx) {
        return _children[idx].Value;
    }

    public ITreeNode OrphanChild(int idx) {
        var child = GetChild(idx);
        _children.RemoveAt(idx);
        child.SetParentInternal(null);
        return child;
    }

    public void InsertChild(int idx, ITreeNode node) {
        throw new ApplicationException("Insertion without a key not supported in a mapping");
    }

    public ITreeNode SwapChild(int idx, ITreeNode node) {
        node.AssertRoot();
        
        var pair = _children[idx];

        node.SetParentInternal(this);
        var newPair = new KeyValuePair<string, ITreeNode>(pair.Key, node);
        _children[idx] = newPair;

        var existing = pair.Value;
        existing.SetParentInternal(null);
        return existing;
    }
    
    #endregion
    
    #region Mapping
    
    public ITreeNode GetChild(string key) {
        return GetChild(IndexOfKey(key));
    }

    public ITreeNode OrphanChild(string key) {
        return OrphanChild(IndexOfKey(key));
    }

    public void InsertChild(string key, ITreeNode node) {
        node.AssertRoot();
        TreeHelper.AssertNotPresent(IndexOfKey(key), key);
        
        node.SetParentInternal(this);
        var pair = new KeyValuePair<string, ITreeNode>(key, node);
        _children.Add(pair);
    }

    public void InsertChild(string key, int idx, ITreeNode node) {
        node.AssertRoot();
        TreeHelper.AssertNotPresent(IndexOfKey(key), key);
        
        node.SetParentInternal(this);
        var pair = new KeyValuePair<string, ITreeNode>(key, node);
        _children.Insert(idx, pair);
    }

    public ITreeNode SwapChild(string key, ITreeNode node) {
        return SwapChild(IndexOfKey(key), node);
    }

    #endregion

    public override ITreeNode DeepClone() {
        var result = new TreeMapping();
        for (int i = 0; i < Count; i++) {
            var pair = _children[i];
            result.InsertChild(pair.Key, pair.Value.DeepClone());
        }
        return result;
    }

    public override IEnumerable<ParsingEvent> ToEvents() {
        yield return new MappingStart();

        foreach (var pair in _children) {
            yield return new Scalar(pair.Key);
            foreach (var evnt in pair.Value.ToEvents()) {
                yield return evnt;
            }
        }
        
        yield return new MappingEnd();
    }

    private int IndexOfKey(string key) {
        return _children.FindIndex(pair => pair.Key == key);
    }
}