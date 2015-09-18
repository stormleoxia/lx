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
using Lx.Tools.Common.Program;
using Lx.Tools.Common.Wrappers;
using Microsoft.Practices.Unity;

namespace Lx.Tools.Projects.Reference
{
    public class ReferenceManager : ProgramDefinition
    {
        private readonly IUnityContainer _container;

        public ReferenceManager(IUnityContainer container, ReferenceOptions options, UsageDefinition definition,
            IEnvironment environment, IDebugger debugger, IConsole console, IVersion versionGetter)
            : base(options, definition, environment, debugger, console, versionGetter)
        {
            _container = container;
        }

        protected override HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions)
        {
            return activatedOptions;
        }

        protected override void InnerRun(HashSet<Option> options, string[] args)
        {
            if (options.Contains(ReferenceOptions.AddReference))
            {
                var adder = _container.Resolve<IReferenceAdder>();
                adder.AddReference(args);
            }
            else
            {
                _console.Error.WriteLine("No managed option");
                DisplayUsage();
            }
        }
    }
}