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
    /// Logique d'interaction pour ConsulterOuvrage.xaml
    /// </summary>
    public partial class ConsulterOuvrage : Page
    {
        IServiceEmpConsulter service = (IServiceEmpConsulter)Activator.GetObject(typeof(IServiceEmpConsulter),
                                                                          "tcp://127.0.0.1:2019/ServiceEmpConsulter");
        string pseudo;
        public ConsulterOuvrage(string pseudo)
        {
            this.pseudo=pseudo;
            InitializeComponent();
        }
        public void FillDataGrid()
        {
            String barcode = barcodeFilterInput.Text;
            String titre = titreFilterInput.Text;
            String auteur = auteurFilterInput.Text;
            String theme = themeFilterInput.Text;
            String type = typeFilterInput.Text;


            Ouvrage filtre = new Ouvrage("" + titre, "" + auteur, "" + theme, "" + type, "" + barcode, 0);
            DataTable dataTable = service.FilterConsulterOuvrage(filtre, true);
           

            ouvragesDataGrid.DataContext = dataTable;

            

        }


        private void PageLoaded(object sender, RoutedEventArgs e)
        {

            this.FillDataGrid();

        }
        private void FilterActivated(object sender, TextChangedEventArgs e)
        {
            this.FillDataGrid();
        }

        private void ReinitFilter_Click(object sender, RoutedEventArgs e)
        {
            barcodeFilterInput.Text = "";
            titreFilterInput.Text = "";
            auteurFilterInput.Text = "";
            themeFilterInput.Text = "";
            typeFilterInput.Text = "";
        }

        private void ReserverButton_Click(object sender, RoutedEventArgs e)
        {
            IList dgr = ouvragesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {

                string barcode = row[0].ToString();
                


                Emprunt emprunt = new Emprunt(pseudo, DateTime.Now.ToString("yyyy-MM-dd-hh-mm"), DateTime.Now.AddDays(1).ToString("yyyy-MM-dd-hh-mm"), barcode, "");



                if (service.Reserver(emprunt))
                    MessageBox.Show("Ouvrage Reservé!\n Barcode : " + barcode);
                else
                    MessageBox.Show("Reservation impossible !\n Barcode : " + barcode + "\n Error : " + service.getMessage());

            }

            this.FillDataGrid();
        }
    }
}
