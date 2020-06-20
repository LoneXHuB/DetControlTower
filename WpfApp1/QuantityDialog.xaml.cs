using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DetControlTower
{
    /// <summary>
    /// Logique d'interaction pour QuantityDialog.xaml
    /// </summary>
    public partial class QuantityDialog : Window
    {
        private int qte;
        public int Qte { set; get; }

        public delegate void AddClickedEventHandler(object o, Int32 qte);
        public event AddClickedEventHandler AddClicked;

        public QuantityDialog()
        {
            InitializeComponent();
        }

        protected virtual void OnAddClicked()
        {
            if(AddClicked != null)
            {
                AddClicked(this, this.Qte);
            }
            this.Close();
        }

        private void Ajouter_Click(object sender , EventArgs e)
        {
            int quantity = 0;
            if (!Int32.TryParse(QteInput.Text, out quantity))
                MessageBox.Show("Attention ! \n entré incorrecte!");
            else
            {
                Qte = quantity;
                OnAddClicked();
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
