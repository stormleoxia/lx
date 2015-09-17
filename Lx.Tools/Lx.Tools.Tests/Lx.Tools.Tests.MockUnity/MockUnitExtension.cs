using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{

    public class MockUnitExtension : UnityContainerExtension
    {
        private readonly MockUnitStrategy _strategy;

        public MockBehavior Behavior
        {
            get { return _strategy.Behavior; }
            set { _strategy.Behavior = value; }
        }

        internal MockUnitExtension()
        {
            _strategy = new MockUnitStrategy();
        }

        protected override void Initialize()
        {   Context.Strategies.Add(_strategy, UnityBuildStage.PreCreation);
        }
    }
}
