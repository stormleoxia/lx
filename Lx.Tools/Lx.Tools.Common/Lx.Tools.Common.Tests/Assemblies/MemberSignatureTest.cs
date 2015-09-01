using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using Lx.Tools.Common.Assemblies;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class MemberSignatureTest
    {
        [Test]
        public void MemberTest()
        {
            var signature = new MemberSignature();
            signature.Signature = "MySign";
            Assert.AreEqual("MySign", signature.Signature);
            Assert.AreEqual("MySign", signature.ToString());
        }

        [Test]
        public void PropertyMemberInfoTest()
        {
            var memberInfo = typeof(MyClass).GetMember("MyIntegerMember").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Property System.Int32 MyIntegerMember{get; set;}", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }


        [Test]
        public void MethodMemberInfoTest()
        {
            var memberInfo = typeof(MyClass).GetMember("MyMethod").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void MyMethod()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof(MyClass).GetMember("MyMethodWithParameters").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void MyMethodWithParameters(System.Int32 first, System.String second)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof(MyClass).GetMember("MyMethodWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.String MyMethodWithReturnType()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }


        [Test]
        public void GenericMethodMemberInfoTest()
        {
            var memberInfo = typeof(MyClass).GetMember("GenericMethod").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void GenericMethod<T>()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof(MyClass).GetMember("GenericMethodWithGenericParameter").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void GenericMethodWithGenericParameter<T>(T parameter)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof(MyClass).GetMember("GenericMethodWithGenericParameterWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method T GenericMethodWithGenericParameterWithReturnType<T>(T parameter)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof(MyClass).GetMember("GenericMethodWithGenericParametersWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method TZ GenericMethodWithGenericParametersWithReturnType<TX,TY,TZ>(TX parameter1, TY parameter2, TZ parameter3)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }

        [Test]
        public void CheckConstructorPublicAreCorrectlyConstructed()
        {
            var memberInfo = typeof (MyClass).GetConstructors().FirstOrDefault(x => !x.GetParameters().Any());
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual(".ctor()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());


            memberInfo = typeof(MyClass).GetConstructors().FirstOrDefault(x => x.GetParameters().Any());
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual(".ctor(System.Int32 parameter1, System.String parameter2)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }
    }

    public class MyClass
    {
        public int _myField;
        private int _myHiddenField;
        protected int _myProtectedField;

        public static int _myStaticField;

        public int MyIntegerMember { get; set; }

        public Dictionary<List<List<string>>, List<List<string>>> MyEmbeddedProperty { get; private set; }

        public void MyMethod()
        {
            MyHiddendMethod();
        }

        public MyClass() : this(0)
        {
            MyEmbeddedProperty = new Dictionary<List<List<string>>, List<List<string>>>();
        }

        private MyClass(int myPrivateAccess)
        {
            _myHiddenField = myPrivateAccess;
        }

        public MyClass(int parameter1, string parameter2)
        {
            
        }

        private void MyHiddendMethod()
        {
            // Do something to avoid optimization removal
            _myHiddenField = 10;
            _myProtectedField = 100;
            _myField = 1000;
        }

        public void MyMethodWithParameters(int first, string second)
        {
            
        }

        public string MyMethodWithReturnType()
        {
            return string.Empty;
        }

        public void GenericMethod<T>()
        {
            
        }

        public void GenericMethodWithGenericParameter<T>(T parameter)
        {
            
        }

        public T GenericMethodWithGenericParameterWithReturnType<T>(T parameter)
        {
            return parameter;
        }

        public TZ GenericMethodWithGenericParametersWithReturnType<TX,TY,TZ>(TX parameter1, TY parameter2, TZ parameter3)
        {
            return parameter3;
        }
    }

    public static class MyStaticClass
    {
        public static void MyExtensionMethod(this string myInstance)
        {
            
        }

        public static void MyGenericExtensionMethod<T>(this T myInstance)
        {
            
        }

        public static T MyGenericExtensionMethodWithReturnType<T>(this T myInstance)
        {
            return myInstance;            
        }
    }

    public class MyClass<T> where T : class
    {
        public T MyGetter { get; set; }

        public int this [int parameter]
        {
            get { return 1; }
            set
            {
                
            }
        }
    }
}
