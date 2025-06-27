using YamlDotNet.RepresentationModel;

namespace JustLoaded.Data.Yaml;

public class YPath {
    
    public static YPath Parse(string path) {
        return new YPath();
    }
    
    public YPath() {
        //TODO
    }

    public YPath WithIndex(int index) {
        //TODO
    }

    public YPath WithKey(string key) {
        //TODO
    }
    
    public class Result {
        
    }
}

public interface IYPathComponent {

    public IEnumerable<YamlNode> Visit(YamlNode node);
    
    

}