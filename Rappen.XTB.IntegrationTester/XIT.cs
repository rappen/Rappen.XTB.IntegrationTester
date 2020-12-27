using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XrmToolBox;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace Rappen.XTB.IntegrationTester
{
    public partial class XIT : PluginControlBase, IMessageBusHost, IGitHubPlugin, IHelpPlugin, IPayPalPlugin
    {
        private Settings mySettings;

        public string RepositoryName => "Rappen.XTB.IntegrationTester";

        public string UserName => "rappen";

        public string HelpUrl => "https://github.com/rappen/Rappen.XTB.IntegrationTester";

        public string DonationDescription => "XIT fan club";

        public string EmailAccount => "jonas@rappen.net";

        public event EventHandler<MessageBusEventArgs> OnOutgoingMessage;

        public XIT()
        {
            InitializeComponent();
        }

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
            var tools = PluginManagerExtended.Instance.ValidatedPlugins
                //.Where(t => t.Value.GetControl() is IMessageBusHost)
                .Select(t => new ToolProxy(t)).ToList();

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
                txtArguments.Enabled = tool.Tool.Value.GetControl() is IMessageBusHost;
                txtArguments.Text = txtArguments.Enabled ? tool.Argument : string.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbTool.SelectedItem is ToolProxy tool)
            {
                tool.Argument = txtArguments.Text;
                try
                {
                    OnOutgoingMessage(this, new MessageBusEventArgs(tool.Name, chkNewInstance.Checked) { TargetArgument = tool.Argument });
                }
                catch (Exception ex)
                {
                    txtInfo.Text = ex.ToString();
                }
            }
        }

        public void OnIncomingMessage(MessageBusEventArgs message)
        {
            txtInfo.Text = $"Source Tool: {message.SourcePlugin}\n\rArgument: {message.TargetArgument}";
        }
    }
}