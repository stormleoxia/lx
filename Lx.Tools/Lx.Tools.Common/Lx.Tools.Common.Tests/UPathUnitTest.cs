using System;
using System.Runtime.CompilerServices;
using Lx.Tools.Common.Wrappers;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class UPathUnitTest
    {
        private Mock<IFileSystem> _fileSystem;


        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>();
            UPath.FSystem = _fileSystem.Object;
        }


        [TearDown]
        public void Teardown()
        {
            UPath.FSystem = new FileSystem();
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void ReproduceUnixRelativePathNotOnTheSameDrive()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"C:\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            UPath path1 = new UPath(unixPath);
            UPath path2 = new UPath(winPath);
            var res = path1.MakeRelativeUPath(path2);
        }
    }
}
