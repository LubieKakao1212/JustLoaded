using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests.Relative.Simple;

public class SimpleRelativeFilesystemTests : RelativeFilesystemTester<Source> {

    private VirtualFilesystem vfs;
    protected override RelativeFilesystem SetupFilesystem() {
        vfs = new VirtualFilesystem();

        return new RelativeFilesystem(vfs, "prefix".AsPath());
    }

    protected override void MakeFile(ModAssetPath fileName, string content) {
        vfs.AddFile(fileName.path, content);
    }
}

public class Source : RelativeFilesystemTestSourceBase {

    public override IEnumerable<(ModAssetPath file, ModAssetPath query)> GetSingleFileRelativeSource {
        get {
            
        }
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListDirsSource {
        get;
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesShallowSource {
        get;
    }

    public override IEnumerable<(ModAssetPath query, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesRecursiveSource {
        get;
    }

    public override IEnumerable<(string pattern, ModAssetPath[] files, ModAssetPath[] results)> GetListFilesPatternSource {
        get;
    }
}
