using JustLoaded.Serialization.Tree;
using YamlDotNet.Serialization;

namespace JustLoaded.Serialization.Yaml;

public class YamlTreeParser : ITreeParser {

    private readonly IDeserializer _deserializer = new DeserializerBuilder()
        .WithTreeDeserialization()
        .Build();
    
    public ITreeNode ParseTree(Stream stream) {
        return _deserializer.Deserialize<ITreeNode>(new StreamReader(stream));
    }
}