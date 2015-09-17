using Lx.Tools.Common.Program;

namespace Lx.Tools.Projects.Reference
{
    public class ReferenceManagerDefinition : UsageDefinition
    {
        public ReferenceManagerDefinition()
        {
            this.Arguments.Add(new Arguments{Name = "csproj | directory"});
            this.Arguments.Add(new Arguments{Name = "reference"});
        }
    }
}