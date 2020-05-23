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
        
        
        public static string lastMachineRef = "";
        public int AddCount = 1;

        IMachineManagerService service = (IMachineManagerService)Activator
                                               .GetObject(typeof(IMachineManagerService), "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public AjouterMachine()
        {
            this.ProviderList = service.ProviderList();
           
            InitializeComponent();

        }


        private async void AjouterButton_Click(object sender, RoutedEventArgs e)
        {

           if( refInput.Text == "" || desInput.Text == ""
                || arrivalInput.Text == "" || categInput.Text == "" 
                || providerInput.Text == "" || serialInput.Text == ""
                || pdrInput.Text == "" || pdvInput.Text == ""  )
             {


                this.ShowErrorMessage("Veuillez remplir Tout les champs !");

            }
           
            else
            {
               
                if (!Double.TryParse(pdrInput.Text , NumberStyles.Number, CultureInfo.InvariantCulture, out Double pdr) || !Double.TryParse(pdvInput.Text, NumberStyles.Number, CultureInfo.InvariantCulture, out Double pdv))
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
                            await Task.Run(() => { Dispatcher.Invoke(() => { this.ShowSuccessMessage("Succès! \n(" + AddCount + ") machines : " + lastMachineRef + " Ajoutées"); });
                            });
                          

                        }


                        else
                        {
                                AddCount = 1;
                                this.ShowErrorMessage(service.getMessage());

                        }

                    }
                       

                    
                }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TitreInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void QuantiteInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = new AjouterMachine();
        }
    }
}
