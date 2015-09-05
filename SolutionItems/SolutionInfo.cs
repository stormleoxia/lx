#region Copyright (c) 2015 Leoxia Ltd.

// Copyright © 2015 Leoxia Ltd.
// 
// This file is part of Lx.
//
// Lx is released under GNU General Public License unless stated otherwise.
// You may not use this file except in compliance with the License.
// You can redistribute it and/or modify it under the terms of the GNU General Public License 
// as published by the Free Software Foundation, either version 3 of the License, 
// or any later version.
// 
// In case GNU General Public License is not applicable for your use of Lx, 
// you can subscribe to commercial license on 
// http://www.leoxia.com 
// by contacting us through the form page or send us a mail
// mailto:contact@leoxia.com
//  
// Unless required by applicable law or agreed to in writing, 
// Lx is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES 
// OR CONDITIONS OF ANY KIND, either express or implied. 
// See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with Lx.
// It is present in the Lx root folder SolutionItems/GPL.txt
// If not, see http://www.gnu.org/licenses/.
//

#endregion

#region Usings

using System;
using System.Reflection;
using System.Runtime.InteropServices;

#endregion

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyCompany("Leoxia Ltd")]
[assembly: AssemblyProduct("Lx")]

#if LEOXIA_LICENSED
    #pragma warning disable 1699
    [assembly: AssemblyCopyright("Copyright © 2015 Leoxia. Released under Commercial License Terms.")]
#else
    [assembly: AssemblyCopyright("Copyright © 2015 Leoxia. Released under GPL License Terms.")]
#endif

#if !DEBUG
[assembly: AssemblyKeyFile("leoxia.public.snk")]
[assembly: AssemblyDelaySign(true)]
#endif

[assembly: AssemblyTrademark("Leoxia")]
[assembly: AssemblyCulture("")]

// Enforce CLS Compliant types

[assembly: CLSCompliant(true)]

// Enforce Security Minimum

// TODO : check replacement

//[assembly: SecurityPermission(SecurityAction.RequestMinimum)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly: ComVisible(false)]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//

[assembly: AssemblyVersion("0.9.0.0")]
[assembly: AssemblyFileVersion("0.9.0.0")]

#if DEBUG

[assembly: AssemblyConfiguration("Alpha")]
#else
[assembly: AssemblyConfiguration("Retail")]
#endif