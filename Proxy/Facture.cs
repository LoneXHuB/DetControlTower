using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Facture
    {
        private string idFacture;
        private DateTime dateFacture;
        private String payMethod;
        private double payed;
        private String state;
        private Boolean trial;
        private String type;
        private Bon bon;
        private Client client;
        private static IFactureManagerService factureService = (IFactureManagerService)Activator.GetObject(typeof(IFactureManagerService),
                                                                                   "tcp://"+ MyGeneralConstants.Host+ ":2019/FactureManagerService");
    

        public Facture( DateTime dateFacture, string payMethod
            , double payed, string state, bool trial, string type, Bon bon, 
            Client client)
        {
            this.DateFacture = dateFacture;
            this.PayMethod = payMethod;
            this.Payed = payed;
            this.State = state;
            this.Trial = trial;
            this.Type = type;
            this.Bon = bon;
            this.Client = client;
            this.UpdateId();
        }

        public Facture(String idFacture)
        {
            this.IdFacture = idFacture;
            this.Client = new Client();
        }

        public Facture()
        {
            this.Client = new Client();
        }

        public void UpdateId()
        {
            this.IdFacture = (Facture.factureService.getLastIdFacture(this.Type) + 1) +"/"+ DateTime.Now.Year;

        }
    public string IdFacture { get => idFacture; set => idFacture = value; }
        public DateTime DateFacture { get => dateFacture; set => dateFacture = value; }
        public string PayMethod { get => payMethod; set => payMethod = value; }
        public double Payed { get => payed; set => payed = value; }
        public string State { get => state; set => state = value; }
        public bool Trial { get => trial; set => trial = value; }
        public string Type { get => type; set => type = value; }
        public Bon Bon { get => bon; set => bon = value; }
        public Client Client { get => client; set => client = value; }
        public string Waranty { get; set; }
        public double Timbre { get; set; }
        public double Remise { get; set; }
    }
}
