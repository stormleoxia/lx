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

using System.IO;
using System.Linq;

namespace Lx.Tools.Common.Paths
{
    internal abstract class WinPath : BasePath
    {
        protected WinPath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path,
            string[] components)
            : base(factory, platformPathType, pathType, path, components)
        {
            var firstComponent = components.FirstOrDefault();
            Drive = PathUtility.GetDrive(firstComponent);
        }

        public string Drive { get; private set; }
    }

    internal abstract class BasePath : IPath
    {
        private BasePath _parent;
        private bool _parentUninitialized = true;
        private IPath _root;
        private bool _rootUninitialized = true;

        protected BasePath(PathFactory factory, PlatformPathTypes platformPathType, PathTypes pathType, string path,
            string[] components)
        {
            Factory = factory;
            PlatformPathType = platformPathType;
            PathType = pathType;
            Components = components;
            Path = path;
        }

        protected internal PlatformPathTypes PlatformPathType { get; private set; }
        protected internal PathTypes PathType { get; private set; }
        protected internal PathFactory Factory { get; private set; }
        protected internal string[] Components { get; private set; }
        public abstract IFilePath File { get; }
        public string Path { get; private set; }

        public IPath Root
        {
            get
            {
                if (_rootUninitialized)
                {
                    _rootUninitialized = false;
                    var rootPath = PathUtility.GetRootPath(Path, Components);
                    if (rootPath != null)
                    {
                        _root = Factory.Create(rootPath, PlatformPathType, PathTypes.Root,
                            new[] {Components.First()});
                    }
                }
                return _root;
            }
        }

        public IPath Child { get; private set; }

        public IPath Parent
        {
            get
            {
                if (_parentUninitialized)
                {
                    _parentUninitialized = false;
                    if (Components.Length > 1)
                    {
                        var path = PathUtility.GetParent(Path, Components);
                        var components = Components.Take(Components.Length - 1).ToArray();
                        // TODO specialize creation to avoid cast
                        _parent = (BasePath) Factory.Create(path, PlatformPathType, PathTypes.Root, components);
                        _parent.Child = this;
                    }
                }
                return _parent;
            }
        }

        public IPath Intersect(IPath path, PathIntersections bottomUp)
        {
            return path;
        }
    }
}