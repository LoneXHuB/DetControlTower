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

namespace DetControlTower
{
    /// <summary>
    /// Logique d'interaction pour StockPiece.xaml
    /// </summary>
    public partial class StockPiece : Page, idPiece
    {
        IMachineManagerService service = (IMachineManagerService)Activator.GetObject(typeof(IMachineManagerService),
                                                                         "tcp://" + MyGeneralConstants.Host + ":2019/MachineManagerService");
        private DataTable dataTable;

        public DataTable DataTable { get; set; }

        public StockPiece()
        {
            InitializeComponent();
        }

        public void FillDataGrid()
        {
            String refference = refFilterInput.Text;

            Piece filter = new Piece(refference, "");

            DataTable DataTable = service.GetPieceList(filter);


            ouvragesDataGrid.DataContext = DataTable;

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
            IList dgr = ouvragesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {

                int idPiece = Int32.Parse(row["id"].ToString());
                string reference = row["refference"].ToString();
                Piece piece = new Piece(idPiece, reference);


                if (service.RemovePiece(piece))
                    MessageBox.Show("Piece Supprimé!\n referance : " + reference);
                else
                    MessageBox.Show("Suppression impossible !\n ref : " + reference + "\n Error : " + service.getMessage());
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

        }
    }
}
