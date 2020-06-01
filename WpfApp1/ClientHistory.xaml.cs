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
    public partial class ClientHistory : Page
    {
        IFactureManagerService service = (IFactureManagerService)Activator.GetObject(typeof(IFactureManagerService),
                                                                          "tcp://"+ MyGeneralConstants.Host+":2019/FactureManagerService");
        private IMachineManagerService serviceMachine = (IMachineManagerService)Activator.GetObject(typeof(IMachineManagerService),
                                                                         "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public ClientHistory()
        {

            InitializeComponent();

        }

        public void FillDataGrid()
        {
            Facture filter = new Facture();

            String clientName = clientNameInput.Text;
            String idFacture = idFactureInput.Text;

           
                filter = new Facture(idFacture);
                
           
            
          
            filter.Client.Name = clientName;

            DataTable dataTable = service.getFilteredFactureList(filter);


            facturesDataGrid.DataContext = dataTable;

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {

            this.FillDataGrid();

        }

        private void OuvragesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            //DataGridRow selectedRow = ((DataGridRow)((DataGrid)sender).SelectedItem);


        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)

        {
            IList dgr = facturesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {

                string idFacture = row["id_facture"].ToString();

                MessageBoxResult dialogResult =MessageBox.Show("Etes vous sur de vouloir Annuler cette facture ?",
                    "facture numero: "+ idFacture, MessageBoxButton.YesNo);

                if (dialogResult == MessageBoxResult.Yes)
                {

                    Facture facture = this.FillFactureFromRow(row);


                    if (facture.Type.Equals("Facture Avoir"))
                        MessageBox.Show("Creation de la facture avoir impossible! \n la facture source est une facture Avoir .");
                    else
                    {
                        ObservableCollection<Machine> cart = service.GetFactureMachineList(facture);

                        facture.Type = "Facture Avoir";
                        facture.UpdateId();
                  
                        if (service.Save(facture))
                        {
                            foreach (Machine machine in cart)
                            {
                                machine.State = "Rendu en Stock";
                                machine.IdFacture = facture.IdFacture;
                                machine.TypeFacture = facture.Type;
                                serviceMachine.UpdateMachine(machine);
                            }
                            MessageBox.Show("Facture Annulée!\n id : " + idFacture);
                        }
                        else
                            MessageBox.Show("Annulation impossible !\n id : " + idFacture + "\n Error : " + service.getMessage());

                    }

                }
               
            }

            this.FillDataGrid();




        }






        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            IList dgr = facturesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {
                string id =row["id_facture"].ToString();
                string state = row["state"].ToString();
                string type = row["type"].ToString();




                Facture facture = new Facture(id);
                facture.State = state;
                facture.Type = type;


                    if (service.EditFacture(facture))
                    MessageBox.Show("Etat Facture Modifié!\n id : " + id);
                else
                    MessageBox.Show("Modification impossible !\n id : " + id + "\n Error : " + service.getMessage());

            }

            this.FillDataGrid();
        }


        private void FilterActivated(object sender, TextChangedEventArgs e)
        {
            FillDataGrid();
        }

        private void ReinitFilter_Click(object sender, RoutedEventArgs e)
        {
            clientNameInput.Text = "";
            idFactureInput.Text = ""; 
          
        }

        private void ImprimerButton_Click(object sender, RoutedEventArgs e)
        {
         

            IList dgr = facturesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {
                Facture facture = this.FillFactureFromRow(row);

                PrintPreview printWindow;

                ObservableCollection<Machine> machineCollection = service.GetFactureMachineList(facture);
                if(machineCollection.Count == 0)
                {
                    ObservableCollection<Tache> tacheCollection = service.GetFactureTacheList(facture);
                    printWindow = new PrintPreview(facture, tacheCollection);
                }
                else
                    printWindow = new PrintPreview(facture , machineCollection );

                printWindow.Show();


            }

            this.FillDataGrid();
        }
        
        private Facture FillFactureFromRow(DataRowView row)
        {
            string id = row["id_facture"].ToString();
            string state = row["state"].ToString();
            string type = row["type"].ToString();
            string waranty = row["waranty"].ToString();
            double remise = Double.Parse(row["remise"].ToString());
            double timbre = Double.Parse(row["timbre"].ToString());
            DateTime date = Convert.ToDateTime(row["date_fact"].ToString());
            string payMethod = row["pay_method"].ToString();


            //client INFO

            string name = row["name"].ToString();
            string email = row["email"].ToString();
            string activity = row["activity"].ToString();
            string address = row["address"].ToString();
            string mobile = row["mobile"].ToString();
            string nif = row["nif"].ToString();
            string regNumber = row["reg_number"].ToString();
            string carteArt = row["carteArt"].ToString();
            string article = row["article"].ToString();


            Facture facture = new Facture(id);
            facture.State = state;
            facture.Type = type;
            facture.Waranty = waranty;
            facture.Timbre = timbre;
            facture.Remise = remise;
            facture.DateFacture = date;
            facture.PayMethod = payMethod;

            facture.Client = new Client(activity, address, article, email, mobile, name, nif, regNumber, carteArt);

            return facture;

        }
    }
}

