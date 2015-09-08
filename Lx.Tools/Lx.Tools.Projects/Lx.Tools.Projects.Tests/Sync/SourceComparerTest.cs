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

using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Projects.Sync;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourceComparerTest
    {
        [Test]
        public void CompareComplexDifferencesTest()
        {
            var comparer = new SourceComparer();
            var comparison = comparer.Compare(new HashSet<string> {"First", "thirD", " second", @"..\fourth"},
                new HashSet<string> {"Third", "./First", "Second ", "../FourTH"});
            Assert.IsNotNull(comparison);
            var projectMissing = comparison.MissingFilesInProject.ToArray();
            Assert.IsEmpty(projectMissing);
            var sourceMissing = comparison.MissingFilesInSource.ToArray();
            Assert.IsEmpty(sourceMissing);
        }

        [Test]
        public void CompareUsageTest()
        {
            var comparer = new SourceComparer();
            var comparison = comparer.Compare(new HashSet<string> {"first", "second"},
                new HashSet<string> {"first", "third"});
            Assert.IsNotNull(comparison);
            var projectMissing = comparison.MissingFilesInProject.ToArray();
            Assert.IsNotNull(projectMissing);
            Assert.AreEqual(1, projectMissing.Length);
            Assert.AreEqual("third", projectMissing.First().Path);
            Assert.IsTrue(projectMissing.First().ToString().Contains("third"));
            var sourceMissing = comparison.MissingFilesInSource.ToArray();
            Assert.IsNotNull(sourceMissing);
            Assert.AreEqual(1, sourceMissing.Length);
            Assert.AreEqual("second", sourceMissing.First().Path);
            Assert.IsTrue(sourceMissing.First().ToString().Contains("second"));
        }
    }
}