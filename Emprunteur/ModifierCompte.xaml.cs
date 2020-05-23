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
using System.Runtime.Remoting;
using System.Data;
using System.Collections;


namespace Emprunteur
{
    /// <summary>
    /// Logique d'interaction pour ModifierCompte.xaml
    /// </summary>
    public partial class ModifierCompte : Page
    {
        IServiceEmpGererCompte service = (IServiceEmpGererCompte)Activator.GetObject(typeof(IServiceEmpGererCompte),
                                                                   "tcp://127.0.0.1:2019/ServiceEmpGererCompte");
        public string pseudo;

        public ModifierCompte(string pseudo)
        {
            this.pseudo = pseudo;
            InitializeComponent();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            IList dgr = comptesDataGrid.SelectedItems;

            foreach (DataRowView row in dgr)
            {
               
                string nom = row[1].ToString();
                string email = row[2].ToString();
                

                Proxy.Emprunteur compte = new Proxy.Emprunteur(nom, "", email, this.pseudo, "");




                if (service.ModifierCompte(compte))
                    MessageBox.Show("Compte Modifié!\n Pseudo : " + pseudo);
                else
                    MessageBox.Show("Modification impossible !\n Pseudo : " + pseudo + "\n Error : " + service.getMessage());

            }

            this.FillDataGrid();
        }

        public void FillDataGrid()
        {
          

            Enseignant enseignantfiltre = new Enseignant();

            Etudiant etudiantfiltre = new Etudiant("", "" , "", this.pseudo , "", "" , "", "");

            try
            {
                enseignantfiltre = new Enseignant("" , "", "" , this.pseudo , "", 0, "");
            }
            catch (Exception e)
            {
                enseignantfiltre = new Enseignant("" , "", "" , this.pseudo , "", 0, "");
            }

        

            DataTable dataTable = service.FilterSearchEmp(etudiantfiltre, enseignantfiltre);



            comptesDataGrid.DataContext = dataTable;

        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {

            this.FillDataGrid();

        }

    }
}
