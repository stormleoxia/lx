using System;
using System.Linq.Expressions;
using System.Reflection;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class ExpressionTreeCallBenchmark : IBenchmark
    {
        private MethodContainer _container;
        private Action<MethodContainer> _compiled;

        public void Init()
        {
            MethodInfo methodInfo = typeof(MethodContainer).GetMethod("MethodCall");
            var containerExpr = Expression.Parameter(typeof(MethodContainer), "container");
            var invokeExpr = Expression.Call(containerExpr, methodInfo);
            var lambda = Expression.Lambda<Action<MethodContainer>>(invokeExpr, containerExpr);
            _compiled = lambda.Compile();
            _container = new MethodContainer();
        }

        public void Call()
        {
            _compiled.Invoke(_container);
        }

        public void Cleanup()
        {

        }

        public string Name { get { return "Expression Tree Call"; } }
    }
}