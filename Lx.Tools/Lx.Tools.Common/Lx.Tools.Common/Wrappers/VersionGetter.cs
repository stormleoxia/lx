using System.Reflection;

namespace Lx.Tools.Common.Wrappers
{
    public class VersionGetter : IVersion
    {
        public string Version
        {
            get { return Assembly.GetExecutingAssembly().GetName().Version.ToString(); }
        }
    }
}