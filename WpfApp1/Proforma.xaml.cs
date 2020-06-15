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
using System.Collections.ObjectModel;
using System.Collections;
using DetControlTower;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Proforma.xaml
    /// </summary>

    public partial class Proforma : Page
    {
        private IMachineManagerService service = (IMachineManagerService)Activator.GetObject(typeof(IMachineManagerService),
                                                                            "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");
        private IClientManagerService clientService = (IClientManagerService)Activator.GetObject(typeof(IClientManagerService),
                                                                            "tcp://" + MyGeneralConstants.Host + ":2019/ClientManagerService");
        private IFactureManagerService factureService = (IFactureManagerService)Activator.GetObject(typeof(IFactureManagerService),
                                                                                    "tcp://" + MyGeneralConstants.Host + ":2019/FactureManagerService");

        private ObservableCollection<Machine> cart = new ObservableCollection<Machine>();
        private ObservableCollection<Machine> cartMachines = new ObservableCollection<Machine>();
        private ObservableCollection<String> clientList = new ObservableCollection<String>();
        private Facture proforma ;

        public ObservableCollection<String> ClientList { set => clientList = value; get => clientList; }

        private bool newClient = false;

        public Proforma()
        {
            ClientList = clientService.ClientList();

            InitializeComponent();

            artisant.IsSelected = true;
        }

        public void FillDataGrid()
        {
            String reference = refInput.Text;
          

            Machine filter = new Machine(null , reference);

            DataTable dataTable = service.GetMachineList(filter , true);


            MachinesDataGrid.DataContext = dataTable;

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
           
            DataContext = new Proforma();
            this.FillDataGrid();

        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            IList selectedRows = MachinesDataGrid.SelectedItems;

            foreach (DataRowView row in selectedRows)
            {
                Machine machine = new Machine();
                machine.Id = row["id"].ToString();
                machine.Refference = row["ref"].ToString();
                machine.Pdv = Double.Parse(row["pdv"].ToString());
                machine.NameF = row["namef"].ToString();
                machine.Designation = row["designation"].ToString();

                cartMachines.Add(machine);
                //Counting machine quantity
                if (!cart.Any(cartMachine => cartMachine.Refference == machine.Refference))
                    cart.Add(machine);
                else
                {
                    foreach (Machine cartMachine in cart)
                    {
                        if (cartMachine.Refference.Equals(machine.Refference))
                        {
                            cartMachine.Quantity++;
                            break;
                        }

                    }
                }
            }
            MessageBox.Show("Machines ajouté au panier ! ");
            CartDataGrid.ItemsSource = cart;
        }

        private void NewClientButton_Click(object sender, RoutedEventArgs e)
        {
            nameRow.Visibility = Visibility.Visible;
            regRow.Visibility = Visibility.Visible;
            nifRow.Visibility = Visibility.Visible;
            mobileRow.Visibility = Visibility.Visible;
            articleRow.Visibility = Visibility.Visible;
            addressRow.Visibility = Visibility.Visible;
            activityRow.Visibility = Visibility.Visible;
            activityCodeInput.Visibility = Visibility.Visible;

            newClientButton.Visibility = Visibility.Collapsed;
            oldClientButton.Visibility = Visibility.Visible;

            this.newClient = true;

        }

        
        private void OldClientButton_Click(object sender, RoutedEventArgs e)
        {
            nameRow.Visibility = Visibility.Collapsed;
            regRow.Visibility = Visibility.Collapsed;
            nifRow.Visibility = Visibility.Collapsed;
            mobileRow.Visibility = Visibility.Collapsed;
            articleRow.Visibility = Visibility.Collapsed;
            addressRow.Visibility = Visibility.Collapsed;
            activityRow.Visibility = Visibility.Collapsed;
            activityCodeInput.Visibility = Visibility.Collapsed;

            newClientButton.Visibility = Visibility.Visible;
            oldClientButton.Visibility = Visibility.Collapsed;

            this.newClient = false;
        }

        private void ProformaButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Etes vous sur de vouloir enregistrer cette facture ?","Attention !", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                if ((nameInput.Text == "" ||
                 nifInput.Text == "" ||
                 mobileInput.Text == "" ||
                 articleInput.Text == "" ||
                 addressInput.Text == "" ||
                 activityInput.Text == "" ||
                 emailInput.Text == "") && newClient)

                    MessageBox.Show("Veuillez remplir tout les champs !");

                else
                {

                    Client client = new Client();

                    if (!newClient)
                    {
                        client = clientService.GetClientByEmail(emailInput.Text);
                    }
                    else
                    {
                        string clientActivity = "";

                        if (artisant.IsSelected)
                            clientActivity = "Artisant";
                        else
                            clientActivity = activityCodeInput.Text;
                        

                        client = new Client(clientActivity, addressInput.Text, articleInput.Text, emailInput.Text
                            , mobileInput.Text, nameInput.Text, nifInput.Text, regInput.Text, cartArtInput.Text);

                        MessageBoxResult messageBoxResult = MessageBox.Show("Voulez vous enregistrer le nouveau client ?", "Confirmer les information client", MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            if (clientService.AddClient(client))
                                MessageBox.Show("Nouveau client enregistré ! \n nom : " + client.Name);
                            else
                                MessageBox.Show("Attention ! \n Erreur d'enregistrement client ! \n Message : \n " + clientService.getMessage());
                        }
                        else
                            MessageBox.Show("Attention ! \n le client n'a pas été enregistré . \n nom : " + client.Name);

                    }

                    if (!Double.TryParse(payedInput.Text, out Double payed))
                        MessageBox.Show("Attention ! \n Veuillez entrer un motant payé valide !");
                    else
                    {
                        string payMeth = ((ComboBoxItem)methodInput.SelectedItem).Content.ToString() + " " + modPayInput.Text;

                        this.proforma = new Facture(DateTime.Now, payMeth, payed, "Proforma", false, "Facture Proforma", null, client);

                        String garantie = ((ComboBoxItem)garantieInput.SelectedItem).Content.ToString();

                        if (Double.TryParse(remiseInput.Text, out Double remise)
                                 && Double.TryParse(timbreInput.Text, out Double timbre))
                        {
                            proforma.Remise = remise;
                            proforma.Timbre = timbre;
                            proforma.Waranty = garantie;

                            if (factureService.Save(proforma))
                            {
                                foreach (Machine machine in cartMachines)
                                {
                                    machine.State = "Sous Proforma";
                                    machine.IdFacture = proforma.IdFacture;
                                    machine.TypeFacture = proforma.Type;
                                    service.UpdateMachine(machine);
                                }

                                MessageBox.Show("Nouvelle proforma enregistré !");

                                PrintPreview printWindow = new PrintPreview(proforma, cart);
                                printWindow.Show();
                                cart.Clear();
                                cartMachines.Clear();
                            }
                            else
                                MessageBox.Show("Attention ! \n Erreur d'enregistrement facture ! \n Message : \n " + factureService.getMessage());
                        }
                        else
                            MessageBox.Show("Veuillez verifier la remise et les droits de timbre entrés");
                    }
                }
            }
        }

         public void ViderButton_Click (Object sender , RoutedEventArgs eventArgs)
         {
            nameInput.Text = "";
            regInput.Text = "";
            nifInput.Text = "";
            mobileInput.Text = "";
            articleInput.Text = "";
            addressInput.Text = "";
            activityInput.Text = "";
            emailInput.Text = "";
            activityInput.Text = "";
         }

        private void RefferenceFilter_Changed(object sender, TextChangedEventArgs e)
        {
            FillDataGrid();
        }

        private void GarantieChanged(object sender, SelectionChangedEventArgs e)
        {
          
        }

        private void ActivityChanged(object sender, SelectionChangedEventArgs e)
        {
            if (artisant.IsSelected == true)
            {
                activityCodeRow.Visibility = Visibility.Collapsed;
                activityCodeInput.Text = "";

                regInput.Visibility = Visibility.Collapsed;
                regLabel.Visibility = Visibility.Collapsed;
                regInput.Text = "";

                cartArtInput.Visibility = Visibility.Visible;
                cartArtLabel.Visibility = Visibility.Visible;
            }
            else
            {
                activityCodeRow.Visibility = Visibility.Visible;

                regInput.Visibility = Visibility.Visible;
                regLabel.Visibility = Visibility.Visible;

                cartArtInput.Visibility = Visibility.Collapsed;
                cartArtLabel.Visibility = Visibility.Collapsed;
                cartArtInput.Text = "";
            }
                    
            
        }

        private void EmptyCart_Click(object sender, RoutedEventArgs e)
        {
            cart.Clear();
        }
    }
}
