using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using Proxy;
using DetControlTower;
namespace WpfApp1
{

    public partial class BiblioWindow : Window
    {
        private String user;
        private ISmtpNotifier serviceNotifier = (ISmtpNotifier)Activator
                                                              .GetObject(typeof(ISmtpNotifier)
                                                             , "tcp://"+MyGeneralConstants.Host+":2019/SmtpNotifier");
        


        public BiblioWindow(String user)
        {
            this.user = user;
            InitializeComponent();
            userField.Text = user;
            MainFrame.Content = new AjouterMachine();
            
        }

        public void Notify(String barcode)
        {
            serviceNotifier.notify(barcode);
        }

        private void ButtonLogout_Click(Object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow(false);
            window.Show();
            this.Close();
        }



        private void ButtonDeployNav_Click(Object sender, RoutedEventArgs e)
        {
            
          
            DeployNav.Visibility = Visibility.Collapsed;
            HideNav.Visibility = Visibility.Visible;
            Copyright.Visibility = Visibility.Visible;
            
        }
        private void ButtonHideNav_Click(Object sender, RoutedEventArgs e)
        {
            HideNav.Visibility = Visibility.Collapsed;
            DeployNav.Visibility = Visibility.Visible;
            Copyright.Visibility = Visibility.Collapsed;
        }

        private void GererOuvrage_Select(object sender, SelectionChangedEventArgs e)
        {

            if (((ListBoxItem)((ListBox)sender).SelectedItem) != null)
            {

                ouvragesDisponibleListItem.IsSelected = false;

                switch (((ListBoxItem)((ListBox)sender).SelectedItem).Content)
                {
                    case "Rapport":

                        MainFrame.Content = new ClientHistory();

                        break;

                    case "Ajouter pièce rech.":

                        MainFrame.Content = new AjouterPiece();

                        break;

                    case "Liste pièce rech.":

                        MainFrame.Content = new StockPiece();

                        break;

                    case "Ajouter Machine":

                        MainFrame.Content = new AjouterMachine();

                        break;
                    case "Facture":

                        MainFrame.Content = new FactureFinal();

                        break;
                    case "Liste de prix":

                        MainFrame.Content = new StockManager();

                        break;

                    case "Proforma":

                        MainFrame.Content = new Proforma();

                        break;

                    default:
                        Console.WriteLine("NavMenu Page not implemented ! /n default case initiated !");
                        break;
                }

                      ((ListBoxItem)((ListBox)sender).SelectedItem).IsSelected = false;
            }

        }

        private void ListView_SelectionChanged(object sender, RoutedEventArgs e)
        { 
            MainFrame.Content = new Proforma();
        }

        
    }
}
