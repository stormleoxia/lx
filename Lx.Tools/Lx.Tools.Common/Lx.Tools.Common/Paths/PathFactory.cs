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

using System;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common.Paths
{
    public class PathFactory
    {
        private readonly PathPartFactory _partFactory;
        private readonly PathConfiguration _pathConfiguration;
        private readonly IFileSystem _system;

        public PathFactory(IFileSystem system, PathConfiguration pathConfiguration)
        {
            _system = system;
            _pathConfiguration = pathConfiguration;
            _partFactory = new PathPartFactory(pathConfiguration);
        }

        public IPath Create(string path)
        {
            return Create(path, _pathConfiguration.DefaultPlatformPathType);
        }

        public IPath Create(string path, PlatformPathTypes platformPathType)
        {
            switch (platformPathType)
            {
                case PlatformPathTypes.Infer:
                    return Create(path, InferPlatformPathType(path));
                default: // TODO manage UNC and URI
                {
                    var components = path.Split('/', '\\');
                    var parts = _partFactory.MakeParts(path);
                    components = PathUtility.CleanUp(components);
                    return Create(path, platformPathType, InferPathType(path, components), components);
                }
            }
        }

        internal IPath Create(string path, PlatformPathTypes platformPathType, PathTypes pathType, string[] components)
        {
            switch (platformPathType)
            {
                case PlatformPathTypes.Infer:
                    return Create(path, InferPlatformPathType(path), pathType, components);
                case PlatformPathTypes.Windows:
                    switch (pathType)
                    {
                        case PathTypes.Root:
                            return new WinRootPath(this, platformPathType, pathType, path, components);
                        case PathTypes.Directory:
                            return new WinDirectoryPath(this, platformPathType, pathType, path, components);
                        case PathTypes.File:
                            return new WinFilePath(this, platformPathType, pathType, path, components);
                        default:
                            throw new ArgumentOutOfRangeException("pathType", pathType, null);
                    }
                case PlatformPathTypes.Unix:
                    switch (pathType)
                    {
                        case PathTypes.Root:
                            return new UnixRootPath(this, platformPathType, pathType, path, components);
                        case PathTypes.Directory:
                            return new UnixDirectoryPath(this, platformPathType, pathType, path, components);
                        case PathTypes.File:
                            return new UnixFilePath(this, platformPathType, pathType, path, components);
                        default:
                            throw new ArgumentOutOfRangeException("pathType", pathType, null);
                    }
                    ;
                default:
                    throw new ArgumentOutOfRangeException("platformPathType", platformPathType, null);
            }
        }

        private PathTypes InferPathType(string path, string[] components)
        {
            if (_system.FileExists(path))
            {
                return PathTypes.File;
            }
            if (_system.DirectoryExists(path))
            {
                if (_system.IsRoot(path))
                {
                    return PathTypes.Root;
                }
                return PathTypes.Directory;
            }
            return PathUtility.InferPathType(path, components);
        }

        private PlatformPathTypes InferPlatformPathType(string path)
        {
            var slashCount = path.Count(x => x == '/');
            var backslashCount = path.Count(x => x == '\\');
            if (backslashCount > slashCount || path.Count(x => x == ':') == 1)
            {
                return PlatformPathTypes.Windows;
            }
            return PlatformPathTypes.Unix;
        }
    }


    public class PathPart
    {
        internal PathPart(string rawValue, PathComponentKind kind)
        {
            Kind = kind;
            RawValue = rawValue;
        }

        public PathComponentKind Kind { get; private set; }
        public string RawValue { get; private set; }
    }
}