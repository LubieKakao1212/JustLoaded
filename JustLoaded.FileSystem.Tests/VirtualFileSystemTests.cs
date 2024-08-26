using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public class VirtualFilesystemTests : FilesystemTester<VirtualFilesystem, FilesystemTestSource> {

    protected override VirtualFilesystem SetupFilesystem() {
        return new VirtualFilesystem();
    }

    protected override void MakeFile(string fileName, string content) {
        fs.AddFile(fileName, content);
    }
    
    [TestCase("/", new[] { "dir/file1", "file2"}, new[] { "file2" })]
    [TestCase("/", new[] { "dir/file1", "dir/file2"}, new string[] { })]
    [TestCase("/dir", new[] { "dir/file1", "dir2/file2"}, new[] { "dir/file1" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/dir2/file2"}, new string[] { })]
    [TestCase("/a/dir", new[] { "a/dir/file1", "a/dir/file2"}, new[] { "a/dir/file1", "a/dir/file2" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/file2"}, new[] { "a/file2" })]
    public void ListFilesShallow(string listDir, string[] files, string[] expectedFiles) {
        DoListFiles(listDir, files, expectedFiles, "*", false);
        //Assert.Fail("TODO");
    }
    
    [TestCase("/", new[] { "dir/file1", "file2"}, new[] { "file2", "dir/file1" })]
    [TestCase("/dir", new[] { "dir/file1", "dir2/file2"}, new[] { "dir/file1" })]
    [TestCase("/", new[] { "dir/dir1/dir2/file1"}, new[] { "dir/dir1/dir2/file1" })]
    public void ListFilesRecursive(string listDir, string[] files, string[] expectedFiles) {
        DoListFiles(listDir, files, expectedFiles, "*", true);
    }

    [TestCase("*", new[] { "file.json", "file.txt", "file", "apple.txt" }, new[] { "file.json", "file.txt", "file", "apple.txt" })]
    [TestCase("*.*", new[] { "file.json", "file.txt", "file", "apple.txt" }, new[] { "file.json", "file.txt", "apple.txt" })]
    [TestCase("*.txt", new[] { "file.json", "file.txt", "file", "apple.txt" }, new[] { "file.txt", "apple.txt" })]
    [TestCase("file.*", new[] { "file.json", "file.txt", "file", "apple.txt" }, new[] { "file.json", "file.txt" })]
    public void ListFilesPattern(string pattern, string[] files, string[] expectedFiles) {
        DoListFiles("", files, expectedFiles, pattern, false);
    }
    
    private void DoListFiles(string listDir, string[] files, string[] expectedFiles, string pattern, bool recursive) {
        var vfs = new VirtualFilesystem();
        
        foreach (var file in files) {
            vfs.AddFile(file, file);
        }
        
        var dirsSet = new HashSet<string>(expectedFiles);
        foreach (var dir in vfs.ListFiles(listDir, pattern, recursive)) {
            Assert.That(dirsSet, Contains.Item(dir.path));
            dirsSet.Remove(dir.path);
        }
        Assert.That(dirsSet, Is.Empty);
    }
    
    private void AssertFileContents(VirtualFilesystem vfs, string file, string expectedContent) {
        using var stream1 = vfs.OpenFile(file);
        Assert.That(stream1, Is.Not.Null);
        using var text = new StreamReader(stream1);
        Assert.That(text.ReadLine(), Is.EqualTo(expectedContent));
    }
    
}