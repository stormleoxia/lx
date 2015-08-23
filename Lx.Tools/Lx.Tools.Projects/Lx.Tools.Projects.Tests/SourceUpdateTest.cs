using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Projects.SourceDump;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests
{
    [TestFixture]
    public class SourceUpdateTest
    {
        private string _curDir;
        private string _unixCurDir;
        private string _unixRooted;
        private string _winCurDir;
        private string _windowsRooted;

        [TestFixtureSetUp]
        public void Setup()
        {
            _curDir = Directory.GetCurrentDirectory();
            _winCurDir = _curDir;
            if (!_winCurDir.Contains(":"))
            {
                _unixCurDir = _winCurDir;
                _winCurDir = "C:" + _winCurDir;
            }
            else
            {
                _unixCurDir = _winCurDir.Split(':')[1].Replace('\\', '/');
            }
            _windowsRooted = _winCurDir + @"\mywin_file.cs";
            _unixRooted = _unixCurDir + "/root.cs";
        }

        [Test]
        public void TestNoOption()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option>());
            var result = dumper.Dump(new List<string> {"file1.cs", "file2.cs"}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("file1.cs", result[0]);
            Assert.AreEqual("file2.cs", result[1]);
        }

        [Test]
        public void TestUnixPaths()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {Options.UnixPaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(@"../winfile.cs", result[0]);
            Assert.AreEqual("../unixfile.cs", result[1]);
            Assert.AreEqual(_windowsRooted.Replace("\\", "/"), result[2]);
            Assert.AreEqual(_unixRooted, result[3]);
        }

        [Test]
        public void TestWinPaths()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {Options.WindowsPaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(@"..\winfile.cs", result[0]);
            Assert.AreEqual(@"..\unixfile.cs", result[1]);
            Assert.AreEqual(_windowsRooted, result[2]);
            Assert.AreEqual(_unixRooted.Replace('/', '\\'), result[3]);
        }

        [Test]
        public void TestRelativePaths()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {Options.RelativePaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(@"..\winfile.cs", result[0]);
            Assert.AreEqual("../unixfile.cs", result[1]);
            Assert.AreEqual(@"mywin_file.cs", result[2]);
            Assert.AreEqual(@"root.cs", result[3]);
        }

        [Test]
        public void TestAbsolutePaths()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {Options.AbsolutePaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(@"..\winfile.cs", result[0]);
            Assert.AreEqual(@"../unixfile.cs", result[1]);
            Assert.AreEqual(_windowsRooted, result[2]);
            Assert.AreEqual(_unixRooted.Replace('/', '\\'), result[3]);
        }
    }
}