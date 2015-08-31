using Lx.Tools.Common.Program;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests
{
    [TestFixture]
    public class UsageDefinitionTest
    {
        [Test]
        public void CheckNameIsTheProcessName()
        {
            UsageDefinition definition = new UsageDefinition();            
            Assert.AreEqual("Lx.Tools.Common.Tests", definition.ExeName);
        }

        [Test]
        public void CheckArgumentsAreUsable()
        {
            UsageDefinition definition = new UsageDefinition();
            definition.Arguments.Add(new Arguments { Name = "MyFirstArg" });
            Assert.IsNotNull(definition.Arguments);
            Assert.AreEqual(1, definition.Arguments.Count);
            Assert.IsNotNull(definition.Arguments[0]);
            Assert.AreEqual("MyFirstArg", definition.Arguments[0].Name);
        }
    }
}
