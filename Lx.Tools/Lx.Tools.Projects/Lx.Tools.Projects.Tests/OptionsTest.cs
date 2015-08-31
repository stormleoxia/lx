using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class OptionsTest
    {
        private SourceDumperOptions _options;

        [SetUp]
        public void Setup()
        {
            _options = new SourceDumperOptions();
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
