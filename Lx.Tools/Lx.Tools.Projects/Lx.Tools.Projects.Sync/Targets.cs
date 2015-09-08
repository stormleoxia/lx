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

namespace Lx.Tools.Projects.Sync
{
    /// <summary>
    ///     Each platform has a given target
    /// </summary>
    public enum Targets
    {
        /// <summary>
        ///     All is the default when there is not specific targeting
        /// </summary>
        All = 0,

        Basic = 1,

        /// <summary>
        ///     The xammac is for Mac targeting
        /// </summary>
        Xammac = 2,
        XammacNet4Dot5 = 3,
        Mobile = 4,
        MobileStatic = 5,

        /// <summary>
        ///     The monodroid is for Android targeting
        /// </summary>
        Monodroid = 6,

        Monotouch = 7,

        /// <summary>
        ///     The Framework 2.0 targeting
        /// </summary>
        Net2Dot0 = 8,

        /// <summary>
        ///     The Framework 3.5 targeting
        /// </summary>
        Net3Dot5 = 9,

        /// <summary>
        ///     The Framework 4.0 targeting
        /// </summary>
        Net4Dot0 = 10,

        /// <summary>
        ///     The Framework 4.5 targeting
        /// </summary>
        Net4Dot5 = 11
    }
}