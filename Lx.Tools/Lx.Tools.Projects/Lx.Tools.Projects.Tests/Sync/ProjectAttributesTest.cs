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

using Lx.Tools.Projects.Sync;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectAttributesTest
    {
        [Test]
        public void DotNet4Dot5Test()
        {
            var att = new ProjectAttributes(@"/usr/lib/net_4_5-build-System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.Net4Dot5);
            Assert.IsTrue(att.HasScope);
            Assert.AreEqual(att.Scope, Scopes.build);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void IsTestInNamespaceTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core.Test.csproj");
            Assert.IsTrue(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core.Test");
        }

        [Test]
        public void IsTestInScopeTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core-test.csproj");
            Assert.IsTrue(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void NoScopeAllTargetTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void NoScopeTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/basic-System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.Basic);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }
    }
}