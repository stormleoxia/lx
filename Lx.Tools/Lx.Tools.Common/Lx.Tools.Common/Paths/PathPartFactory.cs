using System.Collections.Generic;
using System.Linq;

namespace Lx.Tools.Common.Paths
{
    public class PathPartFactory
    {
        private readonly PathConfiguration _pathConfiguration;

        public PathPartFactory(PathConfiguration pathConfiguration)
        {
            _pathConfiguration = pathConfiguration;
        }


        public PathPart[] MakeParts(string path)
        {
            List<PathPart> list = new List<PathPart>();
            var parts = StringEx.SplitKeepDelimiters(path, new[] {"/", "\\"}).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            list.Add(MakeRootPart(parts[0]));
            if (parts.Length > 2)
            {
                for (int index = 1; index < parts.Length - 1; index++)
                {
                    var part = parts[index];
                    list.Add(MakePart(part));
                }
            }
            var lastPart = parts[parts.Length - 1];
            list.Add(MakeFilePart(lastPart));
            return list.ToArray();
        }

        private PathPart MakeFilePart(string component)
        {
            if (IsSeparator(component))
            {
                return new PathPart(component, PathComponentKind.Separator);
            }            
            return new PathPart(component, PathComponentKind.File);
        }

        private PathPart MakeRootPart(string component)
        {
            if (IsRootOnUnix(component) || IsRootOnWindows(component))
            {
                return new PathPart(component, PathComponentKind.Root);
            }
            return MakePart(component);
        }

        private bool IsRootOnWindows(string component)
        {
            return component.Length == 2 && component[1] == ':';
        }

        private bool IsRootOnUnix(string component)
        {
            return component == "/";
        }

        private PathPart MakePart(string component)
        {
            if (IsGoToParent(component))
            {
                return new PathPart(component, PathComponentKind.GoToParent);
            }
            if (IsSeparator(component))
            {
                return new PathPart(component, PathComponentKind.Separator);
            }
            return new PathPart(component, PathComponentKind.Directory);
        }

        private bool IsSeparator(string component)
        {
            return component == "/" || component == "\\";
        }

        private bool IsGoToParent(string component)
        {
            return component == _pathConfiguration.GoToParentPattern;
        }
    }
}