using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public class VirtualFilesystemTests {
    
    [SetUp]
    public void Setup() {
    }

    [TestCase("coolFile")]
    [TestCase("dir/coolFile")]
    [TestCase("dir1/coolFile")]
    public void AddGetFileFlat(string fileName) {
        var fileContent = "A Cool File Content";
        
        var vfs = new VirtualFilesystem();
        vfs.AddFile(fileName, fileContent);

        using var stream = vfs.OpenFile(fileName);
        Assert.That(stream, Is.Not.Null);
        
        using var text = new StreamReader(stream);
        Assert.That(text.ReadLine(), Is.EqualTo(fileContent));
    }

    [TestCase("file1", "file2")]
    [TestCase("dir/file1", "file2")]
    [TestCase("file1", "dir/file2")]
    [TestCase("dir/file1", "dir/file2")]
    public void AddGetMultiple(string file1, string file2) {
        var file1Content = "content1";
        var file2Content = "content2";
        
        var vfs = new VirtualFilesystem();
        vfs.AddFile(file1, file1Content);
        vfs.AddFile(file2, file2Content);

        AssertFileContents(vfs, file1, file1Content);
        AssertFileContents(vfs, file2, file2Content);
    }

    [TestCase("/", new[] { "dir/file1", "file2"}, new[] { "dir" })]
    [TestCase("/", new[] { "dir/file1", "dir/file2"}, new[] { "dir" })]
    [TestCase("/", new[] { "dir/file1", "dir2/file2"}, new[] { "dir", "dir2" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/dir2/file2"}, new[] { "a/dir", "a/dir2" })]
    [TestCase("/a", new[] { "a/dir/file1", "a/dir2/file2"}, new[] { "a/dir", "a/dir2" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/file2"}, new[] { "a/dir" })]
    [TestCase("/a/b/", new[] { "a/b/file1", "a/c/file2", "a/b/dir/file2"}, new[] { "a/b/dir" })]
    public void ListPaths(string listDir, string[] files, string[] expectedDirs) {
        var vfs = new VirtualFilesystem();
        
        foreach (var file in files) {
            vfs.AddFile(file, "");
        }

        var dirsSet = new HashSet<string>(expectedDirs);
        foreach (var dir in vfs.ListPaths(listDir)) {
            Assert.That(dirsSet, Contains.Item(dir.path));
            dirsSet.Remove(dir.path);
        }
        
        Assert.That(dirsSet, Is.Empty);
    }

    [TestCase("/", new[] { "dir/file1", "file2"}, new[] { "file2" })]
    [TestCase("/", new[] { "dir/file1", "dir/file2"}, new string[] { })]
    [TestCase("/dir", new[] { "dir/file1", "dir2/file2"}, new[] { "dir/file1" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/dir2/file2"}, new string[] { })]
    [TestCase("/a/dir", new[] { "a/dir/file1", "a/dir/file2"}, new[] { "a/dir/file1", "a/dir/file2" })]
    [TestCase("/a/", new[] { "a/dir/file1", "a/file2"}, new[] { "a/file2" })]
    public void ListFilesShallow(string listDir, string[] files, string[] expectedFiles) {
        DoListFiles(listDir, files, expectedFiles, "*", false);
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