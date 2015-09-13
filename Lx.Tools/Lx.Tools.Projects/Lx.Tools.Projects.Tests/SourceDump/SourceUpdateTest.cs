#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.SourceDump;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
{
    [TestFixture]
    public class SourceUpdateTest
    {
        private readonly PropertyInfo _propertyInfo = typeof (UPath).GetProperty("FSystem",
            BindingFlags.Static | BindingFlags.NonPublic);

        private string _curDir;
        private Mock<IFileSystem> _fileSystem;
        private string _unixRooted;
        private string _windowsRooted;

        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _propertyInfo.SetValue(null, _fileSystem.Object);
        }

        [TestFixtureSetUp]
        public void FixtureSetup()
        {
            _curDir = @"D:\Development\Applications\lx\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug";
            _windowsRooted =
                @"D:\Development\Applications\lx\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug";
            _unixRooted = @"/usr/home/dev/lx/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
        }

        [TestFixtureTearDown]
        public void Teardown()
        {
            _propertyInfo.SetValue(null, new FileSystem());
            _fileSystem.VerifyAll();
        }

        [Test]
        public void TestAbsolutePaths()
        {
            var winFile = @"..\winfile.cs";
            var unixfile = @"../unixfile.cs";
            _fileSystem.Setup(x => x.ResolvePath(winFile)).Returns((string) null);
            _fileSystem.Setup(x => x.ResolvePath(unixfile)).Returns((string) null);
            var dumper = new SourceDumper(_curDir, new HashSet<Option> {SourceDumperOptions.AbsolutePaths});
            var result =
                dumper.Dump(new List<string> {@"..\winfile.cs", "../unixfile.cs", _windowsRooted, _unixRooted}).ToList();
            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(winFile, result[0]);
            Assert.AreEqual(unixfile, result[1]);
            Assert.AreEqual(_windowsRooted, result[2]);
            Assert.AreEqual(_unixRooted, result[3]);
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
        public void TestRelativePathsOnUnix()
        {
            var unixRelative = "../../Lx.csproj";
            var unixRooted = Path.Combine(_unixRooted, unixRelative);

            var dumper = new SourceDumper(_unixRooted, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var result = dumper.Dump(new List<string> {unixRelative, unixRooted}).ToList();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(unixRelative.Replace('/', Path.DirectorySeparatorChar), result[0], "1: For " + unixRelative);
            Assert.AreEqual(unixRelative.Replace('/', Path.DirectorySeparatorChar), result[1], "2: For " + unixRooted);
        }

        [Test]
        public void TestRelativePathsOnWindows()
        {
            var winRelative = @"..\..\Lx.csproj";
            var winRooted = Path.Combine(_windowsRooted, winRelative);

            var dumper = new SourceDumper(_curDir, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var result = dumper.Dump(new List<string> {winRelative, winRooted}).ToList();

            Assert.IsNotNull(result);
            Assert.IsNotEmpty(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(winRelative.Replace('\\', Path.DirectorySeparatorChar), result[0], "1: For " + winRelative);
            Assert.AreEqual(winRelative.Replace('\\', Path.DirectorySeparatorChar), result[1], "2: For " + winRooted);
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
    }

    public static class PathEx
    {
        /// <summary>
        ///     Make the path use windows separator (backslash) instead of slashes.
        ///     Beware to not add Drive: at the beginning as it will make tests fail
        ///     since code check drive coherence.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string Windowsify(this string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, '\\');
        }

        public static string Unixify(this string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, '/');
        }
    }
}