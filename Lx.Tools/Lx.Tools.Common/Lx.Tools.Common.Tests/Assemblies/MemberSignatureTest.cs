#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

using System.Collections.Generic;
using System.Linq;
using Lx.Tools.Common.Assemblies;
using NUnit.Framework;

namespace Lx.Tools.Common.Tests.Assemblies
{
    [TestFixture]
    public class MemberSignatureTest
    {
        [Test]
        public void CheckConstructorPublicAreCorrectlyConstructed()
        {
            var memberInfo = typeof (MyClass).GetConstructors().FirstOrDefault(x => !x.GetParameters().Any());
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual(".ctor()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());


            memberInfo = typeof (MyClass).GetConstructors().FirstOrDefault(x => x.GetParameters().Any());
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual(".ctor(System.Int32 parameter1, System.String parameter2)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }

        [Test]
        public void GenericMethodMemberInfoTest()
        {
            var memberInfo = typeof (MyClass).GetMember("GenericMethod").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void GenericMethod<T>()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof (MyClass).GetMember("GenericMethodWithGenericParameter").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void GenericMethodWithGenericParameter<T>(T parameter)", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof (MyClass).GetMember("GenericMethodWithGenericParameterWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method T GenericMethodWithGenericParameterWithReturnType<T>(T parameter)",
                signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof (MyClass).GetMember("GenericMethodWithGenericParametersWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual(
                "Method TZ GenericMethodWithGenericParametersWithReturnType<TX,TY,TZ>(TX parameter1, TY parameter2, TZ parameter3)",
                signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }

        [Test]
        public void MemberTest()
        {
            var signature = new MemberSignature();
            signature.Signature = "MySign";
            Assert.AreEqual("MySign", signature.Signature);
            Assert.AreEqual("MySign", signature.ToString());
        }

        [Test]
        public void MethodMemberInfoTest()
        {
            var memberInfo = typeof (MyClass).GetMember("MyMethod").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void MyMethod()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof (MyClass).GetMember("MyMethodWithParameters").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.Void MyMethodWithParameters(System.Int32 first, System.String second)",
                signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());

            memberInfo = typeof (MyClass).GetMember("MyMethodWithReturnType").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Method System.String MyMethodWithReturnType()", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }

        [Test]
        public void PropertyMemberInfoTest()
        {
            var memberInfo = typeof (MyClass).GetMember("MyIntegerMember").FirstOrDefault();
            Assert.IsNotNull(memberInfo);
            var signature = new MemberSignature(memberInfo);
            Assert.AreEqual("Property System.Int32 MyIntegerMember{get; set;}", signature.Signature);
            Assert.AreEqual(signature.Signature, signature.ToString());
        }
    }

    public class MyClass
    {
        public static int _myStaticField;
        public int _myField;
        private int _myHiddenField;
        protected int _myProtectedField;

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

        public int MyIntegerMember { get; set; }
        public Dictionary<List<List<string>>, List<List<string>>> MyEmbeddedProperty { get; private set; }

        public void MyMethod()
        {
            MyHiddendMethod();
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

        public TZ GenericMethodWithGenericParametersWithReturnType<TX, TY, TZ>(TX parameter1, TY parameter2,
            TZ parameter3)
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

        public int this[int parameter]
        {
            get { return 1; }
            set { }
        }
    }
}