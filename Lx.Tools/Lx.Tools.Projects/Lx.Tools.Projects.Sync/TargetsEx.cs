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

namespace Lx.Tools.Projects.Sync
{
    internal static class TargetsEx
    {
        private static readonly Targets[] _targetsValue;
        private static readonly Targets[] _targetsValueButAll;

        static TargetsEx()
        {
            _targetsValue = (Targets[]) Enum.GetValues(typeof (Targets));
            _targetsValueButAll = _targetsValue.Where(x => x != Targets.All).ToArray();
        }

        public static string Convert(this Targets target)
        {
            switch (target)
            {
                case Targets.All:
                    return string.Empty;
                case Targets.Basic:
                    return "basic";
                case Targets.Xammac:
                    return "xammac";
                case Targets.XammacNet4Dot5:
                    return "xammac_net_4_5";
                case Targets.Monotouch:
                    return "monotouch";
                case Targets.Mobile:
                    return "mobile";
                case Targets.MobileStatic:
                    return "mobile_static";
                case Targets.Monodroid:
                    return "monodroid";
                case Targets.Net2Dot0:
                    return "net_2_0";
                case Targets.Net3Dot5:
                    return "net_3_5";
                case Targets.Net4Dot0:
                    return "net_4_0";
                case Targets.Net4Dot5:
                    return "net_4_5";
                default:
                    throw new ArgumentOutOfRangeException("target", target, null);
            }
        }

        public static Targets[] GetValues()
        {
            return _targetsValue;
        }

        public static Targets[] GetValuesButAll()
        {
            return _targetsValueButAll;
        }

        /// <summary>
        ///     Extracts target depending on the csproj name.
        ///     Try to find the target substring in the name for doing so.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static Targets ExtractTarget(this string fileName)
        {
            if (fileName.Contains(Targets.XammacNet4Dot5.Convert()))
            {
                return Targets.XammacNet4Dot5;
            }
            foreach (var target in GetValuesButAll())
            {
                if (fileName.Contains(target.Convert()))
                {
                    return target;
                }
            }
            return Targets.All;
        }
    }
}