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

using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Reference;
using Lx.Tools.Tests.MockUnity;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.References
{
    [TestFixture]
    public class ReferenceAdderTest : MockUnitTestFixture
    {
        [Test]
        public void UsageTest()
        {
            const string fileName = "mycsproj.csproj";
            const string reference = "System.Data";
            MockUnit.Extension.Behavior = MockBehavior.Strict;
            var manager = Container.Resolve<ReferenceAdder>();
            var mock = MockUnit.Get<IFileSystem>();
            mock.Setup(x => x.DirectoryExists(fileName)).Returns(false);
            mock.Setup(x => x.FileExists(fileName)).Returns(true);
            mock.Setup(x => x.DirectoryExists(reference)).Returns(false);
            mock.Setup(x => x.FileExists(reference)).Returns(false);
            var project = MockUnit.Get<IProject>();
            project.Setup(x => x.GetItems("Reference")).Returns(new IProjectItem[0]);
            project.Setup(x => x.AddItem("Reference", reference, null));
            project.Setup(x => x.Save());
            var console = MockUnit.Get<IConsole>();
            console.Setup(x => x.WriteLine(It.IsAny<string>()));
            Assert.IsNotNull(manager);
            manager.AddReference(new[] {null, fileName, null, reference});
        }
    }
}