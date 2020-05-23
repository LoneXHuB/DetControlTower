using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Tache
    {
        private int id;
        private string designation;
        private double prix;
        DateTime dateCreation = DateTime.Now;

        public DateTime DateCreation { set; get; }
        public int Id { get; set; }
        public string Designation { get; set; }
        public double Prix { get; set; }

        public Tache(string designation, double prix)
        {
            Designation = designation;
            Prix = prix;
        }

        public Tache(int id, string designation, double prix) : this(designation , prix)
        {
            Id = id;
        }
    }
}
