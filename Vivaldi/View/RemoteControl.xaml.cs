using AxMSTSCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Vivaldi.View
{
    /// <summary>
    /// Lógica de interacción para RemoteControl.xaml
    /// </summary>
    public partial class RemoteControl : UserControl
    {
        public RemoteControl()
        {
            InitializeComponent();
        }

        public void Connect((string username, string domain, string password, string machineName) credentials)
        {
            try
            {
                var remoteDesktopClient = new AxMsRdpClient6NotSafeForScripting();
                this.AddChild(remoteDesktopClient);

                remoteDesktopClient.AdvancedSettings7.AuthenticationLevel = 0;
                remoteDesktopClient.AdvancedSettings7.EnableCredSspSupport = true;
                remoteDesktopClient.Server = credentials.machineName;
                remoteDesktopClient.Domain = credentials.domain;
                remoteDesktopClient.UserName = credentials.username;
                remoteDesktopClient.AdvancedSettings7.ClearTextPassword = credentials.password;
                remoteDesktopClient.Connect();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
