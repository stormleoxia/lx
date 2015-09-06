using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.SourceDump;
using Lx.Tools.Projects.Tests.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
{
    [TestFixture]
    public class SourceDumperUnitTest
    {
        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _propertyInfo.SetValue(null, _fileSystem.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _propertyInfo.SetValue(null, new FileSystem());
        }

        private Mock<IFileSystem> _fileSystem;

        private readonly PropertyInfo _propertyInfo = typeof (UPath).GetProperty("FSystem",
            BindingFlags.Static | BindingFlags.NonPublic);

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ExceptionTest()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"C:\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            var dumper = new SourceDumper(unixPath, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var res = dumper.Dump(new List<string> {winPath}).ToArray();
            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual(winPath, res.FirstOrDefault());
        }

        [Test]
        public void UnixTest()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            var dumper = new SourceDumper(unixPath, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var res = dumper.Dump(new List<string> {winPath}).ToArray();
            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count());
			Assert.AreEqual(@"..\..\Lx.Tools.Projects.Tests.csproj".ToPlatformPath(), res.FirstOrDefault());
            _fileSystem.VerifyAll();
        }
    }
}