using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Common.Paths;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Paths
{
    [TestFixture]
    class PathPartFactoryTest
    {
        [Test]
        public void MakePartsWithWindowsAbsolutePathTest()
        {
            var input = @"C:\dir1\Dir2/test.txt";
            var factory = new PathPartFactory(new PathConfiguration());
            var res = factory.MakeParts(input);
            Assert.IsNotNull(res);
            Assert.AreEqual(7, res.Length);
            Assert.AreEqual("C:", res[0].RawValue);
            Assert.AreEqual(PathComponentKind.Root, res[0].Kind); 
            Assert.AreEqual("\\", res[1].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[1].Kind);
            Assert.AreEqual("dir1", res[2].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[2].Kind);
            Assert.AreEqual("\\", res[3].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[3].Kind);
            Assert.AreEqual("Dir2", res[4].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[4].Kind);
            Assert.AreEqual("/", res[5].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[5].Kind);
            Assert.AreEqual("test.txt", res[6].RawValue);
            Assert.AreEqual(PathComponentKind.File, res[6].Kind); 
        }

        [Test]
        public void MakePartsWithUnixAbsolutePathTest()
        {
            var input = @"/dir1/Dir2/test.txt";
            var factory = new PathPartFactory(new PathConfiguration());
            var res = factory.MakeParts(input);
            Assert.IsNotNull(res);
            Assert.AreEqual(6, res.Length);
            int i = 0;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Root, res[i].Kind);
            i++;
            Assert.AreEqual("dir1", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[i].Kind);
            i++;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[i].Kind);
            i++;
            Assert.AreEqual("Dir2", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Directory, res[i].Kind);
            i++;
            Assert.AreEqual("/", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.Separator, res[i].Kind);
            i++;
            Assert.AreEqual("test.txt", res[i].RawValue);
            Assert.AreEqual(PathComponentKind.File, res[i].Kind);
        }
    }
}
