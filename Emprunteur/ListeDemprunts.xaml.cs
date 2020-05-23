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
    /// Logique d'interaction pour ListeDemprunts.xaml
    /// </summary>
    public partial class ListeDemprunts : Page
    {
        IServiceGererOuvrage service = (IServiceGererOuvrage)Activator.GetObject(typeof(IServiceGererOuvrage),
                                                                        "tcp://127.0.0.1:2019/ServiceGererOuvrage");

        private string pseudo;


        public ListeDemprunts(String pseudo)
        {
            this.pseudo = pseudo;
            InitializeComponent();
        }

        public void FillDataGrid()
        {
          

          
            

            Emprunt filtreEmprunt = new Emprunt(this.pseudo, "", "", "" , "");
            DataTable dataTableEmprunt = service.FilterConsulterEmprunt(filtreEmprunt);

            reservationDataGrid.DataContext = dataTableEmprunt;

        }


        private void PageLoaded(object sender, RoutedEventArgs e)
        {

            this.FillDataGrid();

        }

    }
}
