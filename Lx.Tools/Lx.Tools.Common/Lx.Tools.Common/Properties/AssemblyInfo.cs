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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle("Lx.Tools.Common")]
[assembly: AssemblyDescription("Common sources for Lx.Tools")]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly: Guid("db8cfde1-e31e-486d-956c-97353c9c9227")]
#if MSVC
[assembly:
    InternalsVisibleTo(
        @"Lx.Tools.Common.Tests, PublicKey=002400000e800000140200000602000000240000525341310010000001000100c72c6c92e2872a" +
        "ee1fb46b7d3d898123f91fd5e47abb6b1ea4dd03ffa86deeeaa341fec8ffd07a09bf14c2620350" +
        "89a52c37953faa35d2a2e1b77fada918b9a30c645de0c48d0cae795ca9d6791a42acd663ede054" +
        "4f7f756668f7fbfe3f235df04eda1135be561e23775f36aa52a6e02fcb022c3bb93b1444bc956c" +
        "83acfc51912d6c9f7fd98742c034c154cc2b887a5db3724080d7ebd76066346b695d4a890b7329" +
        "7f7678a7f76ea39d56f2ada3e5ef4a287a0f6c0e76ebb5a357fe094e88432b17906135dc3ccbb7" +
        "525d03fb92fcf82b6cbd2c06858c247395d689c74097650547dde014248a4baf4c4d4935c1c942" +
        "66910da945f7a92a9e21febe22061bca47ed66a607d56edc7ea773290dc79aa438b7c82313638e" +
        "7364459fb90f24bf0257c01b15e20860d58c84ee6e6f6b4301237fa1f7fff370db8963c8c58181" +
        "06a6e7aeabb0dd4ea0aa2185598818e5cbb6718f96ea3efc7b21a7674a8ab258b0a1f9287c7d34" +
        "7a31ae130201612bf03e42b9417d9c737f11e3e2d7026776e692744982eb748d8890eccade86c1" +
        "f8c42cc8bad2e40677281d4ba13d6d0e852d51e8b449a6fc82f3e021c97b6c821cfa73366f23e5" +
        "dccd812acb50a84f33f056b170c1701e7e7ef5af651f5fac29f7142d796b75ff86310a7d287d29" +
        "05e36d0328afd094b0abf9500c6b8a601526570a0cdfc638568eaccf55142a84a97f083cc7")]
#else

[assembly: InternalsVisibleTo(@"Lx.Tools.Common.Tests")]
#endif