using System;

namespace Lx.Tools.Common.Threading
{
    public class ThreadSafe<T> where T : new()
    {
        private readonly object _syncRoot = new object();
        private readonly T _instance;

        public ThreadSafe()
        {
            _instance = new T();
        }

        public T InnerInstance
        {
            get
            {
                lock (_syncRoot)
                {
                    return _instance;
                }
            }
        }

        public void Execute(Action<T> action)
        {
            lock (_syncRoot)
            {
                action(_instance);
            }
        }
    }
}
