using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public class PhysicalFileSystemTests : FilesystemTester<PhysicalFilesystem, FilesystemTestSource> {
    
    protected override PhysicalFilesystem SetupFilesystem() {
        return new PhysicalFilesystem(FileHelper.ResourcesPath);
    }

    protected override void MakeFile(string fileName, string content) {
        FileHelper.CreateFile(fileName, content);
    }

    [TearDown]
    public void TearDown2() {
        Directory.Delete(FileHelper.ResourcesPath, true);
    }
}
