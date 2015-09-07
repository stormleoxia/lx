using System.Linq;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    internal class NotSupportedTargetStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            if (attributes.Target != Targets.All)
            {
                var allOtherTargets = TargetsEx.GetValuesButAll().Where(x => x != attributes.Target).ToArray();
                return files.Where(x => !allOtherTargets.Any(y => x.Contains(y.Convert()))).ToArray();
            }
            return files;
        }
    }
}