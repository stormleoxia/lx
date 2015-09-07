using System.Linq;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    internal class ScopeFilterStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            if (attributes.HasScope)
            {
                return files.Where(x => x.Contains(attributes.Scope.ToString())).ToArray();
            }
            return files;
        }
    }
}