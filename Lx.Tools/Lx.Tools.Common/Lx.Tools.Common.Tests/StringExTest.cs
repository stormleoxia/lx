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
using System.IO;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class StringExTest
    {
        [Test]
        public void Test()
        {
            Assert.AreEqual("/usr/lib/myfile.lib".Replace('/', Path.DirectorySeparatorChar),
                "/usr/lib/myfile.lib".ToPlatformPath());
            Assert.AreEqual(@"\usr\lib\myfile.lib".Replace('\\', Path.DirectorySeparatorChar),
                @"\usr\lib\myfile.lib".ToPlatformPath());
        }

        [Test]
        public void SplitKeepDelimitersWithPathTest()
        {
            var res = StringEx.SplitKeepDelimiters(@"C:\Directory\Other/File.txt", new string[] {@"\", @"/"});
            Assert.AreEqual(7, res.Length);
            Assert.AreEqual("C:", res[0]);
            Assert.AreEqual(@"\", res[1]);
            Assert.AreEqual("Directory", res[2]);
            Assert.AreEqual("\\", res[3]);
            Assert.AreEqual("Other", res[4]);
            Assert.AreEqual("/", res[5]);
            Assert.AreEqual("File.txt", res[6]);
        }

        [Test]
        public void SplitKeepDelimitersWithPathWithLongDelimiterTest()
        {
            var input = @"//a///";
            var res = StringEx.SplitKeepDelimiters(input, new string[] { @"ab", @"/" });
            Assert.AreEqual(input, string.Concat(res));
        }

        [Test]
        public void SplitKeepDelimitersWithPathWithOverlappingDelimiterTest()
        {
            var input = @"abc";
            var res = StringEx.SplitKeepDelimiters(input, new string[] { @"ab", @"bc" });
            Assert.AreEqual(input, string.Concat(res));
            Assert.AreEqual(3, res.Length);
            Assert.AreEqual(string.Empty, res[0]);
            Assert.AreEqual("ab", res[1]);
            Assert.AreEqual("c", res[2]);
        }

        [Test]
        public void SplitKeepDelimitersWithSeveralInputTest()
        {
            var inputs = new List<KeyValuePair<string[], string>>()
            {
                new KeyValuePair<string[], string>(
                    new string[]{"ab", "bc"}, "abcdef"),
                new KeyValuePair<string[], string>(
                    new string[]{"abc", "def"}, "abcdef"),
                new KeyValuePair<string[], string>(
                    new string[]{"abc"}, "/abc/"),
                new KeyValuePair<string[], string>(
                    new string[]{"abc"}, "abc"),
                new KeyValuePair<string[], string>(
                    new string[]{"/"}, "/"),
                new KeyValuePair<string[], string>(
                    new string[]{"ab"}, "ab"),
                new KeyValuePair<string[], string>(
                    new string[]{"/"}, "////////"),
                new KeyValuePair<string[], string>(
                    new string[]{"/", @"\\"}, @"//\///\\//\Toto/\\"),
                new KeyValuePair<string[], string>(
                    new string[]{".", @".."}, @"..\/..//.\\...//...\T..oto/\\")
            };
            foreach (var input in inputs)
            {
                var res = StringEx.SplitKeepDelimiters(input.Value, input.Key);
                Assert.AreEqual(input.Value, string.Concat(res));
            }
        }
        
    }
}