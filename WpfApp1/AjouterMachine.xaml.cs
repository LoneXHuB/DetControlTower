using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
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
    /// <summary>
    /// Logique d'interaction pour AjouterOuvrage.xaml
    /// </summary>
    public partial class AjouterMachine : Page
    {
        private ObservableCollection<String> providerList;

        public ObservableCollection<String> ProviderList { get=> providerList ; set => providerList = value; }

        private ObservableCollection<String> refList;

        public ObservableCollection<String> RefList { get; set; }

        public static string lastMachineRef = "";
        public int AddCount = 1;

        IMachineManagerService service = (IMachineManagerService)Activator
                                               .GetObject(typeof(IMachineManagerService), "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public AjouterMachine()
        {
            this.ProviderList = service.ProviderList();
            this.RefList = service.RefList();
            InitializeComponent();

        }

        private async void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
           if( (refInput.Text == "" || desInput.Text == ""
                || arrivalInput.Text == "" || categInput.Text == "" 
                || providerInput.Text == "" || serialInput.Text == ""
                || pdrInput.Text == "" || pdvInput.Text == "" || dateInput.Text == "" ) && IsNewCheckBox.IsChecked.Value)
           {

                this.ShowErrorMessage("Veuillez remplir Tout les champs !");
                return;
           }
           if(!IsNewCheckBox.IsChecked.Value)
            {
                AddExistingMachine();
                return;
            }


            if (!Double.TryParse(pdrInput.Text , NumberStyles.Number, CultureInfo.InvariantCulture, out Double pdr) 
                || !Double.TryParse(pdvInput.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Double pdv))
                MessageBox.Show("Entré incorrect! \n le prix de revient et le prix de vente doivent etre des chiffres");

            else if(pdr >= pdv)
            {
                MessageBox.Show("Veuillez introduire un prix de vente avec une marge bénéficiaire par rapport au prix de revient !");
            }

            else
            {
                Machine machine = new Machine(serialInput.Text, categInput.Text, desInput.Text, "disponible", providerInput.Text
                    , arrivalInput.Text, pdr , pdv, refInput.Text, dateInput.DisplayDate, null);
                
                int qte = 1;
                Int32.TryParse(QteInput.Text, out qte);

                AddMachineByQte(machine, qte);
            }
        }

        private async void AddMachineByQte(Machine machine, int qte)
        {
            for (int i = 0; i < qte; i++)
            {
                if (machine.Refference.Equals(lastMachineRef))
                    AddCount++;
                else
                {
                    lastMachineRef = machine.Refference;
                    AddCount = 1;
                }
                if (service.AddMachine(machine))
                {
                    this.ProviderList = service.ProviderList();
                    await Task.Run(() =>
                    {
                        Dispatcher.Invoke(() => { this.ShowSuccessMessage("Succès! \n(" + AddCount + ") machines : " + lastMachineRef + " Ajoutées"); });
                    });
                }
                else
                {
                    AddCount = 1;
                    this.ShowErrorMessage(service.getMessage());
                }
            }
        }

        private void AddExistingMachine()
        {
            //if machine isn't new only check those three
            if ((refInput.Text == "" || arrivalInput.Text == "" || serialInput.Text == "" || dateInput.Text == "" ) && !IsNewCheckBox.IsChecked.Value)
            {

                this.ShowErrorMessage("Veuillez remplir Tout les champs !");
                return;
            }
            else
            {
                Machine machine = service.GetMachineByRef(refInput.Text);
                machine.ArrivalNumber = arrivalInput.Text;
                machine.ArrivalDate = dateInput.DisplayDate;
                machine.Serial = serialInput.Text;

                if (machine.Id == "" || machine.Id == null)
                {
                    this.ShowErrorMessage("La machine " + refInput.Text + " n'est pas encore en stock. \n Veuillez remplir tout les champs.");
                    IsNewCheckBox.IsChecked = true;
                    return;
                }
                int qte = 1;
                Int32.TryParse(QteInput.Text, out qte);

                AddMachineByQte(machine, qte);
            }
        }

        private void ViderButton_Click(object sender, RoutedEventArgs e)
        {
            refInput.Text = "";
            desInput.Text = "";
            arrivalInput.Text = "";
            categInput.Text = "";
            providerInput.Text = "";
            pdrInput.Text = "";
            pdvInput.Text = "";
            dateInput.Text = "";
            serialInput.Text = "";
        }
     
        private void ShowErrorMessage( String message )
        {
            InputExceptionFlag.Visibility = Visibility.Visible;
            InputExceptionFlag.Text = message;
            WarningIcon.Visibility = Visibility.Visible;
            DoneIcon.Visibility = Visibility.Collapsed;
        }

        private void ShowSuccessMessage(String message)
        {
            InputExceptionFlag.Visibility = Visibility.Visible;
            InputExceptionFlag.Text = message;
            WarningIcon.Visibility = Visibility.Collapsed;
            DoneIcon.Visibility = Visibility.Visible;
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new AjouterMachine();
        }

        private void IsNew_Checked(object sender, RoutedEventArgs e)
        {
            DesRow.Visibility = Visibility.Visible;
            CategRow.Visibility = Visibility.Visible;
            ProviderRow.Visibility = Visibility.Visible;
            PdrRow.Visibility = Visibility.Visible;
            PdvRow.Visibility = Visibility.Visible;
        }

        private void IsNew_Unchecked(object sender, RoutedEventArgs e)
        {
            DesRow.Visibility = Visibility.Collapsed;
            CategRow.Visibility = Visibility.Collapsed;
            ProviderRow.Visibility = Visibility.Collapsed;
            PdrRow.Visibility = Visibility.Collapsed;
            PdvRow.Visibility = Visibility.Collapsed;
        }

        private void Ref_TextChanged(object sender, TextChangedEventArgs e)
        {
           bool newMachine = !RefList.Any((refference => refference.StartsWith(refInput.Text)));
            if(newMachine && refInput.Text != "")
                IsNewCheckBox.IsChecked = true;

        }
    }
}
