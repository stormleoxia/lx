﻿#region Copyright (c) 2015 Leoxia Ltd

//  Copyright © 2015 Leoxia Ltd
//  
//  This file is part of Lx.
// 
//  Lx is released under GNU General Public License unless stated otherwise.
//  You may not use this file except in compliance with the License.
//  You can redistribute it and/or modify it under the terms of the GNU General Public License 
//  as published by the Free Software Foundation, either version 3 of the License, 
//  or any later version.
//  
//  In case GNU General Public License is not applicable for your use of Lx, 
//  you can subscribe to commercial license on 
//  http://www.leoxia.com 
//  by contacting us through the form page or send us a mail
//  mailto:contact@leoxia.com
//   
//  Unless required by applicable law or agreed to in writing, 
//  Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
//  OR CONDITIONS OF ANY KIND, either express or implied. 
//  See the GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License along with Lx.
//  It is present in the Lx root folder SolutionItems/GPL.txt
//  If not, see http://www.gnu.org/licenses/.

#endregion

using System;
using System.Collections.Generic;
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

        /// <summary>
        ///     Gets the drive.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <returns></returns>
        internal static string GetDrive(string component)
        {
            if (!string.IsNullOrEmpty(component))
            {
                if (component.Length < 3)
                {
                    if (component[1] == ':')
                    {
                        return component[0].ToString();
                    }
                }
            }
            return null;
        }

        /// <summary>
        ///     Gets file name and extension.
        /// </summary>
        /// <param name="components">The components.</param>
        /// <returns></returns>
        internal static string GetFile(string[] components)
        {
            return components.LastOrDefault();
        }

        /// <summary>
        ///     Gets the parent path from the given path and its components.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="components">The components.</param>
        /// <returns></returns>
        internal static string GetParent(string path, string[] components)
        {
            var last = components.Last();
            var index = path.IndexOf(last, StringComparison.InvariantCulture);
            return path.Remove(index);
        }

        internal static string GetRootPath(string path, string[] components)
        {
            var first = components.First();
            if (first.Contains(":"))
            {
                return first;
            }
            return null;
        }
    }
}