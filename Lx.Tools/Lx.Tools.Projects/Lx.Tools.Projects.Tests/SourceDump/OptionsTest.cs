using System.Runtime.InteropServices;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.SourceDump;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
{
    [TestFixture]
    public class SourceDumperOptionsTest
    {
        private SourceDumperOptions _options;
        private Mock<IEnvironment> _environment;
        private Mock<IConsole> _console;

        [SetUp]
        public void Setup()
        {
            _environment = new Mock<IEnvironment>();
            _console = new Mock<IConsole>();
            _options = new SourceDumperOptions(_environment.Object, _console.Object);
        }


        [Test]
        public void GetOptionsShouldDetectUnixPath()
        {
            var arguments = new[] {"--unix-paths", "--myfile"};
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.UnixPaths));
            Assert.IsNull(arguments[0]);
            Assert.IsNotNull(arguments[1]);
        }


        [Test]
        public void GetOptionsShouldDetectWinPath()
        {
            var arguments = new[] { "-unix-paths", "--windows-paths" };
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.WindowsPaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }


        [Test]
        public void GetOptionsShouldDetectRelativePath()
        {
            var arguments = new[] { "-unix-paths", "--relative-paths" };
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.RelativePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectAbsolutePath()
        {
            var arguments = new[] { "-unix-paths", "--absolute-paths" };
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.AbsolutePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectOutputFile()
        {
            var arguments = new[] { "myfiles", "--output-file" };
            var options = _options.ParseOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(SourceDumperOptions.OutputFile));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }
    }
}
