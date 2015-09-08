#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

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

        private string Unixify(string path)
        {
            if (!path.Contains(":"))
            {
                return path.Replace('\\', '/');
            }
            return path.Split(':')[1].Replace('\\', '/');
        }

        [Test]
        public void CheckReferenceDirectoryConstructorProvideAbsolutePath()
        {
            var path = new UPath(_curDir, "..\\..");
            var info = new DirectoryInfo(_curDir);
            Assert.AreEqual(info.Parent.Parent.FullName, path.ToString());
        }

        [Test]
        public void CheckReferenceDirectoryConstructorProvideAbsolutePathWithUnixPaths()
        {
            var path = new UPath(_unixCurDir, "..\\..");
            var info = new DirectoryInfo(_curDir);
            var expected = Unixify(info.Parent.Parent.FullName).Replace('/', Path.DirectorySeparatorChar);
            Assert.AreEqual(expected, path.ToString());
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
        public void TestRelativePathInCurrentDir()
        {
            var path1 = new UPath("mydir\\dir2\\file.cs");
            var path2 = new UPath("mydir");
            Assert.AreEqual("dir2/file.cs".Replace('/', Path.DirectorySeparatorChar),
                path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativeToAbsolutePath()
        {
            var path1 = new UPath("../file.cs");
            var path2 = new UPath(_winCurDir);
            Assert.AreEqual("../file.cs".Replace('/', Path.DirectorySeparatorChar),
                path2.MakeRelativeUPath(path1).ToString());
        }

        [Test]
        public void TestRelativeToCurrentPath()
        {
            var path1 = new UPath("../file.cs");
            var path2 = new UPath(".");
            Assert.AreEqual(".." + Path.DirectorySeparatorChar + "file.cs", path2.MakeRelativeUPath(path1).ToString());
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

        [Test]
        public void TestWinVsUnixRoots()
        {
            var path1 = new UPath(_winCurDir.Replace('\\', '/') + "/root.cs");
            var path2 = new UPath(_winCurDir.Replace('/', '\\'));
            Assert.AreEqual("root.cs", path2.MakeRelativeUPath(path1).ToString());
        }
    }
}