using System.Linq;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    internal class NameFilterStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            if (!string.IsNullOrEmpty(attributes.Name))
            {
                return files.Where(x => x.Contains(attributes.Name)).ToArray();
            }
            return files;
        }
    }
}