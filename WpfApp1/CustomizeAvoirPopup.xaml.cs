using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Proxy;

namespace DetControlTower
{
    /// <summary>
    /// Logique d'interaction pour CustomizeAvoirPopup.xaml
    /// </summary>
    public partial class CustomizeAvoirPopup : Window
    {
        public event EventHandler<FactureEventArgs<Machine>> PrintClicked;

        public ObservableCollection<Machine> cart;
        
        private Facture facture;

        public CustomizeAvoirPopup(Facture facture ,ObservableCollection<Machine> cart)
        {
            this.cart = cart;
            this.facture = facture;
            InitializeComponent();
        }

        private void Imprimer_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<Machine> collection =  new ObservableCollection<Machine>();
            List<Machine> datagridMachines = machineDataGrid.Items.OfType<Machine>().ToList();
            foreach (Machine machine in datagridMachines)
            {
                 collection.Add(machine);
            }
            
            OnPrintClicked(collection);
            this.Close();
        }

        private void Annuler_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected virtual void OnPrintClicked(ObservableCollection<Machine> collection)
        {
            PrintClicked?.Invoke(this, new FactureEventArgs<Machine>(facture, collection));
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            machineDataGrid.DataContext = cart;
        }
    }
}
