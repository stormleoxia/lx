#region Copyright (c) 2015 Leoxia Ltd

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
            var list = new List<PathPart>();
            var parts = StringEx.SplitKeepDelimiters(path,
                new[]
                {
                    _pathConfiguration.DirectorySeparator,
                    _pathConfiguration.AltDirectorySeparator
                }).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            list.Add(MakeRootPart(parts[0]));
            if (parts.Length > 2)
            {
                for (var index = 1; index < parts.Length - 1; index++)
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
            if (IsStayInCurrent(component))
            {
                return new PathPart(component, PathComponentKind.StayInCurrent);
            }
            if (IsSeparator(component))
            {
                return new PathPart(component, PathComponentKind.Separator);
            }
            return new PathPart(component, PathComponentKind.Directory);
        }

        private bool IsStayInCurrent(string component)
        {
            return component == _pathConfiguration.StayInCurrent;
        }

        private bool IsSeparator(string component)
        {
            return component == _pathConfiguration.DirectorySeparator ||
                   component == _pathConfiguration.AltDirectorySeparator;
        }

        private bool IsGoToParent(string component)
        {
            return component == _pathConfiguration.GoToParent;
        }
    }
}