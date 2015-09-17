using Microsoft.Practices.ObjectBuilder2;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{
    public class MockUnitStrategy : IBuilderStrategy
    {
        public MockUnitStrategy()
        {
            Behavior = MockBehavior.Loose;            
        }

        public MockBehavior Behavior { get; set; }

        public void PreBuildUp(IBuilderContext context)
        {
            var type = context.OriginalBuildKey.Type;
            if (type.IsInterface)
            {
                if (context.Existing == null)
                {
                    var mock = MockUnit.Get(type);
                    context.Existing = mock.Object;
                }
            }
        }

        public void PostBuildUp(IBuilderContext context)
        {
        }

        public void PreTearDown(IBuilderContext context)
        {
        }

        public void PostTearDown(IBuilderContext context)
        {
        }
    }
}