using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.ActionManagement;
using JetBrains.Application.PluginSupport;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("RGendarme")]
[assembly: AssemblyDescription("Gendarme for Resharper")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Anton Zhidkov")]
[assembly: AssemblyProduct("RGendarme")]
[assembly: AssemblyCopyright("Copyright © Anton Zhidkov, 2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

[assembly: AssemblyVersion("0.1.0.0")]
[assembly: AssemblyFileVersion("0.1.0.0")]

[assembly: ActionsXml("RGendarme.Actions.xml")]

// The following information is displayed by ReSharper in the Plugins dialog
[assembly: PluginTitle("RGendarme")]
[assembly: PluginDescription("Gendarme for Resharper")]
[assembly: PluginVendor("Anton Zhidkov")]

// Disable Com interoperability
[assembly: ComVisible(false)]
[assembly: Guid("845e7570-d2c5-4073-a0b3-6059e992ce4e")]
