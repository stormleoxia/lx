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
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectSyncTest
    {
        [Test]
        public void UsageTest()
        {
            var projectPath = "project";
            var target = Targets.Net4Dot5;
            var projectItems = new HashSet<string> {"first", "second"};
            var sourceFiles = new HashSet<string> {"first", "third"};
            var factory = new Mock<IProjectFactory>(MockBehavior.Strict);
            var provider = new Mock<IProjectItemsProvider>(MockBehavior.Strict);
            provider.Setup(x => x.GetItems()).Returns(projectItems);
            factory.Setup(x => x.CreateProjectItemsProvider(projectPath)).Returns(provider.Object);
            var sourceProvider = new Mock<ISourcesProvider>(MockBehavior.Strict);
            sourceProvider.Setup(x => x.GetFiles()).Returns(sourceFiles);
            factory.Setup(x => x.CreateSourcesProvider(projectPath)).Returns(sourceProvider.Object);
            var comparer = new Mock<ISourceComparer>(MockBehavior.Strict);
            var comparison = new SourceComparison();
            comparer.Setup(x => x.Compare(projectItems, sourceFiles)).Returns(comparison);
            factory.Setup(x => x.CreateSourceComparer()).Returns(comparer.Object);
            var updater = new Mock<IProjectUpdater>(MockBehavior.Strict);
            updater.Setup(x => x.Update(comparison));
            factory.Setup(x => x.CreateProjectUpdater(projectPath)).Returns(updater.Object);
            var console = new Mock<IConsole>(MockBehavior.Strict);
            console.Setup(x => x.WriteLine(string.Empty));
            var projectSync = new ProjectSync(projectPath, target, console.Object, factory.Object);
            projectSync.Synchronize();
        }
    }
}