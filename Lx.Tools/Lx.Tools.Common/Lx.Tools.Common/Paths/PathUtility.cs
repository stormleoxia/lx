﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Lx.Tools.Common.Paths
{
    public static class PathUtility
    {
        internal static string[] CleanUp(string[] components)
        {
            var result = new List<string>();
            foreach (var component in components)
            {
                if (IsDotOrEmpty(component))
                {
                    continue;
                }                
                if (IsDoubleDotWithAboveDirectory(component, result))
                {
                    result.RemoveAt(result.Count - 1);
                }
                else
                {
                    result.Add(component);
                }
            }
            if (result.Count > 0)
            {
                if (result.Last() == string.Empty)
                {
                    result.RemoveAt(result.Count - 1);
                }
            }
            return result.ToArray();
        }

        private static bool IsDoubleDotWithAboveDirectory(string component, List<string> result)
        {
            return component == ".." && result.Count > 0 && result.Last() != "..";
        }

        private static bool IsDotOrEmpty(string component)
        {
            return component == "." || component == string.Empty;
        }

        internal static PathTypes InferPathType(string path, string[] components)
        {
            if (components.Length == 0)
            {
                if (path[0].IsDirectorySeparator())
                {
                    return PathTypes.Root;
                }
            }
            if (components.Length == 1)
            {
                var component = components[0];
                if (component.Length < 3)
                {
                    if (component[1] == ':')
                    {
                        return PathTypes.Root;
                    }
                }
            }
            if (path[path.Length - 1].IsDirectorySeparator())
            {
                return PathTypes.Directory;
            }
            return PathTypes.File;
        }
    }
}
