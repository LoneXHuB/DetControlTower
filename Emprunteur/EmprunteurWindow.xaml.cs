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

namespace Emprunteur
{
    /// <summary>
    /// Logique d'interaction pour EmprunteurWindow.xaml
    /// </summary>
    public partial class EmprunteurWindow : Window
    {
        private String user;

        public EmprunteurWindow(String user)
        {
           
            InitializeComponent();
            this.user = user;
            userField.Text = user;
            MainFrame.Content = new ConsulterOuvrage(user);

        }

        private void MainFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

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

                //ouvragesDisponibleListItem.IsSelected = false;

                switch (((ListBoxItem)((ListBox)sender).SelectedItem).Content)
                {
                    

                     case "Consulter Ouvrages":

                        MainFrame.Content = new ConsulterOuvrage(user);

                break;

                    case "Modifier Compte":

                        MainFrame.Content = new ModifierCompte(user);

                        break;
                    case "Liste Réservations":

                        MainFrame.Content = new ListeReservations(user);

                        break;
                    case "Consulter Ouvrage":

                        MainFrame.Content = new ConsulterOuvrage(user);

                        break;

                    case "Liste D'emprunts":

                        MainFrame.Content = new ListeDemprunts(user);

                        break;

                    default:
                        Console.WriteLine("NavMenu Page not implemented ! /n default case initiated !");
                        break;
                }

                      ((ListBoxItem)((ListBox)sender).SelectedItem).IsSelected = false;
            }

        }

        /*private void ListView_SelectionChanged(object sender, RoutedEventArgs e)
        {

            MainFrame.Content = new OuvragesDisponible();


        }*/

    }
}
