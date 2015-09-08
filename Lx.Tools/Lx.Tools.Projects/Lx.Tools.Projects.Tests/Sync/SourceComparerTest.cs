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


        [Test]
        public void CompareComplexDifferencesTest()
        {
            var comparer = new SourceComparer();
            var comparison = comparer.Compare(new HashSet<string> { "First", "thirD", " second", @"..\fourth" },
                new HashSet<string> { "Third", "./First", "Second ", "../FourTH" });
            Assert.IsNotNull(comparison);
            var projectMissing = comparison.MissingFilesInProject.ToArray();
            Assert.IsEmpty(projectMissing);
            var sourceMissing = comparison.MissingFilesInSource.ToArray();
            Assert.IsEmpty(sourceMissing);
        }
    }
}