namespace JustLoaded.FileSystem.Tests;

public static class FileHelper {

    public static readonly string ResourcesPath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Resources");
    
    public static void CreateFile(string fileName, string content) {
        var filePath = Path.Combine(ResourcesPath, fileName);
        var directory = Path.GetDirectoryName(filePath);
        Assert.That(directory, Is.Not.Null, () => "Test init failed, invalid file path");
        Directory.CreateDirectory(directory);
        File.WriteAllText(filePath, content);
    }
}