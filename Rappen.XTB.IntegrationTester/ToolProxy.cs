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

        public string Name => Tool.Metadata.Name;

        public override string ToString() => Name;

        public string Identifier { get; set; }
        public string Argument { get; set; }

        public ToolProxy() { }
    }
}