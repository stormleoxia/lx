using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class OptionsTest
    {

        [Test]
        public void GetOptionsShouldDetectUnixPath()
        {
            var arguments = new[] {"--unix-paths", "--myfile"};
            var options = Options.GetOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(Options.UnixPaths));
            Assert.IsNull(arguments[0]);
            Assert.IsNotNull(arguments[1]);
        }


        [Test]
        public void GetOptionsShouldDetectWinPath()
        {
            var arguments = new[] { "-unix-paths", "--windows-paths" };
            var options = Options.GetOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(Options.WindowsPaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }


        [Test]
        public void GetOptionsShouldDetectRelativePath()
        {
            var arguments = new[] { "-unix-paths", "--relative-paths" };
            var options = Options.GetOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(Options.RelativePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectAbsolutePath()
        {
            var arguments = new[] { "-unix-paths", "--absolute-paths" };
            var options = Options.GetOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(Options.AbsolutePaths));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }

        [Test]
        public void GetOptionsShouldDetectOutputFile()
        {
            var arguments = new[] { "myfiles", "--output-file" };
            var options = Options.GetOptions(arguments);
            Assert.IsNotNull(options);
            Assert.AreEqual(1, options.Count);
            Assert.IsTrue(options.Contains(Options.OutputFile));
            Assert.IsNotNull(arguments[0]);
            Assert.IsNull(arguments[1]);
        }
    }
}
