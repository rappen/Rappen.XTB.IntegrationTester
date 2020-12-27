using System;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.IntegrationTester
{
    public class ToolProxy
    {
        public readonly Lazy<IXrmToolBoxPlugin, IPluginMetadata> Tool;

        public ToolProxy(Lazy<IXrmToolBoxPlugin, IPluginMetadata> tool)
        {
            Tool = tool;
            Identifier = tool.ToString();
        }

        public override string ToString() => Tool.Metadata.Name;

        public string Identifier { get; set; }
        public string Argument { get; set; }

        public ToolProxy() { }
    }
}