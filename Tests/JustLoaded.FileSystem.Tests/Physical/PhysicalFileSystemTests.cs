using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests.Physical;

/// <summary>
/// TODO fix absolute paths
/// </summary>
public class PhysicalFileSystemTests : FilesystemTester<PhysicalFilesystem, FilesystemTestSource> {
    
    protected override PhysicalFilesystem SetupFilesystem() {
        return new PhysicalFilesystem(FileHelper.ResourcesPath);
    }

    protected override void MakeFile(ModAssetPath fileName, string content) {
        FileHelper.CreateFile(fileName.path.ToPosix(), content);
    }

    [TearDown]
    public void TearDown2() {
        var path = new PosixPath(FileHelper.ResourcesPath.ToPosix());
        path.Delete(true);
    }
}
