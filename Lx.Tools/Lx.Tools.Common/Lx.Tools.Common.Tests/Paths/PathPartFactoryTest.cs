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
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Common.Paths;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Paths
{
    [TestFixture]
    internal class PathPartFactoryTest
    {
        [Test]
        public void MakePartsWithWindowsAbsolutePathTest()
        {
            var input = @"C:\dir1\Dir2/test.txt";
            var factory = new PathPartFactory(new PathConfiguration{DirectorySeparator = "/", AltDirectorySeparator = @"\"});
            var res = factory.MakeParts(input);
            Assert.IsNotNull(res);
            Assert.AreEqual(7, res.Length);
            Assert.AreEqual("C:", res[0].RawValue);
            Assert.AreEqual(PathComponentKind.Root, res[0].Kind);
            Assert.AreEqual("\\", res[1].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[1].Kind);
            Assert.AreEqual("dir1", res[2].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[2].Kind);
            Assert.AreEqual("\\", res[3].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[3].Kind);
            Assert.AreEqual("Dir2", res[4].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[4].Kind);
            Assert.AreEqual("/", res[5].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[5].Kind);
            Assert.AreEqual("test.txt", res[6].RawValue);
            Assert.AreEqual(PathComponentKind.File, res[6].Kind);
        }

        [Test]
        public void MakePartsWithUnixAbsolutePathTest()
        {
            var input = @"/dir1/Dir2/test.txt";
            var factory = new PathPartFactory(new PathConfiguration());
            var res = factory.MakeParts(input);
            Assert.IsNotNull(res);
            Assert.AreEqual(6, res.Length);
            var i = 0;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Root, res[i].Kind);
            i++;
            Assert.AreEqual("dir1", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[i].Kind);
            i++;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[i].Kind);
            i++;
            Assert.AreEqual("Dir2", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[i].Kind);
            i++;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[i].Kind);
            i++;
            Assert.AreEqual("test.txt", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.File, res[i].Kind);
        }
    }
}