using System.Windows;
using System.Windows.Markup;

[assembly: XmlnsDefinition("http://schemas.platform.com/", "Globe.Client.Platform")]
[assembly: XmlnsDefinition("http://schemas.platform.com/", "Globe.Client.Platform.Controls")]
[assembly: XmlnsDefinition("http://schemas.platform.com/", "Globe.Client.Platform.Views")]
[assembly: XmlnsDefinition("http://schemas.platform.com/", "Globe.Client.Platform.Converters")]

[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //where theme specific resource dictionaries are located
                                     //(used if a resource is not found in the page,
                                     // or application resource dictionaries)
    ResourceDictionaryLocation.SourceAssembly //where the generic resource dictionary is located
                                              //(used if a resource is not found in the page,
                                              // app, or any theme specific resource dictionaries)
)]

[assembly: System.Reflection.AssemblyCompanyAttribute("Globe")]
[assembly: System.Reflection.AssemblyConfigurationAttribute("Release")]
[assembly: System.Reflection.AssemblyFileVersionAttribute("1.0.0.0")]
[assembly: System.Reflection.AssemblyInformationalVersionAttribute("1.0.0")]
[assembly: System.Reflection.AssemblyProductAttribute("Globe.Client.Platform")]
[assembly: System.Reflection.AssemblyTitleAttribute("Globe.Client.Platform")]
[assembly: System.Reflection.AssemblyVersionAttribute("1.0.0.0")]
