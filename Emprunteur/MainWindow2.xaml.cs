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
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using Proxy;

namespace Emprunteur
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();
            TcpChannel channel = new TcpChannel();
            ChannelServices.RegisterChannel(channel, false);
        }

        public MainWindow2(Boolean ChannelCreated)
        {
            if (!ChannelCreated)
                InitializeComponent();

        }
        private void ConnectionButton_Click(Object sender, RoutedEventArgs e)
        {
            try
            {


                IServiceAuthentification serviceAuth = (IServiceAuthentification)Activator
                                                        .GetObject(typeof(IServiceAuthentification)
                                                        , "tcp://127.0.0.1:2019/ServiceAuthentification");

                if (serviceAuth.Authentifier(usr.Text, pass.Password))
                {
                    EmprunteurWindow window = new EmprunteurWindow(usr.Text);
                    window.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show(serviceAuth.getMessage());
                    AuthentificationErrorFlag.Visibility = Visibility.Visible;
                    ExceptionErrorFlag.Visibility = Visibility.Collapsed;

                }



            }
            catch (Exception exception)
            {
                AuthentificationErrorFlag.Visibility = Visibility.Collapsed;
                ExceptionErrorFlag.Visibility = Visibility.Visible;

            }

        }
        public void CloseButton_Click(Object sender, RoutedEventArgs args)
        {
            Application.Current.Shutdown();
        }
    }
}
