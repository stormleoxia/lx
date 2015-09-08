namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class MethodContainer
    {
        private int _innerCounter;

        public void MethodCall()
        {
            _innerCounter++;
        }
    }
}