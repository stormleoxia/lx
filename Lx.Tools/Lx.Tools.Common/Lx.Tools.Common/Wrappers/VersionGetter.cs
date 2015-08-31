namespace Lx.Tools.Common.Wrappers
{
    public class VersionGetter : IVersion
    {
        public string Version { get { return global::System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();  } }
    }
}