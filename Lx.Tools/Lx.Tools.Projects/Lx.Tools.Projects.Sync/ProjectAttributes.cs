#region Copyright (c) 2015 Leoxia Ltd

// #region Copyright (c) 2015 Leoxia Ltd
// 
// // Copyright © 2015 Leoxia Ltd.
// // 
// // This file is part of Lx.
// //
// // Lx is released under GNU General Public License unless stated otherwise.
// // You may not use this file except in compliance with the License.
// // You can redistribute it and/or modify it under the terms of the GNU General Public License 
// // as published by the Free Software Foundation, either version 3 of the License, 
// // or any later version.
// // 
// // In case GNU General Public License is not applicable for your use of Lx, 
// // you can subscribe to commercial license on 
// // http://www.leoxia.com 
// // by contacting us through the form page or send us a mail
// // mailto:contact@leoxia.com
// //  
// // Unless required by applicable law or agreed to in writing, 
// // Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// // OR CONDITIONS OF ANY KIND, either express or implied. 
// // See the GNU General Public License for more details.
// //
// // You should have received a copy of the GNU General Public License along with Lx.
// // It is present in the Lx root folder SolutionItems/GPL.txt
// // If not, see http://www.gnu.org/licenses/.
// //
// 
// #endregion

#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using Lx.Tools.Common;

namespace Lx.Tools.Projects.Sync
{
    public class ProjectAttributes
    {
        private static readonly Dictionary<string, Targets> _targets;

        static ProjectAttributes()
        {
            _targets = TargetsEx.GetValuesButAll().ToDictionary(x => x.Convert(), y => y);
        }

        public ProjectAttributes(string projectFilePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(projectFilePath);
            if (fileName != null)
            {
                var components = fileName.Split('-');
                var notRecognizedComponents = new List<string>();
                var targetUninitialized = true;
                foreach (var component in components)
                {
                    if (IsATestProject(component))
                    {
                        IsTest = true;
                        continue;
                    }
                    if (TestIsInNamespace(component))
                    {
                        IsTest = true;
                        notRecognizedComponents.Add(component);
                        continue;
                    }
                    Targets target;
                    if (TryGetTarget(component, out target))
                    {
                        targetUninitialized = false;
                        Target = target;
                        continue;
                    }
                    Scopes scope;
                    if (TryGetScope(component, out scope))
                    {
                        HasScope = true;
                        Scope = scope;
                        continue;
                    }
                    notRecognizedComponents.Add(component);
                }
                if (targetUninitialized)
                {
                    Target = Targets.All;
                }
                Name = string.Join(",", notRecognizedComponents);
            }
            else
            {
                throw new InvalidOperationException("Unable to get filename without extension for " + projectFilePath);
            }
        }

        public bool IsTest { get; private set; }
        public Scopes Scope { get; private set; }
        public Targets Target { get; private set; }
        public string Name { get; private set; }
        public bool HasScope { get; private set; }

        private static bool IsATestProject(string component)
        {
            return component == "test";
        }

        private static bool TestIsInNamespace(string component)
        {
            if (component.Contains("."))
            {
                return
                    component.Split('.')
                        .Any(x => string.Compare("test", x, StringComparison.InvariantCultureIgnoreCase) == 0);
            }
            return false;
        }

        private bool TryGetScope(string component, out Scopes scope)
        {
            if (ScopesEx.AllValues.Contains(component))
            {
                scope = (Scopes) Enum.Parse(typeof (Scopes), component);
                return true;
            }
            scope = Scopes.build;
            return false;
        }

        private bool TryGetTarget(string component, out Targets target)
        {
            return _targets.TryGetValue(component, out target);
        }
    }
}