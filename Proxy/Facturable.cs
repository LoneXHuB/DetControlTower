using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Facturable
    {
        protected int id;
        protected string idFacture;
        protected string typeFacture;
        protected string designation;
        protected double pdv;
        protected Int32 quantity = 1;

        public string TypeFacture { set; get; }
        public string IdFacture { get; set; }
        public Int32 Quantity { set; get; }
        public int Id { get; set; }
        public string Designation { get; set; }
        public double Pdv { get; set; }

        public Facturable() { }

        public Facturable(double prix)
        {
            Pdv = prix;
        }

        public Facturable(string designation)
        {
            Designation = designation;
        }

        public Facturable(string designation, double prix) : this(designation)
        {
            Pdv = prix;
        }

        public Facturable(int id, string designation, double prix) : this(designation , prix)
        {
            Id = id;
        }
    }
}
