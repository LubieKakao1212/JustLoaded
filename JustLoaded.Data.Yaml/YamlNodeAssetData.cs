using JustLoaded.Filesystem;
using YamlDotNet.RepresentationModel;

namespace JustLoaded.Data.Yaml;

public class YamlNodeAssetData : IYamlAssetData {
    
    public ModAssetPath Path { get; }
    
    public YamlNode Yaml { get; }

    public YamlNodeAssetData(ModAssetPath path, YamlNode yaml) {
        Path = path;
        Yaml = yaml;
    }

    public IAssetData Clone() {
        return new YamlNodeAssetData(Path, Yaml.Clone());
    }
}