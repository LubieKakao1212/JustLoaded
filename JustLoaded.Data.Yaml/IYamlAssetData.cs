using YamlDotNet.RepresentationModel;

namespace JustLoaded.Data.Yaml;

public interface IYamlAssetData : IAssetData {

    YamlNode Yaml { get; }

}