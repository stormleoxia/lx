using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Cloners
{
    public class ManualCloneBenchmark : IBenchmark
    {
        private ClassToClone _clone;
        public string Name { get; private set; }

        public ManualCloneBenchmark()
        {
            Name = "Manual Clone";
        }

        public void Init()
        {
            _clone = new ClassToClone();
            _clone.StringProperty = "MyName";
            _clone.IntegerProperty = -2;
            _clone.StringProperty2 = "AnotherData";
            _clone.IntegerProperty1 = 10;
            _clone.DoubleProperty = 23.5;
        }

        public void Call()
        {
            _clone.ManualClone();
        }

        public void Cleanup()
        {
            
        }
    }
}
