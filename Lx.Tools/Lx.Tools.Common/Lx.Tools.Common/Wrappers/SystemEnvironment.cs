using System;

namespace Lx.Tools.Common.Wrappers
{
    public class SystemEnvironment : IEnvironment
    {
        public void Exit(int exitCode)
        {
            Environment.Exit(exitCode);
        }

        public string NewLine
        {
            get { return Environment.NewLine; }
        }
    }
}