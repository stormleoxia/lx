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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common
{
    public class UPath
    {
        private readonly UPathComponents _absolute;
        private readonly UPathComponents _relative;

        static UPath()
        {
            FSystem = new FileSystem();
        }

        public UPath(string path)
        {
            if (IsPathRooted(path))
            {
                _absolute = new UPathComponents(path, true);
            }
            else
            {
                _relative = new UPathComponents(path, false);
            }
            if (_absolute == null)
            {
                var resolved = FSystem.ResolvePath(path);
                if (resolved != null)
                {
                    _absolute = new UPathComponents(resolved, true);
                }
            }
        }

        private UPath(UPathComponents path)
        {
            if (path.IsAbsolute)
            {
                _absolute = path;
            }
            else
            {
                _relative = path;
            }
        }

        public UPath(string referenceDirectory, string file) : this(Path.Combine(referenceDirectory, file))
        {
        }

        internal static IFileSystem FSystem { get; set; }

        public bool HasAbsolute
        {
            get { return _absolute != null; }
        }

        public bool HasRelative
        {
            get { return _relative != null; }
        }

        public UPathComponents Absolute
        {
            get { return _absolute; }
        }

        public UPathComponents Components
        {
            get { return HasAbsolute ? _absolute : _relative; }
        }

        private bool IsPathRooted(string path)
        {
            return (path.Contains(':') || path[0] == '/' || path[0] == '\\');
        }

        public UPath MakeRelativeUPath(UPath file)
        {
            if (file.HasAbsolute && HasAbsolute)
            {
                return MakeRelativeUPath(_absolute, file._absolute);
            }
            if (file.HasRelative && HasRelative)
            {
                return MakeRelativeUPath(_relative, file._relative);
            }
            if (file.HasRelative && HasAbsolute)
            {
                return file;
            }
            throw new InvalidOperationException(
                "We cannot make relative path between two not found relative and absolute paths:" + this + " and " +
                file);
        }

        private UPath MakeRelativeUPath(UPathComponents referal, UPathComponents referee)
        {
            if (referal.Drive != referee.Drive)
            {
                throw new InvalidOperationException("Making relative path between path on two differents drives " +
                                                    referal + " <> " + referee);
            }
            var fileComponents = new Queue<string>(referee.Components.Take(referee.Components.Length - 1));
            var components = new Queue<string>(referal.Components);
            var newComponents = new List<string>();
            var differencefound = false;
            if (CheckDoubleDots(components, fileComponents))
            {
                throw new InvalidOperationException("Cannot infer the name of a directory for relative path from " +
                                                    referal + " to " + referee);
            }
            while (components.Count > 0 && fileComponents.Count > 0)
            {
                var fileComponent = fileComponents.Peek();
                var component = components.Peek();
                if (component != fileComponent || differencefound)
                {
                    differencefound = true;
                    newComponents.Add("..");
                    components.Dequeue();
                }
                else
                {
                    fileComponents.Dequeue();
                    components.Dequeue();
                }
            }
            while (components.Count > 0)
            {
                newComponents.Add("..");
                components.Dequeue();
            }
            while (fileComponents.Count > 0)
            {
                newComponents.Add(fileComponents.Dequeue());
            }
            newComponents.Add(referee.Components.Last());
            return new UPath(new UPathComponents(referal.Drive, newComponents.ToArray(), false));
        }

        private bool CheckDoubleDots(Queue<string> components, Queue<string> fileComponents)
        {
            var fileDoubledots = CountDoubleDots(fileComponents);
            var doubleDots = CountDoubleDots(components);
            return (components.Count > 0 && doubleDots != fileDoubledots);
        }

        private int CountDoubleDots(Queue<string> components)
        {
            return components.Count(x => x == "..");
        }

        public override string ToString()
        {
            if (_relative != null)
            {
                return _relative.ToString();
            }
            return _absolute.ToString();
        }
    }
}