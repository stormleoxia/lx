using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Projects.Sync;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class SourceComparisonTest
    {
        [Test]
        public void UsageTest()
        {
            var sourceComparison = new SourceComparison();
            sourceComparison.Add(new MissingFileInProject("project"));
            sourceComparison.Add(new MissingFileInSource("source"));
            
            Assert.IsNotNull(sourceComparison.MissingFilesInProject);
            Assert.AreEqual(1, sourceComparison.MissingFilesInProject.Count());
            var res = sourceComparison.MissingFilesInProject.FirstOrDefault();
            Assert.IsNotNull(res);
            Assert.AreEqual("project", res.Path);
            Assert.IsTrue(res.ToString().Contains("project"));

            Assert.IsNotNull(sourceComparison.MissingFilesInSource);
            Assert.AreEqual(1, sourceComparison.MissingFilesInSource.Count());
            var sourceMissing = sourceComparison.MissingFilesInSource.FirstOrDefault();
            Assert.IsNotNull(sourceMissing);
            Assert.AreEqual("source", sourceMissing.Path);
            Assert.IsTrue(sourceMissing.ToString().Contains("source"));

            Assert.IsTrue(sourceComparison.ToString().Contains("source"));
            Assert.IsTrue(sourceComparison.ToString().Contains("project"));
            var lines = sourceComparison.ToString().Split('\n');
            var lineNumber = lines.Length;
            if (lines.Last() == String.Empty)
            {
                lineNumber--;
            }
            Assert.AreEqual(2, lineNumber);
        }
    }
}
