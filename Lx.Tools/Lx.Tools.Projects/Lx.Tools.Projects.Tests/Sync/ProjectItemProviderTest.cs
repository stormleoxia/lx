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
using System.Linq;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectItemProviderTest
    {
        [Test]
        public void UsageTest()
        {
            var project = new Mock<IProject>(MockBehavior.Strict);
            var projectItem1 = new Mock<ISyncProjectItem>();
            projectItem1.Setup(x => x.EvaluatedInclude).Returns("..\\first");
            var projectItem2 = new Mock<ISyncProjectItem>();
            projectItem2.Setup(x => x.EvaluatedInclude).Returns("../second/third");
            var projectItem3 = new Mock<ISyncProjectItem>();
            projectItem3.Setup(x => x.EvaluatedInclude).Returns("../first");
            var projectItems = new List<ISyncProjectItem> {projectItem1.Object, projectItem2.Object};
            project.Setup(x => x.GetItems("Compile")).Returns(projectItems);
            var itemProvider = new ProjectItemsProvider(project.Object);
            var res = itemProvider.GetItems();
            Assert.IsNotNull(res);
            Assert.AreEqual(2, res.Count);
            Assert.AreEqual("../first", res.FirstOrDefault(x => x.Contains("first")));
            Assert.AreEqual("../second/third", res.FirstOrDefault(x => x.Contains("second")));
        }
    }
}