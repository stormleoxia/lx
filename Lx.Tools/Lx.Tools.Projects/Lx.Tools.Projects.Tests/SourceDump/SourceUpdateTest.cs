using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Program;
using Lx.Tools.Projects.SourceDump;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
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
                _winCurDir = "C:" + _winCurDir.Replace('/', '\\');
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
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {SourceDumperOptions.UnixPaths});
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
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {SourceDumperOptions.WindowsPaths});
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
            Console.WriteLine("Current Directory: " + _curDir);
            
            string firstFoundFile = null;
            var higherDirectory = _curDir;
            var relativePath = string.Empty;
            while (firstFoundFile == null)
            {
                relativePath = (string.IsNullOrEmpty(relativePath)) ? ".." : Path.Combine(relativePath, "..");
                higherDirectory = Path.Combine(higherDirectory, "..");
                firstFoundFile = Directory.GetFiles(higherDirectory).FirstOrDefault();
            }
            var fileName = Path.GetFileName(firstFoundFile);
            var unixRelative = Path.Combine(relativePath, fileName).Replace("\\", "/");
            var winRelative = Path.Combine(relativePath, fileName).Replace('/', '\\');
            var winRooted = firstFoundFile.WindowsifyPath();
            var unixRooted = firstFoundFile.Replace("\\", "/");

            var dumper = new SourceDumper(_curDir, new HashSet<Option> { SourceDumperOptions.RelativePaths });
            var result = dumper.Dump(new List<string> {unixRelative, winRelative, winRooted, unixRooted}).ToList();


            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(unixRelative.Replace('/', Path.DirectorySeparatorChar), result[0], "1: For " + unixRelative);
            Assert.AreEqual(winRelative.Replace('\\', Path.DirectorySeparatorChar), result[1], "2: For " + winRelative);
            Assert.AreEqual(winRelative.Replace('\\', Path.DirectorySeparatorChar), result[2], "3: For " + winRooted);
            Assert.AreEqual(unixRelative.Replace('/', Path.DirectorySeparatorChar), result[3], "4: For " + unixRooted);
        }

        [Test]
        public void TestAbsolutePaths()
        {
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {SourceDumperOptions.AbsolutePaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(@"..\winfile.cs", result[0]);
            Assert.AreEqual(@"../unixfile.cs", result[1]);
            Assert.AreEqual(_windowsRooted, result[2]);
            Assert.AreEqual(_unixRooted, result[3]);
        }

    }

    public static class PathEx
    {
        /// <summary>
        /// Make the path use windows separator (backslash) instead of slashes.
        /// Beware to not add Drive: at the beginning as it will make tests fail
        /// since code check drive coherence.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string WindowsifyPath(this string path)
        {
            var res = path.Replace('/', '\\');
            return res;
        }
    }
}