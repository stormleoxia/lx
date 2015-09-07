using System;
using System.IO;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourceFileFinderTest
    {
        private Mock<IFileSystem> _fileSystem;

        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _fileSystem.VerifyAll();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void NoUniqueSourceTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "net_4_5_Lx.Tools.Common.dll.sources", 
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.dll.sources",
                    "net_4_5_build_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            sourceFileFinder.FindSourcesFile();
        }

        [Test]
        public void Net45Test()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "net_4_5_Lx.Tools.Common.dll.sources", 
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.dll.sources",
                    "basic_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            sourceFileFinder.FindSourcesFile();
        }

        [Test]
        public void AllTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources", 
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "basic_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/Lx.Tools.Common.csproj", _fileSystem.Object);
            sourceFileFinder.FindSourcesFile();
        }

        [Test]
        public void ScopeTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources", 
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "build_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/build-Lx.Tools.Common.csproj", _fileSystem.Object);
            sourceFileFinder.FindSourcesFile();
        }

    }
}
