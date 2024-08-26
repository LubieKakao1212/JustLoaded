using System.Collections;
using JustLoaded.Content;
using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public abstract class FilesystemTester<TFilesystem, TSource> where TSource : IFilesystemTestSource, new() where TFilesystem : IFilesystem {

    protected static IEnumerable SourceSingleFileFlat => new TSource().SourceSingleFileFlat;
    protected static IEnumerable SourceMultipleFilesFlat => new TSource().SourceGetMultipleFiles;
    
    protected static IEnumerable SourceListDirs => new TSource().SourceListDirs;
    protected static IEnumerable SourceListFilesShallow => new TSource().SourceListFilesShallow;
    

    protected TFilesystem fs;
    
    [SetUp]
    public void Setup() {
        fs = SetupFilesystem();
    }
    
    [TestCaseSource(nameof(SourceSingleFileFlat))]
    public void GetSingleFile(string fileName) {
        var fileContent = "A Cool File Content";
        
        MakeFile(fileName, fileContent);
        
        using var stream = fs.OpenFile(new ContentKey("", fileName));
        Assert.That(stream, Is.Not.Null);
        
        using var text = new StreamReader(stream);
        Assert.That(text.ReadLine(), Is.EqualTo(fileContent));
    }
    
    [TestCaseSource(nameof(SourceMultipleFilesFlat))]
    public void AddGetMultiple(IEnumerable<string> fileNames) {
        var files = fileNames.ToArray();
        
        for (int i = 0; i < files.Length; i++) {
            MakeFile(files[i], i.ToString());
        }
        
        for (int i = 0; i < files.Length; i++) {
            AssertFileContents(fs, new ContentKey("", files[i]), i.ToString());
        }
    }
    
    [TestCaseSource(nameof(SourceListDirs))]
    public void ListPaths(string listDir, string[] files, ContentKey[] expectedDirs) {
        foreach (var file in files) {
            MakeFile(file, file);
        }

        var dirsSet = new HashSet<ContentKey>(expectedDirs);
        foreach (var dir in fs.ListPaths(listDir)) {
            Assert.That(dirsSet, Contains.Item(dir));
            dirsSet.Remove(dir);
        }
        
        Assert.That(dirsSet, Is.Empty);
    }
    
    [TestCaseSource(nameof(SourceListFilesShallow))]
    public void ListFilesShallow(string listDir, string[] files, ContentKey[] expectedFiles) {
        DoListFiles(listDir, files, expectedFiles, "*", false);
    }
    
    protected abstract TFilesystem SetupFilesystem();
    protected abstract void MakeFile(string fileName, string content);
    
    
    protected void DoListFiles(string listDir, string[] files, ContentKey[] expectedFiles, string pattern, bool recursive) {
        foreach (var file in files) {
            MakeFile(file, file);
        }
        
        var dirsSet = new HashSet<ContentKey>(expectedFiles);
        foreach (var dir in fs.ListFiles(listDir, pattern, recursive)) {
            Assert.That(dirsSet, Contains.Item(dir));
            dirsSet.Remove(dir);
        }
        Assert.That(dirsSet, Is.Empty);
    }
    protected void AssertFileContents(TFilesystem vfs, ContentKey file, string expectedContent) {
        using var stream1 = vfs.OpenFile(file);
        Assert.That(stream1, Is.Not.Null);
        using var text = new StreamReader(stream1);
        Assert.That(text.ReadLine(), Is.EqualTo(expectedContent));
    }
}