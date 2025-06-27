using JustLoaded.Filesystem;
using YamlDotNet.RepresentationModel;

namespace JustLoaded.Data.Yaml;

public class YamlDocumentAssetData : IYamlAssetData {
    private readonly YamlDocument _document;

    public YamlDocumentAssetData(ModAssetPath path, YamlDocument document) {
        _document = document;
        Path = path;
    }

    public ModAssetPath Path { get; }

    public YamlDocument YamlDocument => _document;

    public YamlNode Yaml => _document.RootNode;
    
    public IAssetData Clone() {
        return new YamlDocumentAssetData(Path, new YamlDocument(Yaml.Clone()));
    }
    
}