using System;

namespace Lx.Tools.Common.Wrappers
{
    public interface IWriter : IDisposable
    {
        void WriteLine(string text);
    }
}