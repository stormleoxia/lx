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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lx.Tools.Common;
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.SourceDump;
using Lx.Tools.Projects.Tests.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.SourceDump
{
    [TestFixture]
    public class SourceDumperUnitTest
    {
        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>(MockBehavior.Strict);
            _propertyInfo.SetValue(null, _fileSystem.Object);
        }

        [TearDown]
        public void Teardown()
        {
            _propertyInfo.SetValue(null, new FileSystem());
            _fileSystem.VerifyAll();
        }

        private Mock<IFileSystem> _fileSystem;

        private readonly PropertyInfo _propertyInfo = typeof (UPath).GetProperty("FSystem",
            BindingFlags.Static | BindingFlags.NonPublic);

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ExceptionTest()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"C:\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            var dumper = new SourceDumper(unixPath, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var res = dumper.Dump(new List<string> {winPath}).ToArray();
            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual(winPath, res.FirstOrDefault());
        }

        [Test]
        public void UnixTest()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            var dumper = new SourceDumper(unixPath, new HashSet<Option> {SourceDumperOptions.RelativePaths});
            var res = dumper.Dump(new List<string> {winPath}).ToArray();
            Assert.IsNotNull(res);
            Assert.AreEqual(1, res.Count());
            Assert.AreEqual(@"..\..\Lx.Tools.Projects.Tests.csproj".ToPlatformPath(), res.FirstOrDefault());
        }
    }
}