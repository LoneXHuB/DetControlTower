using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using Proxy;
using System.Resources;
using System.Diagnostics;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class PrintStockPreview : Window
    {
        
        private Facture facture;
        private ObservableCollection<Machine> stock ;
      

        public Facture Facture { get => facture; set => facture = value; }
        public ObservableCollection<Machine> Stock { get => stock; set => stock = value; }
      


        public PrintStockPreview(ObservableCollection<Machine> stock )
        {
            date.Text = DateTime.Now.ToLongDateString();
            user.Text = MyGeneralConstants.Host;
            totalMachines.Text = stock.Count.ToString();
            this.Stock = stock;
            InitializeComponent();
        }







        private async void PrintPage()
        {

            PrintDialog printer = new PrintDialog();
            if (printer.ShowDialog().GetValueOrDefault(false))
            {
                Grid printedGrid = printableGrid;
                //get selected printer capabilities
                System.Printing.PrintCapabilities capabilities = printer.PrintQueue.GetPrintCapabilities(printer.PrintTicket);

                //get scale of the print wrt to screen of WPF visual
                double scale = Math.Min(capabilities.PageImageableArea.ExtentWidth / printedGrid.ActualWidth,
                    capabilities.PageImageableArea.ExtentHeight / printedGrid.ActualHeight);

                //Transform the Visual to scale
                printedGrid.LayoutTransform = new ScaleTransform(scale, scale);

                //get the size of the printer page
                Size sz = new Size(capabilities.PageImageableArea.ExtentWidth, capabilities.PageImageableArea.ExtentHeight);

                //update the layout of the visual to the printer page size.
                printedGrid.Measure(sz);


                printedGrid.Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight), sz));


                //now print the visual to printer to fit on the one page.
                printer.PrintVisual(printedGrid, this.Title);


            }
            else
                MessageBox.Show("Attention ! \n La facture n'a pas été imprimé !");





        }





        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
          
            
            printerControls.Visibility = Visibility.Collapsed;

            foreach(ObservableCollection<Machine> page in this.GetSplitPages(Stock))
            {
                PrintStockPreview printablePage = new PrintStockPreview(page);
                printablePage.PrintPage();

            }




            printerControls.Visibility = Visibility.Visible;
        }
       
        private Boolean MenuRestore()
        {
            this.Dispatcher.Invoke(() => {
                LoadingCover.Visibility = Visibility.Collapsed;
            printerControls.Visibility = Visibility.Visible;
        
            });
            return true;

        }

        

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
           stockGrid.ItemsSource = this.Stock;
        }

        private void GridLoaded(object sender, RoutedEventArgs e)
        {
          //  stockGrid.RowHeight = 20;
        }


        private IEnumerable<ObservableCollection<Machine>> GetSplitPages(ObservableCollection<Machine> stockCollection)
        {
            IEnumerable<Machine> page = new List<Machine>();
            int count = 0;
            int index = 0;
            foreach (Machine machine in stockCollection)
            {
             
                page.Append(machine);
                DataGridRow rowView = (DataGridRow)stockGrid.Items[index];

                count += (int) rowView.ActualHeight;

                if(count >= 760)
                {
                    count = 0;
                    yield return (ObservableCollection<Machine>)page;
                }
                index++;
            }

            
        }
    }
}
