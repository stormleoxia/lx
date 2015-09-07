using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;

namespace Lx.Tools.Common.Program
{
    public class UsageDefinition
    {
        private readonly string _exeName;

        public UsageDefinition()
        {
            _exeName = Process.GetCurrentProcess().ProcessName;
            Arguments = new List<Arguments>();
        }

        public string ExeName
        {
            get { return _exeName; }
        }

        public List<Arguments> Arguments { get; private set; }
    }


}