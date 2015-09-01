using System.Collections.Generic;
using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Sync;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectSyncTest
    {
        [Test]
        public void UsageTest()
        {
            var projectPath = "project";
            var target = Targets.Net4Dot5;
            var projectItems = new HashSet<string>{ "first", "second" };
            var sourceFiles = new HashSet<string>{"first", "third" };
            var factory = new Mock<IProjectFactory>(MockBehavior.Strict);
            var provider = new Mock<IProjectItemsProvider>(MockBehavior.Strict);
            provider.Setup(x => x.GetItems()).Returns(projectItems);
            factory.Setup(x => x.CreateProjectItemsProvider(projectPath, target)).Returns(provider.Object);
            var sourceProvider = new Mock<ISourcesProvider>(MockBehavior.Strict);
            sourceProvider.Setup(x => x.GetFiles()).Returns(sourceFiles);
            factory.Setup(x => x.CreateSourcesProvider(projectPath, target)).Returns(sourceProvider.Object);
            var comparer = new Mock<ISourceComparer>(MockBehavior.Strict);
            var comparison = new SourceComparison();            
            comparer.Setup(x => x.Compare(projectItems, sourceFiles)).Returns(comparison);
            factory.Setup(x => x.CreateSourceComparer()).Returns(comparer.Object);
            var updater = new Mock<IProjectUpdater>(MockBehavior.Strict);
            updater.Setup(x => x.Update(comparison));
            factory.Setup(x => x.CreateProjectUpdater(projectPath)).Returns(updater.Object);
            var console = new Mock<IConsole>(MockBehavior.Strict);
            console.Setup(x => x.WriteLine(string.Empty));
            ProjectSync projectSync = new ProjectSync(projectPath, target, console.Object, factory.Object);
            projectSync.Synchronize();
        }
    }
}
