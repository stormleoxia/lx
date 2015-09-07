using System.Linq;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    internal class TargetFilterStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            if (attributes.Target == Targets.All)
            {
                return files.Where(x => !TargetsEx.GetValuesButAll().Any(y => x.Contains(TargetsEx.Convert(y)))).ToArray();
            }
            return files.Where(x => x.Contains(attributes.Target.Convert())).ToArray();
        }
    }
}