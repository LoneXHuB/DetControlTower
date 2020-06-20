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
using System.Windows.Shapes;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using Proxy;
using DetControlTower;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
        }

        public MainWindow(Boolean ChannelCreated)
        {
            if(! ChannelCreated)
            InitializeComponent();
           
        }

        private void ConnectionButton_Click(Object sender, RoutedEventArgs e)
        {
            ConnectToServer();
        }
        private void ConnectToServer()
        {
            try
            {
                IServiceAuthentification serviceAuth = (IServiceAuthentification)Activator
                                                        .GetObject(typeof(IServiceAuthentification)
                                                        , "tcp://" + MyGeneralConstants.Host + ":2019/ServiceAuthentification");

                if (serviceAuth.AuthentifierBiblio(usr.Text, pass.Password))
                {
                    BiblioWindow window = new BiblioWindow(usr.Text);
                    window.Show();
                    this.Close();

                }
                else
                {
                    AuthentificationErrorFlag.Visibility = Visibility.Visible;
                    ExceptionErrorFlag.Visibility = Visibility.Collapsed;
                }

            }
            catch
            {
                AuthentificationErrorFlag.Visibility = Visibility.Collapsed;
                ExceptionErrorFlag.Visibility = Visibility.Visible;
            }
        }
       public void CloseButton_Click(Object sender , RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }

        private void Keydown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                ConnectToServer();
        }
    }
}
