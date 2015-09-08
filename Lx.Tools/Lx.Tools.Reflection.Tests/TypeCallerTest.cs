using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace Lx.Tools.Reflection.Unit.Tests
{

    [TestFixture]
    public class TypeCallerTest
    {
        [Test]
        public void TypeCallShouldCallTheTypedDelegate()
        {
            var instance = new MethodOwner();
            object parameter = new MorphedParameter();
            var caller = new TypeCaller(instance, "MethodToCall", parameter.GetType());
            caller.Call(parameter);
        }
    }

    public class MorphedParameter
    {
    }

    public class MethodOwner
    {       

        public void MethodToCall<T>(T parameter)
        {
            this.Result = "MethodCall<" + typeof(T).Name + ">";
        }

        public string Result { get; set; }
    }

    public class TypeCaller
    {
        private readonly MethodInfo _method;

        public TypeCaller(MethodOwner instance, string methodtocall, Type type)
        {
            var methods = instance.GetType().GetMethods().Where(x => x.Name == methodtocall);
            foreach (var method in methods)
            {
                if (method.GetGenericArguments().Length == 1)
                {
                    _method = method.MakeGenericMethod(new[] {type});
                }
            }
        }

        public void Call(object parameter)
        {
            if (_method != null)
            {
                
            }
        }
    }
}
