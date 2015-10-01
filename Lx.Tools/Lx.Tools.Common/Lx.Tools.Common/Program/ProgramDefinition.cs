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
using System.Text;
using Lx.Tools.Common.Wrappers;

namespace Lx.Tools.Common.Program
{
    public abstract class ProgramDefinition
    {
        protected readonly IConsole _console;
        private readonly IDebugger _debugger;
        private readonly UsageDefinition _definition;
        private readonly IEnvironment _environment;
        private readonly Options _options;
        private readonly IVersion _versionGetter;

        protected ProgramDefinition(Options options, UsageDefinition definition, IEnvironment environment,
            IDebugger debugger, IConsole console, IVersion versionGetter)
        {
            _options = options;
            _definition = definition;
            _environment = environment;
            _debugger = debugger;
            _console = console;
            _versionGetter = versionGetter;
        }

        public List<Argument> Arguments
        {
            get { return _definition.Arguments; }
        }

        public void Run(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledExcetion;
            if (args.Length == 0)
            {
                DisplayUsage();
            }
            var options = ManageOptions(args);
            try
            {
                InnerRun(options, args);
            }
            catch (Exception e)
            {
                _console.WriteLine(e);
                if (!_debugger.IsAttached)
                {
                    throw;
                }
            }
            Exit(0);
        }

        private void OnUnhandledExcetion(object sender, UnhandledExceptionEventArgs e)
        {
            _console.Error.WriteLine("Unhandled Exception: " + e.ExceptionObject);
        }

        private HashSet<Option> ManageOptions(string[] args)
        {
            var activatedOptions = _options.ParseOptions(args);
            if (activatedOptions.Contains(Options.Help))
            {
                DisplayUsage();
                _options.DisplayHelp();
                Exit(0);
            }
            return InnerManageOptions(activatedOptions);
        }

        protected abstract HashSet<Option> InnerManageOptions(HashSet<Option> activatedOptions);
        protected abstract void InnerRun(HashSet<Option> options, string[] args);

        public void DisplayUsage()
        {
            var builder = new StringBuilder();
            builder.Append("Usage: ");
            builder.Append(_definition.ExeName);
            if (_options.AvailableOptions.Count > 0)
            {
                builder.Append(" [options]");
            }
            foreach (var arg in _definition.Arguments)
            {
                builder.Append(" ");
                builder.Append(arg.Name);
            }
            _console.WriteLine(builder.ToString());
            var version = _versionGetter.Version;
            _console.WriteLine(_definition.ExeName + " " + version);
            _console.WriteLine("Copyright (C) 2015 Leoxia Ltd");
        }

        public void Exit(int exitCode)
        {
            if (_debugger.IsAttached)
            {
                _console.WriteLine("Press a key to exit...");
                _console.ReadLine();
            }
            _environment.Exit(exitCode);
        }
    }
}