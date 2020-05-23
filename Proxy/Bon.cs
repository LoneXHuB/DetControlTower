using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Bon
    {

        private int idBon;
        private String driverName;
        private String driverMobile;
        private String matricule;
        private Client client;

        public Bon(int idBon, string driverName, string driverMobile, string matricule, Client client)
        {
            this.IdBon = idBon;
            this.DriverName = driverName;
            this.DriverMobile = driverMobile;
            this.Matricule = matricule;
            this.Client = client;
        }

        public int IdBon { get => idBon; set => idBon = value; }
        public string DriverName { get => driverName; set => driverName = value; }
        public string DriverMobile { get => driverMobile; set => driverMobile = value; }
        public string Matricule { get => matricule; set => matricule = value; }
        internal Client Client { get => client; set => client = value; }
    }
}
