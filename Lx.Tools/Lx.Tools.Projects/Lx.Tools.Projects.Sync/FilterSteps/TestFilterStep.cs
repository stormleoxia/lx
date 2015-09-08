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
using System.IO;
using System.Linq;
using Microsoft.SqlServer.Server;

namespace Lx.Tools.Projects.Sync.FilterSteps
{
    /// <summary>
    ///     Filter source file related to project depending whether it's a test project or not
    /// </summary>
    internal class TestFilterStep : FilterStep
    {
        public override string[] Filter(string[] files, ProjectAttributes attributes)
        {
            return files.Where(x => FilterTest(attributes.IsTest, x)).ToArray();
        }

        private bool FilterTest(bool isTest, string sourceFile)
        {
            var fileName = Path.GetFileNameWithoutExtension(sourceFile);
            if (!string.IsNullOrEmpty(fileName))
            {
                var res = fileName.ToLower().Contains("test");
                return isTest ? res : !res;
            }
            return false;
        }
    }
}