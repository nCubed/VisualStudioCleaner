using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an callingAssembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an callingAssembly.
using CommandLine;

[assembly: AssemblyTitle( "VisualStudioCleaner.UI.Consolas" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "nCubed" )]
[assembly: AssemblyProduct( "VisualStudioCleaner" )]
[assembly: AssemblyCopyright( "Copyright © nCubed 2014" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this callingAssembly not visible 
// to COM components.  If you need to access a type in this callingAssembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "0050e2d3-091c-4299-989d-5da91a605b88" )]

// Version information for an callingAssembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [callingAssembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion( "1.0.0.0" )]
[assembly: AssemblyFileVersion( "1.0.0.0" )]


// CommandLineParser Automated Build values
// https://github.com/gsscoder/commandline/wiki/Display-A-Help-Screen
[assembly: AssemblyLicense(
    "This is free software. You may redistribute copies of it under the terms of",
    "the MIT License <http://www.opensource.org/licenses/mit-license.php>." )]
[assembly: AssemblyUsage(
    "Usage: VSCleaner.exe -d <directory>",
    "       VSCleaner.exe -d <directory> -o All",
    "       VSCleaner.exe -d <directory> -o Option|Option|Option" )]