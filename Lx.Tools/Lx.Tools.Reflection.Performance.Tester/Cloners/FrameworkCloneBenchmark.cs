using System;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Cloners
{
    class FrameworkCloneBenchmark : IBenchmark
    {
        private ClassToClone _clone;
        public string Name { get; private set; }

        public FrameworkCloneBenchmark()
        {
            Name = "MemberwiseClone Method";
        }

        public void Init()
        {
            _clone = new ClassToClone();
            _clone.StringProperty = "MyName";
            _clone.IntegerProperty = -2;
            _clone.StringProperty2 = "AnotherData";
            _clone.IntegerProperty1 = 10;
            _clone.DoubleProperty = 23.5;
            _clone.Clone();
        }

        public void Call()
        {
            _clone.Clone();
        }

        public void Cleanup()
        {
        }
    }
}
