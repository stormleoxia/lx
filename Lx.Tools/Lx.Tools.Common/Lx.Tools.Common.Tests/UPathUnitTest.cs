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
using Lx.Tools.Common.Wrappers;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class UPathUnitTest
    {
        [SetUp]
        public void Setup()
        {
            _fileSystem = new Mock<IFileSystem>();
            UPath.FSystem = _fileSystem.Object;
        }

        [TearDown]
        public void Teardown()
        {
            UPath.FSystem = new FileSystem();
        }

        private Mock<IFileSystem> _fileSystem;

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void ReproduceUnixRelativePathNotOnTheSameDrive()
        {
            var unixPath =
                @"/usr/local/teamcity-agent/work/6c86e2555ed64afc/Lx.Tools/Lx.Tools.Projects/Lx.Tools.Projects.Tests/bin/Debug";
            var winPath =
                @"C:\usr\local\teamcity-agent\work\6c86e2555ed64afc\Lx.Tools\Lx.Tools.Projects\Lx.Tools.Projects.Tests\bin\Debug\..\..\Lx.Tools.Projects.Tests.csproj";
            var path1 = new UPath(unixPath);
            var path2 = new UPath(winPath);
            var res = path1.MakeRelativeUPath(path2);
        }
    }
}