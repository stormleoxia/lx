using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Projects.Reference
{
    public class ReferenceOptions : Options
    {
        static ReferenceOptions()
        {
            AddReference = new Option {Name = "--add-ref", Explanation = "Add Reference in csproj"};
        }

        public ReferenceOptions(IEnvironment environment, IConsole console) : base(environment, console)
        {
            AvailableOptions.Add(AddReference);
        }

        public static Option AddReference { get; private set; }
    }
}