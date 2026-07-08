using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace JustLoaded.Serialization.Tree;

public class TreeCollection(int capacity) : TreeNodeBase, ITreeCollection {

    public int Count => _children.Count;

    public IEnumerable<ITreeNode> Children => _children;

    private readonly List<ITreeNode> _children = new(capacity);

    public TreeCollection() : this(0) { }
    
    public ITreeNode GetChild(int idx) {
        return _children[idx];
    }

    public ITreeNode OrphanChild(int idx) {
        var child = GetChild(idx);
        _children.Remove(child);
        child.SetParentInternal(null);
        return child;
    }

    public void InsertChild(int idx, ITreeNode node) {
        node.AssertRoot();
        
        node.SetParentInternal(this);
        _children.Insert(idx, node);
    }

    public ITreeNode SwapChild(int idx, ITreeNode node) {
        var previous = OrphanChild(idx);
        InsertChild(idx, node);
        return previous;
    }

    public override IEnumerable<ParsingEvent> ToEvents() {
        yield return new SequenceStart(AnchorName.Empty, TagName.Empty, true, SequenceStyle.Any);
        foreach (var child in _children) {
            foreach (var evnt in child.ToEvents()) {
                yield return evnt;
            }
        }
        yield return new SequenceEnd();
    }

    public override ITreeNode DeepClone() {
        var result = new TreeCollection(Count);
        for (int i = 0; i < Count; i++) {
            result.InsertChild(i, _children[i].DeepClone());
        }
        return result;
    }
}