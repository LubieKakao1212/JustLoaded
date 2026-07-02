using YamlDotNet.Core.Events;

namespace JustLoaded.Serialization.Tree;

public class TreeScalar(string value) : TreeNodeBase, ITreeScalar {

    public string Value { get; set; } = value;

    public override IEnumerable<ParsingEvent> ToEvents() {
        yield return new Scalar(Value);
    }

}