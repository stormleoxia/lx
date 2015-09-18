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
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{
    internal class MockFactory
    {
        private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>();

        public IList<Mock> Mocks
        {
            get { return _mocks.Values.ToArray(); }
        }

        public Mock Get(Type type, MockBehavior behavior)
        {
            Mock instance;
            if (!_mocks.TryGetValue(type, out instance))
            {
                instance = CreateMock(type, behavior);
                _mocks.Add(type, instance);
            }
            return instance;
        }

        public static Mock CreateMock(Type type, MockBehavior behavior)
        {
            var mockType = typeof (MockProxy<>).MakeGenericType(type);
            var proxy = (IMockProxy) Activator.CreateInstance(mockType);
            return proxy.CreateMock(behavior);
        }
    }

    internal interface IMockProxy
    {
        Mock CreateMock(MockBehavior behavior);
    }

    internal class MockProxy<T> : IMockProxy where T : class
    {
        public Mock CreateMock(MockBehavior behavior)
        {
            return new Mock<T>(behavior);
        }
    }
}