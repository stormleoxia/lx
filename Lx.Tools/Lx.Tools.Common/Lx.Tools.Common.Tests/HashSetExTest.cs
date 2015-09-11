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
using System.Configuration;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class CollectionExTest
    {
        [Test]
        public void ToHashSetTest()
        {
            var list = new List<string> {"toto", "toto", "tutu"};
            var hash = list.ToHashSet();
            Assert.IsNotNull(hash);
            Assert.IsNotEmpty(hash);
            Assert.AreEqual(2, hash.Count);
            Assert.IsTrue(hash.Contains("tutu"));
            Assert.IsTrue(hash.Contains("toto"));
            Assert.IsFalse(hash.Contains("totu"));
        }

        [Test]
        public void ToDictionaryIgnoreDuplicatesTest()
        {
            var list = new List<string> { "toTo", "toto", "toto", "Toto", "tutut" };
            Dictionary<string, int> dic = list.ToDictionaryIgnoreDuplicates(x => x.ToLower(), x => x.Length);
            Assert.IsNotNull(dic);
            Assert.AreEqual(2, dic.Count);
            Assert.AreEqual(4, dic["toto"]);
            Assert.AreEqual(5, dic["tutut"]);
        }

        [Test]
        public void ToStackTest()
        {
            var list = new List<string> { "totu", "toto", "tutu" };
            Stack<string> stack = list.ToStack();
            Assert.IsNotNull(stack);
            Assert.AreEqual(3, stack.Count);
            Assert.AreEqual("tutu",stack.Pop());
            Assert.AreEqual("toto", stack.Pop());
            Assert.AreEqual("totu", stack.Pop());
        }
    }
}