using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.IntegrationTester
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "XrmToolBox Integration Tester"),
        ExportMetadata("Description", "Test integration scenarios with any XrmToolBox tool"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAFXRFWHRDcmVhdGlvbiBUaW1lAAfkDAQIAQzALEbAAAAAB3RJTUUH5AwbEQwBxb94dgAAAAlwSFlzAAAK8AAACvABQqw0mAAAAOdQTFRF///G3ufGrca9jKW9e5y9hKW9pb291t7GIVq1AEKtAEq9GFKta5S17/fGAEK1AFLWAFrvAGP/WoS1vc69AFLeAFrnEEqtlK29AErOAGP3EGvvlLW9EGPGnMZj7/cQ//8A5+8YhLV7CGv39/cIpcZaGGvGc5y93u8hvdZCnLW9lL1ja6WU5/cQrc5SCEqtUpSte62EQoy9Snu15+/GKXvW1ucpGHPnxta9jL1rOYTGrc5KKWO19//GWpyltc5Kzt4xjK29AErGtdZKY6WcjL1zUoS1xt45Y4y1lL1rQnO1EGPOGGvOc5S9oEVXGQAAAAF0Uk5TAEDm2GYAAAHLSURBVHjabZMLU+owEIULFnBDSVloq0EltVbABxUf9/q4eKW+6uv//x63aaDNwJlhhnA+srvJiWWVmt/ujbLsZ6c/tzaplQXAWZqmHJLe7ZpdewPmCVQSXgqfLdNvBszHirosWVT9BrhiUKgABuhBNq/4XwdDWegvlZiN5TD+hmzp1+FrLGUJTNWX8Bt0levAPaUfhqHSAC9ocUSfqZcUnWZc0P6PqwZpgw+8JChKP9WA4AtalRO8S3mJGFK1LvQJGHE0gQMpJ/czqomYvhGw7a4DSq+IftKhCl0TuNP+JKKBqUYd0AByfxo/Pb+oc+cLa8ENIPdjsdoudSybVYHSj6IoB3oK0Cvj/2N1qATkJXDD/hpwVJOkx/9PcVytXwDU5DWNiWKqZyv7U4AAitY/F3Gm/YeyfwV4AWWiwdXq7p52eUATYL38thOPxpQCb6T8YwI+qPA6XNDVXeR1JitfXTe7UnnotHdPdUbO6JqVzvMDd5NaEak+7BeRm1CPq+yFx3CyDOUe7FNow8P8OLV9FB/DqIz9DuyKZeALCRfs6sPot7lXfTg+C5rm0+o4wN2uflYug15t/XXa2wCMBNB2WtZG1epbtt1oGu4vJORUDz30AooAAAAASUVORK5CYII="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#FFFFC0"),
        ExportMetadata("PrimaryFontColor", "#0000C0"),
        ExportMetadata("SecondaryFontColor", "#0000FF")]
    public class XITDescription : PluginBase
    {
        public override IXrmToolBoxPluginControl GetControl()
        {
            return new XIT();
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        public XITDescription()
        {
            // If you have external assemblies that you need to load, uncomment the following to 
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}