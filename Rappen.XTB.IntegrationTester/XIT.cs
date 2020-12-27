using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XrmToolBox;
using XrmToolBox.Extensibility;

namespace Rappen.XTB.IntegrationTester
{
    public partial class XIT : PluginControlBase, IMessageBusHost
    {
        private Settings mySettings;

        public XIT()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            var tools = PluginManagerExtended.Instance.ValidatedPlugins.Select(t => new ToolProxy(t)).ToList();

            if (!SettingsManager.Instance.TryLoad(GetType(), out mySettings))
            {
                mySettings = new Settings();
            }
            tools.ForEach(t => t.Argument = mySettings.ToolArguments.FirstOrDefault(ta => ta.Identifier.Equals(t.Tool.ToString()))?.Argument);

            cmbTool.DataSource = tools;
        }

        public override void ClosingPlugin(PluginCloseInfo info)
        {
            mySettings.ToolArguments = (cmbTool.DataSource as List<ToolProxy>).Where(t => !string.IsNullOrWhiteSpace(t.Argument)).ToList();
            SettingsManager.Instance.Save(GetType(), mySettings);
            base.ClosingPlugin(info);
        }

        private void cmbTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtIdentifier.Clear();
            txtVersion.Clear();
            txtArguments.Clear();
            if (cmbTool.SelectedItem is ToolProxy tool)
            {
                txtIdentifier.Text = tool.Tool.ToString();
                txtVersion.Text = tool.Tool.Value.GetVersion();
                txtArguments.Text = tool.Argument;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbTool.SelectedItem is ToolProxy tool)
            {
                tool.Argument = txtArguments.Text;

            }
        }
    }
}