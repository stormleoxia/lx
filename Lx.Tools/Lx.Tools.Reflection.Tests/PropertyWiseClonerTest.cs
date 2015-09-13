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

using NUnit.Framework;

namespace Lx.Tools.Reflection.Unit.Tests
{
    [TestFixture]
    public class PropertyWiseClonerTest
    {
        [Test]
        public void UsageTest()
        {
            var container = new MyPropertyContainer
            {
                IntegerProperty = 10,
                StringProperty = "MyValue"
            };
            var clone = container.PropertyWiseClone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(container.IntegerProperty, clone.IntegerProperty);
            Assert.AreEqual(container.StringProperty, clone.StringProperty);
            var clone2 = container.PropertyWiseClone();
            Assert.IsNotNull(clone);
            Assert.AreEqual(container.IntegerProperty, clone.IntegerProperty);
            Assert.AreEqual(container.StringProperty, clone.StringProperty);
            Assert.IsFalse(ReferenceEquals(clone, clone2));
        }
    }

    public class MyPropertyContainer
    {
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
    }
}