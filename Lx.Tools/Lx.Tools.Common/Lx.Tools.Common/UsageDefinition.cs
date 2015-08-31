using System.Collections.Generic;
using System.Diagnostics;
using Lx.Tools.Common.Program;

namespace Lx.Tools.Common
{
    public class UsageDefinition
    {
        private readonly string _exeName;

        public UsageDefinition()
        {
            _exeName = System.Reflection.Assembly.GetCallingAssembly().GetName().Name;
            Arguments = new List<Arguments>();
        }

        public string ExeName
        {
            get { return _exeName; }
        }

        public List<Arguments> Arguments { get; private set; }
    }
}