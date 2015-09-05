using System;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourcesProviderTest
    {
        [Test]
        public void Dot45Test()
        {
            var projectFilePath = "x/y/z";
            var target = Targets.Net4Dot5;
            var console = new Mock<IConsole>(MockBehavior.Strict);
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            string[] sources = {"file1", "file2-net_4_5"};
            fileSystem.Setup(x => x.GetFiles("x\\y".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(sources);
            var nl = Environment.NewLine;
            var reader1 = new Mock<TextReader>();
            reader1.Setup(x => x.ReadToEnd()).Returns(@"file.txt" + nl + "file" + nl + "#include file3.src");
            fileSystem.Setup(x => x.OpenText("x\\y\\file2-net_4_5".ToPlatformPath())).Returns(reader1.Object);
            var reader2 = new Mock<TextReader>();
            reader2.Setup(x => x.ReadToEnd()).Returns(@"anotherfile.cs");
            fileSystem.Setup(x => x.OpenText("x\\y\\file3.src".ToPlatformPath())).Returns(reader2.Object);
            var provider = new SourcesProvider(projectFilePath, target, fileSystem.Object, console.Object);
            var files = provider.GetFiles();
            Assert.IsNotNull(files);
            Assert.IsNotEmpty(files);
            Assert.AreEqual("file.txt", files.FirstOrDefault(x => x.Contains("file.txt")));
            Assert.AreEqual("file", files.FirstOrDefault(x => x == "file"));
            Assert.AreEqual("anotherfile.cs", files.FirstOrDefault(x => x.Contains("anotherfile.cs")));
        }

        [Test]
        public void NoUniqueTest()
        {
            var projectFilePath = "x/y/z";
            var target = Targets.Net4Dot5;
            var console = new Mock<IConsole>(MockBehavior.Strict);
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            string[] sources = {"file1", "file2"};
            fileSystem.Setup(x => x.GetFiles("x\\y".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(sources);
            console.Setup(x => x.WriteLine(It.Is<string>(y => y.Contains("ERROR"))));
            var provider = new SourcesProvider(projectFilePath, target, fileSystem.Object, console.Object);
            var files = provider.GetFiles();
            Assert.IsNotNull(files);
            Assert.IsEmpty(files);
        }
    }

    public static class StringEx
    {
        public static string ToPlatformPath(this string path)
        {
            return path.Replace('/', Path.DirectorySeparatorChar).Replace('\\', Path.DirectorySeparatorChar);
        }
    }
}