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

namespace Emprunteur
{
    /// <summary>
    /// Logique d'interaction pour ListeReservations.xaml
    /// </summary>
    public partial class ListeReservations : Page
    {

        IServiceEmpConsulter service = (IServiceEmpConsulter)Activator.GetObject(typeof(IServiceEmpConsulter),
                                                                        "tcp://127.0.0.1:2019/ServiceEmpConsulter");

        private string pseudo;


        public ListeReservations(String pseudo)
        {
            this.pseudo = pseudo;
            InitializeComponent();
        }


        public void FillDataGrid()
        {





            Emprunt filtreEmprunt = new Emprunt( pseudo, "", "", "", "");
            DataTable dataTableEmprunt = service.FilterConsulterReservationEmp(filtreEmprunt);

            reservationDataGrid.DataContext = dataTableEmprunt;

        }


        private void PageLoaded(object sender, RoutedEventArgs e)
        {

            this.FillDataGrid();

        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)

        {
            IList dgr = reservationDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {

                string barcode = row[2].ToString();
                string pseudo = row[1].ToString();
                Emprunt emprunt = new Emprunt(pseudo, "", "", barcode, "");


                if (service.SupprimerReservation(emprunt, false))
                    MessageBox.Show(service.getMessage());
                else
                    MessageBox.Show("Suppression impossible !\n Barcode : " + barcode + "\n Error : " + service.getMessage());
            }

            this.FillDataGrid();




        }

        private void OuvragesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
