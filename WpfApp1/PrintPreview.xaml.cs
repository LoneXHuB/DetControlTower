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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Window2.xaml
    /// </summary>
    public partial class PrintPreview : Window
    {
        private Facture facture;
        private ObservableCollection<Facturable> cart = new ObservableCollection<Facturable>();
      

        public Facture Facture { get => facture; set => facture = value; }
        public ObservableCollection<Facturable> Cart { get => cart; set => cart = value; }

        public PrintPreview(Facture facture, ObservableCollection<Tache> ListFacturable)
        {
            this.Facture = facture;
           
            foreach (Tache facturable in ListFacturable)
            {
                if (!this.Cart.Any(cartFacturable => cartFacturable.Designation == facturable.Designation))
                    this.Cart.Add(facturable);

                else
                {
                    foreach (Tache cartFacturable in this.Cart)
                    {
                        if (cartFacturable.Designation.Equals(facturable.Designation))
                        {
                            cartFacturable.Quantity++;
                            break;
                        }
                    }
                }
            }

            InitializeComponent();
        }

        public PrintPreview(Facture facture, ObservableCollection<Machine> ListFacturable)
        {
            this.Facture = facture;
            //check if the email should be displayed or not
            if (!facture.Client.Email.Contains("@"))
                facture.Client.Email = "/";
            foreach (Machine facturable in ListFacturable)
            {
                if (facturable.Refference != null)
                {
                    if (facturable.Refference.Contains("@"))
                        facturable.Refference = "";
                }
                ObservableCollection<Machine> castCart = new ObservableCollection<Machine>();

                if (!castCart.Any(cartFacturable => cartFacturable.Refference == facturable.Refference))
                {
                    Cart.Add(facturable);
                    castCart.Add(facturable);
                }

                else
                {

                    foreach (Machine cartFacturable in Cart)
                    {
                        if (cartFacturable.Refference.Equals(facturable.Refference))
                        {
                            cartFacturable.Quantity++;
                            break;
                        }
                    }
                }
            }

            InitializeComponent();
        }

        private void PrintPage()
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

        private Boolean SaveFactureShot(Boolean menuClear )
        {
            try
            {
                RenderTargetBitmap renderTarget = new RenderTargetBitmap(1860, 2630 
                                                                , 300, 300, System.Windows.Media.PixelFormats.Pbgra32);
                renderTarget.Render(printableGrid);

                string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\";
                string ImagePath = folder + facture.Type + facture.IdFacture.Replace('/', ' ')+".jpeg";

                using (FileStream stream = File.Create(ImagePath))
                {
                    JpegBitmapEncoder jpegEncoder = new JpegBitmapEncoder();
                    jpegEncoder.QualityLevel = 100;

                    jpegEncoder.Frames.Add(BitmapFrame.Create(renderTarget));

                    jpegEncoder.Save(stream);
                    stream.Close();
                }

                return menuClear;
            }
            catch(Exception e) {
                MessageBox.Show("Erreur de sauvegarde \n Error : " + e.Message);
                return false;
                    }
        }

        private Boolean SendFactureEmail(Boolean start)
        {
            try
            {
                MailMessage mail = new MailMessage();
                //put your SMTP address and port here.
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                //Put the email address
                mail.From = new MailAddress("det.noanswer@gmail.com");
                //Put the email where you want to send.
                mail.To.Add(facture.Client.Email);

                mail.Subject = "Votre facture vous a été envoyé !";

                StringBuilder sbBody = new StringBuilder();

                sbBody.AppendLine("Bonjour,");
                sbBody.AppendLine("Merci d'étre passé au siège DET ,");
                sbBody.AppendLine("Veuillez trouver ci jointe votre " + facture.Type);
                sbBody.AppendLine("Bien a vous .");
                sbBody.AppendLine("Bien a vous .");


                mail.Body = sbBody.ToString();
                string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\";
                string imagePath = folder + facture.Type + facture.IdFacture.Replace('/', ' ') + ".jpeg";
                //Your file path
                System.Net.Mail.Attachment factureImage = new System.Net.Mail.Attachment(@""+imagePath);


                mail.Attachments.Add(factureImage);

                //Your username and password!
                SmtpServer.Credentials = new System.Net.NetworkCredential("det.noanswer@gmail.com", "SDCZ40Open");
                //Set Smtp Server port
                SmtpServer.Port = 587;
                SmtpServer.EnableSsl = true;
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;

                SmtpServer.Send(mail);


                MessageBox.Show("Email envoyé!");
            return start;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Echec de l'envoie ! \n Erreur : " + ex.StackTrace +"With Message = " + ex.Message);
            return false;
            }
            finally
            {
                    this.Dispatcher.Invoke(() =>
                    {
                    LoadingCover.Visibility = Visibility.Collapsed;
                    });
            }
        }
  
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            printerControls.Visibility = Visibility.Collapsed;

            PrintPage();

            printerControls.Visibility = Visibility.Visible;
        }
        private Boolean MenuClearForEmail()
        {
            this.Dispatcher.Invoke(() => {
                LoadingCover.Visibility = Visibility.Visible;
                signature.Visibility = Visibility.Visible;
                printerControls.Visibility = Visibility.Collapsed;
            });
            return true;
        }
        private Boolean MenuRestore()
        {
            this.Dispatcher.Invoke(() => {
                LoadingCover.Visibility = Visibility.Collapsed;
            printerControls.Visibility = Visibility.Visible;
            signature.Visibility = Visibility.Collapsed;
            });
            return true;
        }

        private async void EmailButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult dialogResult = MessageBox.Show("Etes vous sur de vouloir envoyer cette facture par email?", "Attention !", MessageBoxButton.YesNo);

            if (dialogResult == MessageBoxResult.Yes)
            {
                Boolean menuClear = await Task.Run(() => MenuClearForEmail());
                Boolean start = await Task.Factory.StartNew(() => this.Dispatcher.Invoke(() => SaveFactureShot(menuClear)));
                await Task.Run(() => SendFactureEmail(start));
                MenuRestore();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            double totalCart = 0.0;

            foreach (Facturable facturable in Cart)
                totalCart += facturable.Pdv*facturable.Quantity;

            totalHt.Text = totalCart.ToString();
            clientAddress.Text = facture.Client.Address;
            clientArticle.Text = facture.Client.Article;
            clientEmail.Text = facture.Client.Email;
            ClientName.Text = facture.Client.Name;
            clientMobile.Text = facture.Client.Mobile;
            clientNif.Text = facture.Client.Nif;
            paymentType.Text = facture.PayMethod;
            date.Text = facture.DateFacture.ToString("yyyy-MM-dd");
            remise.Text = facture.Remise.ToString();
            totalHt2.Text = (totalCart - facture.Remise).ToString();
            tva.Text = (Math.Round((totalCart - facture.Remise) * 19 / 100 , 2)).ToString();
            double totalTTC = (((totalCart - facture.Remise) * 19 / 100) + (totalCart - facture.Remise) + facture.Timbre);
  
            total.Text = Math.Round(totalTTC, 2).ToString();
            garantie.Text = facture.Waranty;
            RegNumber.Text = facture.Client.CartArt;
            RegNumber.Text += facture.Client.RegNumber;
            numFacture.Text =facture.Type + " N°: " + facture.IdFacture;
            clientActivity.Text = facture.Client.Activity;

            amountInLetters.Text = totalTTC.ToLettres();

            
            cartDataGrid.ItemsSource = this.Cart;
        }

     
    }
}
