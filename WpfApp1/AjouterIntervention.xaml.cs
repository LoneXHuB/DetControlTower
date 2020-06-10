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

namespace DetControlTower
{
    /// <summary>
    /// Logique d'interaction pour AjouterPiece.xaml
    /// </summary>
    public partial class AjouterIntervention : Page
    {
        private ObservableCollection<String> tacheList;

        public ObservableCollection<String> TacheList { get => tacheList; set => tacheList = value; }

        IMachineManagerService service = (IMachineManagerService)Activator
                                       .GetObject(typeof(IMachineManagerService), "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public static string lastTacheDesignation = "";
        public int AddCount = 1;

        public AjouterIntervention()
        {
            this.TacheList = service.TacheList();

            InitializeComponent();
        }

        private async void AjouterButton_Click(object sender, RoutedEventArgs e)
        {

            if (designationInput.Text == "" || prixInput.Text == "")
            {
                this.ShowErrorMessage("Veuillez remplir Tout les champs !");
                return;
            }

            else
            {
                double prixTache;

                if(!Double.TryParse(prixInput.Text , out prixTache))
                {
                    this.ShowErrorMessage("Le champ prix doit etre un chiffre");
                    return;
                }

                Tache tache = new Tache(designationInput.Text, prixTache);

                if (tache.Designation.Equals(lastTacheDesignation))
                    AddCount++;
                else
                {
                    lastTacheDesignation = tache.Designation;
                    AddCount = 1;
                }

                if (service.AddTache(tache))
                {
                    this.TacheList = service.TacheList();
                    await Task.Run(() => {
                        Dispatcher.Invoke(() => { this.ShowSuccessMessage("Succès! \n(" + AddCount + ") Tache(s) : " + lastTacheDesignation + " Ajoutées"); });
                    });
                }

                else
                {
                    AddCount = 1;
                    this.ShowErrorMessage(service.getMessage());
                }
            }
        }

        private void ViderButton_Click(object sender, RoutedEventArgs e)
        {
            designationInput.Text = "";
            prixInput.Text = "";
        }

        private void ShowErrorMessage(String message)
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
            DataContext = new AjouterPiece();
        }
    }
}
