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
    public partial class AjouterPiece : Page
    {
        private ObservableCollection<String> pieceList;

        public ObservableCollection<String> PieceList { get => pieceList; set => pieceList = value; }

        IMachineManagerService service = (IMachineManagerService)Activator
                                       .GetObject(typeof(IMachineManagerService), "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");

        public static string lastMachineRef = "";
        public int AddCount = 1;

        public AjouterPiece()
        {
            this.PieceList = service.PieceList();

            InitializeComponent();
        }

     private async void AjouterButton_Click(object sender, RoutedEventArgs e)
     {

        if (refInput.Text == "" || descInput.Text == ""
             || QteInput.Text == "")
        {
            this.ShowErrorMessage("Veuillez remplir Tout les champs !");
        }

        else
        {
            int qte = 1;
            bool correctQteInput = Int32.TryParse(QteInput.Text, out qte);

                if(!correctQteInput)
                {
                    this.ShowErrorMessage("Le champ Quantité doit etre un chiffre entier");
                    return;
                }

            Piece piece = new Piece(refInput.Text , descInput.Text , DateTime.Now);

            for (int i = 0; i < qte; i++)
            {
                if (piece.Refference.Equals(lastMachineRef))
                    AddCount++;
                else
                {
                    lastMachineRef = piece.Refference;
                    AddCount = 1;
                }

                if (service.AddPiece(piece))
                {
                    this.PieceList = service.PieceList();
                    await Task.Run(() => {
                        Dispatcher.Invoke(() => { this.ShowSuccessMessage("Succès! \n(" + AddCount + ") Pièce(s) : " + lastMachineRef + " Ajoutées"); });
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

    private void ViderButton_Click(object sender, RoutedEventArgs e)
    {
        refInput.Text = "";
        descInput.Text = "";
        QteInput.Text = "";
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
