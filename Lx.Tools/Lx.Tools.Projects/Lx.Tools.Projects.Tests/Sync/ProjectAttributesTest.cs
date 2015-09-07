using Lx.Tools.Projects.Sync;
using NUnit.Framework;

namespace Lx.Tools.Projects.Tests.Sync
{
    [TestFixture]
    public class ProjectAttributesTest
    {
        [Test]
        public void DotNet4Dot5Test()
        {
            var att = new ProjectAttributes(@"/usr/lib/net_4_5-build-System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.Net4Dot5);
            Assert.IsTrue(att.HasScope);
            Assert.AreEqual(att.Scope, Scopes.build);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void NoScopeTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/basic-System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.Basic);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void NoScopeAllTargetTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core.csproj");
            Assert.IsFalse(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }

        [Test]
        public void IsTestInNamespaceTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core.Test.csproj");
            Assert.IsTrue(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core.Test");
        }

        [Test]
        public void IsTestInScopeTest()
        {
            var att = new ProjectAttributes(@"/usr/lib/System.Core-test.csproj");
            Assert.IsTrue(att.IsTest);
            Assert.AreEqual(att.Target, Targets.All);
            Assert.IsFalse(att.HasScope);
            Assert.AreEqual(att.Name, "System.Core");
        }
    }
}
