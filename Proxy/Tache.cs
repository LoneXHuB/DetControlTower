using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Tache : Facturable
    {
        private DateTime creationDate = DateTime.Now;

        public DateTime CreationDate { set; get; }

        public Tache() { base.Quantity = 1; }

        public Tache(string designation)
        {
            Designation = designation;
        }

        public Tache(string designation, double prix) : base(designation)
        {
            Pdv = prix;
        }

        public Tache(int id, string designation, double prix) : base(designation , prix)
        {
            Id = id;
        }
    }
}
