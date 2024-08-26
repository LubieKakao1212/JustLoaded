using JustLoaded.Filesystem;

namespace JustLoaded.FileSystem.Tests;

public class FilesystemUtilTests {
    
    [TestCase("path", "path")]
    [TestCase("/path", "path")]
    [TestCase("path/", "path")]
    [TestCase("path/..", "")]
    [TestCase("..", "..")]
    [TestCase("../path", "../path")]
    [TestCase("path/../path1", "path1")]
    [TestCase("./path", "path")]
    [TestCase(".", "")]
    [TestCase("./", "")]
    [TestCase(".path", ".path")]
    [TestCase("..path", "..path")]
    [TestCase("path..", "path..")]
    public void CollapsePathTest(string path, string expectedCollapsed) {
        path = path.CollapsePath();

        Assert.That(path, Is.EqualTo(expectedCollapsed));
    }

    [TestCase("..", null, true)]
    [TestCase("../path", null, true)]
    [TestCase("path/../..", null, true)]
    [TestCase("path", "path", false)]
    [TestCase("path/..", "", false)]
    public void AbsoluteCollapsePathTest(string path, string expected, bool shouldThrow) {
        if (shouldThrow) {
            Assert.Throws<DirectoryNotFoundException>(() => {
                path = path.CollapseAbsolutePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }
        else {
            Assert.DoesNotThrow(() => {
                path = path.CollapseAbsolutePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }
    }

    
    [TestCase("path.", null, true)]
    [TestCase("path..", null, true)]
    [TestCase("path", "path", false)]
    public void CollapseAbsoluteFilePathTest(string path, string expected, bool shouldThrow) {
        if (shouldThrow) {
            Assert.Throws<FileNotFoundException>(() => {
                path = path.CollapseAbsoluteFilePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }
        else {
            Assert.DoesNotThrow(() => {
                path = path.CollapseAbsoluteFilePath();
                Assert.That(path, Is.EqualTo(expected));
            });
        }
    }

}