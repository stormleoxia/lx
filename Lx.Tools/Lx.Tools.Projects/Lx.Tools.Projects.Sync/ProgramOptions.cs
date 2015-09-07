using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Sync
{
    public class ProgramOptions : Options
    {
        static ProgramOptions()
        {
            NoDelete = new Option {Name = "--no-delete", Explanation = "Missing files from sources are not deleted"};
        }

        public static Option NoDelete { get; private set; }

        public ProgramOptions(IEnvironment environment, IConsole console) : base(environment, console)
        {
            AvailableOptions.Add(NoDelete);
        }
    }
}