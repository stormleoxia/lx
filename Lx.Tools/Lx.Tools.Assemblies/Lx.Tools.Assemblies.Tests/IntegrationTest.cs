using System;
using Lx.Tools.Assemblies.Compare;
using NUnit.Framework;

namespace Lx.Tools.Assemblies.Tests
{
    [TestFixture]
    public class IntegrationTest
    {
        [Test]
        public void LoadAssemblyTest()
        {
            var loader = new AssemblyLoader(new AppDomainExtractorFactory());
            var api = loader.ExtractApi("nunit.framework.dll");
            foreach (var n in api.GetNamespaces())
            {
                Console.WriteLine(n.ToString());
            }
        }
    }
}