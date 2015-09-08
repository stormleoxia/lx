using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester.Cloners
{
    class DelegateCloneBenchmark : IBenchmark
    {
        private ClassToClone _clone;
        public string Name { get; private set; }

        public DelegateCloneBenchmark()
        {
            Name = "Delegate Clone";
        }

        public void Init()
        {
            _clone = new ClassToClone();
            _clone.StringProperty = "MyName";
            _clone.IntegerProperty = -2;
            _clone.StringProperty2 = "AnotherData";
            _clone.IntegerProperty1 = 10;
            _clone.DoubleProperty = 23.5;
            _clone.DelegateClone();
        }

        public void Call()
        {
            _clone.DelegateClone();
        }

        public void Cleanup()
        {

        }
    }
}
