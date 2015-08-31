using System.Diagnostics;

namespace Lx.Tools.Common.Wrappers
{
    public class SystemDebugger : IDebugger
    {
        public bool IsAttached { get { return Debugger.IsAttached; } }
    }
}