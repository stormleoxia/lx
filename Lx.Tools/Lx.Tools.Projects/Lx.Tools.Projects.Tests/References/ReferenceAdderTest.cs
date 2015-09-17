using Lx.Tools.Common.Wrappers;
using Lx.Tools.Projects.Reference;
using Lx.Tools.Tests.MockUnity;
using Microsoft.Practices.Unity;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.References
{
    [TestFixture]
    public class ReferenceAdderTest : MockUnitTestFixture
    {
        [Test]
        public void UsageTest()
        {
            const string fileName = "mycsproj.csproj";
            const string reference = "System.Data";
            MockUnit.Extension.Behavior = MockBehavior.Strict;
            var manager = Container.Resolve<ReferenceAdder>();
            var mock = MockUnit.Get<IFileSystem>();
            mock.Setup(x => x.DirectoryExists(fileName)).Returns(false);
            mock.Setup(x => x.FileExists(fileName)).Returns(true);
            mock.Setup(x => x.DirectoryExists(reference)).Returns(false);
            mock.Setup(x => x.FileExists(reference)).Returns(false);
            var project = MockUnit.Get<IProject>();
            project.Setup(x => x.GetItems("Reference")).Returns(new IProjectItem[0]);
            project.Setup(x => x.AddItem("Reference", reference, null));
            project.Setup(x => x.Save());
            var console = MockUnit.Get<IConsole>();
            console.Setup(x => x.WriteLine(It.IsAny<string>()));
            Assert.IsNotNull(manager);
            manager.AddReference(new []{ null, fileName, null, reference});
        }
    }
}
