using JustLoaded.Serialization.Tree;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization;

public class EnumeratorParser(IEnumerator<ParsingEvent> events) : IParser {

    public static EnumeratorParser FromTree(ITreeNode node) {
        return new EnumeratorParser(node.ToEvents().GetEnumerator());
    }
    
    public bool MoveNext() {
        return events.MoveNext();
    }

    public ParsingEvent? Current => events.Current;
    
}