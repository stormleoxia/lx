﻿#region Copyright (c) 2015 Leoxia Ltd

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

using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Emit;
using System.Xml.Serialization;
using Lx.Tools.Performance;

namespace Lx.Tools.Reflection.Performance.Tester
{
    internal class MethodCallBenchmarkGroup : IBenchmarkGroup
    {
        public IEnumerable<IBenchmark> Benchmarks
        {
            get
            {
                return new IBenchmark[]
                {
                    new MethodInvokeBenchmark(),
                    new StandardMethodBenchmark(),
                    new ExpressionTreeCallBenchmark(),
                    new InvokeMemberBenchmark(),
                    new DelegateCallBenchmark()
                };
            }
        }

        public decimal Iterations
        {
            get { return 2000000; }
        }

        public string Name
        {
            get { return "Method Call"; }
        }
    }
}