using System;

namespace Lx.Tools.Common
{
    public interface IWriter : IDisposable
    {
        void WriteLine(string text);
    }
}