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
using Proxy;
using System.Data;
using System.Collections;
using System.Collections.ObjectModel;
using DetControlTower;


namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour StockManager.xaml
    /// </summary>
    public partial class StockManager : Page
    {
        IMachineManagerService service = (IMachineManagerService)Activator.GetObject(typeof(IMachineManagerService),
                                                                          "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public StockManager()
        {
            InitializeComponent();
        }

        public void FillDataGrid()
        {
            String refference = refFilterInput.Text;
            String arrival = arrivalFilterInput.Text;
            String provider = providerFilterInput.Text;
            String category = categFilterInput.Text;
            
            Machine filter = new Machine("" , "" + category, "" ,"" + provider, "" + arrival, ""+ refference );

            DataTable DataTable = service.GetMachineList(filter , false);

            ouvragesDataGrid.DataContext = DataTable;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            this.FillDataGrid();
        }
        
        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            IList dgr = ouvragesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {
                int idMachine = Int32.Parse(row["id"].ToString());
                string referance = row["ref"].ToString();
                Machine machine = new Machine(idMachine);

                if (service.RemoveMachine(machine))
                    MessageBox.Show("Machine Supprimé!\n referance : " + referance);
                else
                    MessageBox.Show("Suppression impossible !\n ref : " + referance + "\n Error : " + service.getMessage());
            }

            this.FillDataGrid();
        }
        
        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            IList dgr = ouvragesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {
                int id = Int32.Parse(row["id"].ToString());
                string reference = row["ref"].ToString();
                string categ = row["categ"].ToString();
                string designation = row["designation"].ToString();
                string remarque = row["remarque"].ToString();

                Machine machine = new Machine(id , categ, designation, reference );
                machine.Remarque = remarque;
                
                    if (service.EditMachine(machine))
                    MessageBox.Show("Information Machine Modifié!\n réference : " + reference);
                else
                    MessageBox.Show("Modification impossible !\n Reférence : " + reference + "\n Error : " + service.getMessage());
            }
            this.FillDataGrid();
        }


        private void FilterActivated(object sender, TextChangedEventArgs e)
        {
            FillDataGrid();
        }

        private void ReinitFilter_Click(object sender, RoutedEventArgs e)
        {
            refFilterInput.Text = "";
            categFilterInput.Text = "";
            arrivalFilterInput.Text = "";
            providerFilterInput.Text = "";
        }

        private void ImprimStock_Click(object sender, RoutedEventArgs e)
        {
            DataTable dataTable = service.GetMachineList(new Machine(), true);

            ObservableCollection<Facturable> stockCollection = new ObservableCollection<Facturable>();

            foreach (DataRow row in dataTable.Rows)
            {
                int id = Int32.Parse(row["id"].ToString());
                string reference = row["ref"].ToString();
                string categ = row["categ"].ToString();
                string designation = row["designation"].ToString();
                string remarque = row["remarque"].ToString();

                Machine machine = new Machine(id, categ, designation, reference);
                machine.Remarque = remarque;

                stockCollection.Add(machine);
            }

            PrintStockPreview stockPreview = new PrintStockPreview( stockCollection );
            stockPreview.Show();
        }
    }
}