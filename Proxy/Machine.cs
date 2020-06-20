using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Machine : Facturable
    {
        private DateTime arrivalDate;
        private String categ;
        private String state;
        private String nameF;
        private String arrivalNumber;
        private double pdr;
        private String refference;
        private String serial;
        private Facture facture;
        private String remarque;
        
        public Machine(string serial , string categ,
        string designation, string state, string nameF, 
        string arrivalNumber, double pdr, double pdv,
        string refference, DateTime arrivalDate, Facture facture) : base (designation , pdv)
        {
            this.arrivalDate = arrivalDate;
            this.categ = categ;
            this.state = state;
            this.nameF = nameF;
            this.arrivalNumber = arrivalNumber;
            this.pdr = pdr;
            this.refference = refference;
            this.serial = serial;
            this.Facture = facture;
        }

        public Machine(int id)
        {
            base.Id = id;
        }
        public Machine(int id, string referance)
        {
            this.Id = id;
            this.Refference = referance;
        }
        public Machine(int id, string referance, double pdv) : base(pdv)
        {
            this.Id = id;
            this.Refference = referance;
        }
        public Machine(int id, string referance,string provider , double pdv) : base(pdv)
        {
            this.Id = id;
            this.Refference = referance;
            this.NameF = provider;
        }
        public Machine(string id, string categ,
         string designation, string nameF, string arrivalNumber, string refference) : base (designation)
        {
            this.categ = categ;
            this.nameF = nameF;
            this.arrivalNumber = arrivalNumber;
            this.refference = refference;
            this.Facture = facture;
        }
        public Machine( int id,string categ , string designation, string refference) : base (designation)
        {
            this.categ = categ;
            this.Id = id;
            this.refference = refference;  
        }

        public Machine() { base.Quantity = 1; }

        public DateTime ArrivalDate { get => arrivalDate ; set => arrivalDate = value; }
        public String Categ { get => categ; set => categ = value; }
        public String State { get => state; set => state = value; }
        public string NameF { get => nameF; set => nameF = value; }
        public string ArrivalNumber { get => arrivalNumber; set => arrivalNumber = value; }
        public double Pdr { get => pdr; set => pdr = value; }
        public string Refference { get => refference; set => refference = value; }
        public string Serial { get => serial; set => serial = value; }
        public Facture Facture { get => facture; set => facture = value; }
        public string Remarque { get => remarque; set => remarque = value; }
    }
}
