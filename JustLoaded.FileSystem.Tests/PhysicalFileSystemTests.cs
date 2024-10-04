using JustLoaded.Filesystem;
using PathLib;

namespace JustLoaded.FileSystem.Tests;

public class PhysicalFileSystemTests : FilesystemTester<PhysicalFilesystem, FilesystemTestSource> {
    
    protected override PhysicalFilesystem SetupFilesystem() {
        return new PhysicalFilesystem(new PurePosixPath(FileHelper.ResourcesPath));
    }

    protected override void MakeFile(ModAssetPath fileName, string content) {
        FileHelper.CreateFile(fileName.path.ToPosix(), content);
    }

    [TearDown]
    public void TearDown2() {
        Directory.Delete(FileHelper.ResourcesPath, true);
    }
}
