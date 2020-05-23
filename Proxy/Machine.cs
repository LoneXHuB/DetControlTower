using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Machine
    {
        private DateTime arrivalDate;
        private String categ;
        private String designation;
        private String state;
        private String nameF;
        private String arrivalNumber;
        private double pdr;
        private double pdv;
        private String refference;
        private String serial;
        private Facture facture;
        private String id;
        private int quantity = 1 ;
        private string idFacture ="none";
        private String remarque;

            public Machine(string serial , string categ,
            string designation, string state, string nameF, 
            string arrivalNumber, double pdr, double pdv,
            string refference, DateTime arrivalDate, Facture facture)

        {
            this.arrivalDate = arrivalDate;
            this.categ = categ;
            this.designation = designation;
            this.state = state;
            this.nameF = nameF;
            this.arrivalNumber = arrivalNumber;
            this.pdr = pdr;
            this.pdv = pdv;
            this.refference = refference;
            this.serial = serial;
            this.Facture = facture;
        }

        public Machine(string id)
        {
            this.Id = id;
        }
        public Machine(string id, string referance)
        {
            this.Id = id;
            this.Refference = referance;
        }
        public Machine(string id, string referance, double pdv)
        {
            this.Id = id;
            this.Refference = referance;
            this.Pdv = pdv;
        }
        public Machine(string id, string referance,string provider , double pdv)
        {
            this.Id = id;
            this.Refference = referance;
            this.Pdv = pdv;
            this.NameF = provider;
        }
        public Machine(string id, string categ,
         string designation, string nameF, string arrivalNumber, string refference)

        {

            this.categ = categ;
            this.designation = designation;
            this.nameF = nameF;
            this.arrivalNumber = arrivalNumber;
            this.refference = refference;
            this.Facture = facture;
        }
        public Machine( string id,string categ , string designation, string refference)

        {

            this.categ = categ;
            this.Id = id;
            this.Designation = designation;
            this.refference = refference;
           
        }

        public Machine() { }

        public DateTime ArrivalDate { get => arrivalDate ; set => arrivalDate = value; }
        public String Categ { get => categ; set => categ = value; }
        public String Designation { get => designation; set => designation = value; }
        public String State { get => state; set => state = value; }
        public string NameF { get => nameF; set => nameF = value; }
        public string ArrivalNumber { get => arrivalNumber; set => arrivalNumber = value; }
        public double Pdr { get => pdr; set => pdr = value; }
        public double Pdv { get => pdv; set => pdv = value; }
        public string Refference { get => refference; set => refference = value; }
        public string Serial { get => serial; set => serial = value; }
        public Facture Facture { get => facture; set => facture = value; }
        public String Id { get => id; set => id = value; }
        public int Quantity { get => quantity; set => quantity = value; }
        public string IdFacture { get => idFacture; set => idFacture = value; }
        public string TypeFacture { get; set; }
        public string Remarque { get => remarque; set => remarque = value; }
    }
}
