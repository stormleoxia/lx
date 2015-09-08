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

using System;
using System.IO;
using Lx.Tools.Common;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourceFileFinderTest
    {
        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _fileSystem.VerifyAll();
        }

        private Mock<IFileSystem> _fileSystem;

        [Test]
        public void AllTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "basic_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/Lx.Tools.Common.csproj", _fileSystem.Object);
            var sourceFound = sourceFileFinder.FindSourcesFile();
            Assert.AreEqual("Lx.Tools.Common.dll.sources", sourceFound);
        }

        [Test]
        public void Net45Test()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.dll.sources",
                    "basic_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            var sourceFound = sourceFileFinder.FindSourcesFile();
            Assert.AreEqual("net_4_5_Lx.Tools.Common.dll.sources", sourceFound);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void NoUniqueSourceTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.dll.sources",
                    "net_4_5_build_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            sourceFileFinder.FindSourcesFile();
        }

        [Test]
        public void ReproduceXammacNet45Bug()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "xammac_net_4_5_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            var sourceFound = sourceFileFinder.FindSourcesFile();
            Assert.AreEqual("Lx.Tools.Common.dll.sources", sourceFound);
        }

        [Test]
        public void ScopeTest()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "build_Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/build-Lx.Tools.Common.csproj", _fileSystem.Object);
            var sourceFound = sourceFileFinder.FindSourcesFile();
            Assert.AreEqual("build_Lx.Tools.Common.dll.sources", sourceFound);
        }

        [Test]
        public void TryWithXammacNet45Test()
        {
            _fileSystem.Setup(x => x.GetFiles("x/y/z".ToPlatformPath(), "*.sources", SearchOption.TopDirectoryOnly))
                .Returns(new[]
                {
                    "Lx.Tools.Common.dll.sources",
                    "net_4_5_Lx.Tools.Common.Test.dll.sources",
                    "net_4_5_Lx.Tools.Common.dll.sources",
                    "xammac_net_4_5-Lx.Tools.Common.dll.sources"
                });
            var sourceFileFinder = new SourceFileFinder(@"x/y/z/net_4_5-Lx.Tools.Common.csproj", _fileSystem.Object);
            var sourceFound = sourceFileFinder.FindSourcesFile();
            Assert.AreEqual("net_4_5_Lx.Tools.Common.dll.sources", sourceFound);
        }
    }
}