using Lx.Tools.Common.Paths;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Paths
{
    [TestFixture]
    public class PathUtilityTest
    {
        [Test]
        public void CleanUpFileDoubleDotsTest()
        {
            var path = @"..\../";
            var input = path.Split('/', '\\');
            var res = PathUtility.CleanUp(input);
            var str = string.Join("/", res);
            Assert.AreEqual("../..", str);
        }
 
        [Test]
        public void CleanUpFileSimpleDotTest()
        {
            var path = @"./Dir/.\Dir2/../";
            var res = PathUtility.CleanUp(path.Split('/', '\\'));
            var str = string.Join("/", res);
            Assert.AreEqual("Dir", str);
        }
    
        [Test]
        public void CleanUpFileDoubleSlashTest()
        {
            var path = @"./Dir//.\\Dir2///\\..///Dir3";
            var input = path.Split('/', '\\');
            var res = PathUtility.CleanUp(input);
            var str = string.Join("/", res);
            Assert.AreEqual("Dir/Dir3", str);
        }

        [Test, Ignore]
        public void CleanUpDotRelativeTest()
        {
            var path = @"./.";
            var input = path.Split('/', '\\');
            var res = PathUtility.CleanUp(input);
            var str = string.Join("/", res);
            Assert.AreEqual(".", str);
        }


        [Test]
        public void InferPathTypeFileTest()
        {
            // Infers with file on root (Unix)
            var path = @"/file.txt";
            var res = PathUtility.InferPathType(path, PathUtility.CleanUp(path.Split('/', '\\')));
            Assert.AreEqual(PathTypes.File, res);

            // Infers with file in directory (Unix)
            path = @"/directory/file.txt";
            res = PathUtility.InferPathType(path, PathUtility.CleanUp(path.Split('/', '\\')));
            Assert.AreEqual(PathTypes.File, res);


            // Infers with file on root (Windows)
            path = @"C:\file.txt";
            res = PathUtility.InferPathType(path, PathUtility.CleanUp(path.Split('/', '\\')));
            Assert.AreEqual(PathTypes.File, res);

            // Infers with file in directory (Windows)
            path = @"C:\directory\file.txt";
            res = PathUtility.InferPathType(path, PathUtility.CleanUp(path.Split('/', '\\')));
            Assert.AreEqual(PathTypes.File, res);     
        }


        [Test]
        public void InferPathTypeRootTest()
        {
            // Infers with root (Unix)
            var path = @"/";
            var input = PathUtility.CleanUp(path.Split('/', '\\'));
            var res = PathUtility.InferPathType(path, input);
            Assert.AreEqual(PathTypes.Root, res);

            // Infers with root variant (Unix)
            path = @"//";
            input = PathUtility.CleanUp(path.Split('/', '\\'));
            res = PathUtility.InferPathType(path, input);
            Assert.AreEqual(PathTypes.Root, res);


            // Infers with root (Windows)
            path = @"C:\";
            input = PathUtility.CleanUp(path.Split('/', '\\'));
            res = PathUtility.InferPathType(path, input);
            Assert.AreEqual(PathTypes.Root, res);

            // Infers with root variant (Windows)
            path = @"C:\\";
            input = PathUtility.CleanUp(path.Split('/', '\\'));
            res = PathUtility.InferPathType(path, input);
            Assert.AreEqual(PathTypes.Root, res);
        }
    }
}
