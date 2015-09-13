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

using System.IO;
using Lx.Tools.Common.Paths;
using Lx.Tools.Common.Wrappers;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Paths
{
    [TestFixture]
    public class FilePathTest
    {
        private PathFactory _factory;
        private Mock<IFileSystem> _system;

        [SetUp]
        public void Setup()
        {
            _system = new Mock<IFileSystem>();
            _factory = new PathFactory(_system.Object, new PathConfiguration());
        }

        [Test]
        public void AbsolutePathTest()
        {
            var path = @"C:\Directory\SubDirectory\MyFile.txt";
            var filePath = _factory.Create(path);
            Assert.AreEqual("C", ((WinPath) filePath).Drive);
            Assert.IsNotNull(filePath.File);
            Assert.AreEqual("MyFile.txt", filePath.File.Path);
            Assert.IsNotNull(filePath.Parent);
            Assert.AreEqual(@"C:\Directory\SubDirectory\", filePath.Parent.Path);
            Assert.IsTrue(ReferenceEquals(filePath, filePath.Parent.Child));
            Assert.IsNotNull(filePath.Root);
            Assert.AreEqual(@"C:", filePath.Root.Path);
        }

        [Test, Ignore]
        public void RelativePathTest()
        {
            var path = @"Directory\MyFile.txt";
            var filePath = _factory.Create(path);
            Assert.AreEqual(string.Empty, ((WinPath) filePath).Drive);
            Assert.IsNotNull(filePath.File);
            Assert.AreEqual("MyFile.txt", filePath.File.Path);
            Assert.IsNotNull(filePath.Root);
        }

        [Test, Ignore]
        public void IntersectBottomUpTest()
        {
            var path1 = _factory.Create(@"C:\Dir1\Dir2\Dir3");
            var path2 = _factory.Create(@"C:\Dir4\Dir2\Dir3");
            var path3 = path1.Intersect(path2, PathIntersections.BottomUp);
            Assert.AreEqual(@"Dir2\Dir3", path3.Path);
        }

        [Test, Ignore]
        public void IntersectTopDownTest()
        {
            var path1 = _factory.Create(@"C:\Dir1\Dir2\Dir3");
            var path2 = _factory.Create(@"C:\Dir1\Dir2\Dir4");
            var path3 = path1.Intersect(path2, PathIntersections.TopDown);
            Assert.AreEqual(@"C:\Dir1\Dir2", path3.Path);
        }
    }
}