﻿//-----------------------------------------------------------------------
// <copyright file="GlobalAssemblyInfo.cs" company="YuGuan Corporation">
//     Copyright (c) YuGuan Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System.Reflection;

/// <summary>
/// This file contains common AssemblyVersion data to be shared across all projects in solution.
/// </summary>
[assembly: AssemblyDescription(".Net Development Library")]
[assembly: AssemblyCompany("YuGuan")]
[assembly: AssemblyProduct("DevLib")]
[assembly: AssemblyCopyright("YuGuan Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.2.*")]
////[assembly: AssemblyFileVersion("1.0.0.0")]