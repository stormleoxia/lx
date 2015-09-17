﻿using System;
using System.Collections.Generic;
using System.Linq;
using Moq;

namespace Lx.Tools.Tests.MockUnity
{
    internal class MockFactory
    {
        private readonly IDictionary<Type, Mock> _mocks = new Dictionary<Type, Mock>(); 

        public MockFactory()
        {
            
        }

        public IList<Mock> Mocks { get { return _mocks.Values.ToArray();  } }

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
            var proxy = (IMockProxy)Activator.CreateInstance(mockType);
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
