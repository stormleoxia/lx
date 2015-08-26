using System;
using System.IO;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class UPathTest
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
        public void TestRelativePath()
        {
            var path1 = new UPath("../file.cs");
            var path2 = new UPath("..\\");
            Assert.AreEqual("file.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativePath2()
        {
            var path1 = new UPath("..\\mydir\\file.cs");
            var path2 = new UPath("../mydir/");
            Assert.AreEqual("file.cs", path2.MakeRelativeUPath(path1).ToString());
        }


        [Test]
        public void TestRelativePathInCurrentDir()
        {
            var path1 = new UPath("mydir\\dir2\\file.cs");
            var path2 = new UPath("mydir");
            Assert.AreEqual("dir2/file.cs".Replace('/', Path.DirectorySeparatorChar), path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativePath3()
        {
            var path1 = new UPath("..\\dir1\\dir2\\file.cs");
            var path2 = new UPath("../dir1/dir2/dir3");
            Assert.AreEqual(".." + Path.DirectorySeparatorChar + "file.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativePath4()
        {
            var path1 = new UPath("..\\dir1\\dir2\\dir3\\file.cs");
            var path2 = new UPath("../dir1/dir2/dir3");
            Assert.AreEqual("file.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativePath5()
        {
            var path1 = new UPath("..\\dir1\\dir2\\dir3\\file.cs");
            var path2 = new UPath("../dir1/dir2");
            Assert.AreEqual("dir3" + Path.DirectorySeparatorChar + "file.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativeToAbsolutePath()
        {
            var path1 = new UPath("../file.cs");
            var path2 = new UPath(_winCurDir);
            Assert.AreEqual("../file.cs".Replace('/', Path.DirectorySeparatorChar), path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativeToCurrentPath()
        {
            var path1 = new UPath("../file.cs");
            var path2 = new UPath(".");
            Assert.AreEqual(".." + Path.DirectorySeparatorChar + "file.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestAbsoluteToAbsolutePath()
        {
            var path1 = new UPath(new FileInfo("../../UPath.cs").FullName);
            var path2 = new UPath(new DirectoryInfo("../../").FullName);
            Assert.AreEqual("UPath.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestAbsoluteToAbsolutePath2()
        {
            var path1 = new UPath(new FileInfo("../../Properties/AssemblyInfo.cs").FullName);
            var path2 = new UPath(new DirectoryInfo("../../").FullName);
            Assert.AreEqual("Properties" + Path.DirectorySeparatorChar + "AssemblyInfo.cs",
                path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestAbsoluteToAbsolutePathWithADifferentDirectory()
        {
            var path1 = new UPath(new FileInfo("../../UPath.cs").FullName);
            var path2 = new UPath(new DirectoryInfo("../../Properties").FullName);
            Assert.AreEqual(".." + Path.DirectorySeparatorChar + "UPath.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        /// <summary>
        ///     Implicit reference to current directory is not resolvable
        /// </summary>
        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void TestRelativeToRelativePathNotResolvable()
        {
            var path1 = new UPath("FileNotExists.cs");
            var path2 = new UPath("../NotExisting");
            var res = path2.MakeRelativeUPath(path1).ToString();
        }

        /// <summary>
        ///     No relative path exists between to different drives
        /// </summary>
        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void TestAbsoluteWithDifferentDrives()
        {
            var path1 = new UPath(@"D:\FileNotExists.cs");
            var path2 = new UPath(@"C:\NotExisting");
            var res = path2.MakeRelativeUPath(path1).ToString();
        }

        [Test]
        public void TestWinVsUnixRoots()
        {
            var path1 = new UPath(_winCurDir.Replace('\\', '/') + "/root.cs");
            var path2 = new UPath(_winCurDir.Replace('/', '\\'));
            Assert.AreEqual("root.cs", path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void CheckReferenceDirectoryConstructorProvideAbsolutePath()
        {
            var path = new UPath(_curDir, "..\\..");
            var info = new DirectoryInfo(_curDir);
            Assert.AreEqual(info.Parent.Parent.FullName, path.ToString());
        }
    }
}