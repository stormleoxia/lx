#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

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
        public string Result { get; set; }

        public void MethodToCall<T>(T parameter)
        {
            Result = "MethodCall<" + typeof (T).Name + ">";
        }
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
                    _method = method.MakeGenericMethod(type);
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