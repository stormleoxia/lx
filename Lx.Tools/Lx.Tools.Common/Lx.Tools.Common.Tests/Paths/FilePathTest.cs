using System.IO;
using Lx.Tools.Common.Paths;
using Lx.Tools.Common.Wrappers;
using Moq;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Paths
{
    [TestFixture]
    public class FilePathTest
    {
        private PathFactory _factory;
        private Mock<IFileSystem> _system;

        [SetUp]
        public void Setup()
        {
            _system = new Mock<IFileSystem>();
            _factory = new PathFactory(_system.Object, new PathConfiguration());
        }

        [Test]
        public void AbsolutePathTest()
        {
            var path = @"C:\Directory\SubDirectory\MyFile.txt";
            var filePath = _factory.Create(path);
            Assert.AreEqual("C", ((WinPath)filePath).Drive);
            Assert.IsNotNull(filePath.File);
            Assert.AreEqual("MyFile.txt", filePath.File.Path);
            Assert.IsNotNull(filePath.Parent);
            Assert.AreEqual(@"C:\Directory\SubDirectory\", filePath.Parent.Path);
            Assert.IsTrue(ReferenceEquals(filePath, filePath.Parent.Child));
            Assert.IsNotNull(filePath.Root);
            Assert.AreEqual(@"C:", filePath.Root.Path);
        }

        [Test, Ignore]
        public void RelativePathTest()
        {
            var path = @"Directory\MyFile.txt";
            var filePath = _factory.Create(path);
            Assert.AreEqual(string.Empty, ((WinPath)filePath).Drive);
            Assert.IsNotNull(filePath.File);
            Assert.AreEqual("MyFile.txt", filePath.File.Path);
            Assert.IsNotNull(filePath.Root);
        }

        [Test, Ignore]
        public void IntersectBottomUpTest()
        {
            var path1 = _factory.Create(@"C:\Dir1\Dir2\Dir3");
            var path2 = _factory.Create(@"C:\Dir4\Dir2\Dir3");
            var path3 = path1.Intersect(path2, PathIntersections.BottomUp);
            Assert.AreEqual(@"Dir2\Dir3", path3.Path);
        }

        [Test, Ignore]
        public void IntersectTopDownTest()
        {
            var path1 = _factory.Create(@"C:\Dir1\Dir2\Dir3");
            var path2 = _factory.Create(@"C:\Dir1\Dir2\Dir4");
            var path3 = path1.Intersect(path2, PathIntersections.TopDown);
            Assert.AreEqual(@"C:\Dir1\Dir2", path3.Path);
        }
    }
}
