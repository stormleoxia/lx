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
using System.Linq;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourcesProviderTest
    {
        [Test]
        public void Dot45Test()
        {
            var projectFilePath = "x/y/net_4_5-file2.csproj";
            var console = new Mock<IConsole>(MockBehavior.Strict);
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            string[] sources = {"file1", "file2-net_4_5"};
            fileSystem.Setup(x => x.GetFiles("x\\y".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(sources);
            var nl = Environment.NewLine;
            var reader1 = new Mock<TextReader>();
            //fileSystem.Setup(x => x.FileExists("x\\y\\file.txt".ToPlatformPath())).Returns(true);
            //fileSystem.Setup(x => x.FileExists("x\\y\\file".ToPlatformPath())).Returns(true);
            //fileSystem.Setup(x => x.FileExists("x\\y\\nonexisting".ToPlatformPath())).Returns(false);
            //fileSystem.Setup(x => x.FileExists("x\\y\\anotherfile.cs".ToPlatformPath())).Returns(true);
            //console.Setup(x => x.WriteLine("Source Not Found: x\\y\\nonexisting".ToPlatformPath()));
            reader1.Setup(x => x.ReadToEnd())
                .Returns(@"file.txt" + nl + "file" + nl + "nonexisting" + nl + "#include file3.src");
            fileSystem.Setup(x => x.OpenText("x\\y\\file2-net_4_5".ToPlatformPath())).Returns(reader1.Object);
            var reader2 = new Mock<TextReader>();
            reader2.Setup(x => x.ReadToEnd()).Returns(@"anotherfile.cs");
            fileSystem.Setup(x => x.OpenText("x\\y\\file3.src".ToPlatformPath())).Returns(reader2.Object);
            var provider = new SourcesProvider(projectFilePath, fileSystem.Object, console.Object);
            var files = provider.GetFiles();
            Assert.IsNotNull(files);
            Assert.IsNotEmpty(files);
            Assert.AreEqual("file.txt", files.FirstOrDefault(x => x.Contains("file.txt")));
            Assert.AreEqual("file", files.FirstOrDefault(x => x == "file"));
            Assert.AreEqual("anotherfile.cs", files.FirstOrDefault(x => x.Contains("anotherfile.cs")));
            fileSystem.VerifyAll();
            console.VerifyAll();
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void NoUniqueTest()
        {
            var projectFilePath = "x/y/z";
            var target = Targets.Net4Dot5;
            var console = new Mock<IConsole>(MockBehavior.Strict);
            var fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            string[] sources = {"file1", "file2"};
            fileSystem.Setup(x => x.GetFiles("x\\y".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(sources);
            console.Setup(x => x.WriteLine(It.Is<string>(y => y.Contains("ERROR"))));
            var provider = new SourcesProvider(projectFilePath, fileSystem.Object, console.Object);
            var files = provider.GetFiles();
            Assert.IsNotNull(files);
            Assert.IsEmpty(files);
            fileSystem.VerifyAll();
            console.VerifyAll();
        }
    }
}