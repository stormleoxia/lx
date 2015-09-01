using System;
using System.Linq;
using Lx.Tools.Common.Assemblies;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class TypeDefinitionTest
    {

        [Test]
        public void CheckMembers()
        {
            var type = typeof (MyClass);
            TypeDefinition definition = new TypeDefinition(type);
            Assert.AreEqual(type.Namespace, definition.Namespace);
            Assert.AreEqual(type.Name, definition.Name);
            Assert.IsNotNull(definition.Members);
            Assert.AreEqual(17, definition.Members.Count);
        }

        [Test]
        public void CheckHiddenMemberAreStillHidden()
        {
            var type = typeof(MyClass);
            TypeDefinition definition = new TypeDefinition(type);
            Assert.AreEqual(type.Namespace, definition.Namespace);
            Assert.AreEqual(type.Name, definition.Name);
            Assert.IsNotNull(definition.Members);
            Assert.IsNull(definition.Members.FirstOrDefault(x => x.Signature.Contains("hidden")));
            Assert.IsNull(definition.Members.FirstOrDefault(x => x.Signature.Contains("Hidden")));
        }
    }


}
