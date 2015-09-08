using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lx.Tools.Reflection.Performance.Tester.Cloners
{
    public class ClassToClone
    {
        public string StringProperty { get; set; }
        public int IntegerProperty { get; set; }
        public string StringProperty2 { get; set; }
        public int IntegerProperty1 { get; set; }
        public double DoubleProperty { get; set; }
        
        private static readonly InstanceCopier<ClassToClone> _copier = new InstanceCopier<ClassToClone>();

        static ClassToClone()
        {
            
        }

        public ClassToClone()
        {
        }

        public ClassToClone Clone()
        {
            return (ClassToClone)MemberwiseClone();
        }

        public ClassToClone DelegateClone()
        {
            var instance = new ClassToClone();
            _copier.Copy(this, instance);
            return instance;
        }
    }
}
