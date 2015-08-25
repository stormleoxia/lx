using System;

namespace Lx.Tools.Projects.SourceDump
{
    internal interface IWriter : IDisposable
    {
        void WriteLine(string text);
    }
}