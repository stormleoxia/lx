namespace Lx.Tools.Common.Wrappers
{
    public class SystemEnvironment : IEnvironment
    {
        public void Exit(int exitCode)
        {
            global::System.Environment.Exit(exitCode);
        }

        public string NewLine
        {
            get { return System.Environment.NewLine; }
        }
    }
}