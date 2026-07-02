using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public abstract class TreeNodeBase : ITreeNode {

    public ITreeNode? Parent { get; private set; }

    public object? Companion { get; set; }

    public abstract IEnumerable<ParsingEvent> ToEvents();

    public void SetParentInternal(ITreeNode? newParent) {
        Parent = newParent;
    }
}