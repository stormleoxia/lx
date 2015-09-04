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