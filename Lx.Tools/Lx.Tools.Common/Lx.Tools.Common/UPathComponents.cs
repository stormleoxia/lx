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

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lx.Tools.Common
{
    public class UPathComponents
    {
        private readonly string _path;

        public UPathComponents(string path, bool isAbsolute)
        {
            _path = path;
            if (_path.Contains(':'))
            {
                var splits = _path.Split(':');
                Drive = splits[0];
                _path = splits[1];
            }
            Components = _path.Split('/', '\\');
            Components = ResolveComponents(Components);
            IsAbsolute = isAbsolute;
        }

        public UPathComponents(string drive, string[] newComponents, bool isAbsolute)
        {
            Drive = drive;
            Components = newComponents;
            IsAbsolute = isAbsolute;
        }

        public string[] Components { get; private set; }
        public bool IsAbsolute { get; private set; }
        public string Drive { get; set; }

        private string[] ResolveComponents(string[] components)
        {
            var result = new List<string>();
            foreach (var component in components)
            {
                if (component == ".")
                {
                    continue;
                }
                if (component == ".." && result.Count > 0 && result.Last() != "..")
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

        public override string ToString()
        {
            var first = string.Empty;
            if (IsAbsolute)
            {
                first = Drive;
                if (!string.IsNullOrEmpty(first))
                {
                    first += ":";
                }
                else
                {
                    first = Path.DirectorySeparatorChar.ToString();
                }
            }
            return Components.Aggregate(first, (x, y) =>
            {
                if (string.IsNullOrEmpty(x))
                {
                    return y;
                }
                if (string.IsNullOrEmpty(y))
                {
                    return x;
                }
                if (x == Path.DirectorySeparatorChar.ToString())
                {
                    return x + y;
                }
                return x + Path.DirectorySeparatorChar + y;
            });
        }
    }
}