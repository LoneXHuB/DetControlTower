using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy;
using DAO;
using System.Data;
using System.Collections.ObjectModel;

namespace Server
{
    class FactureManagerService : MarshalByRefObject, IFactureManagerService
    {
        private DataAccessObject dao = new DataAccessObject();
        private String message;

        public string Message { get => message; set => message = value; }



        public int getLastIdFacture(string type)
        {
            return dao.getLastIdFacture(type);
        }

        public DataTable getFilteredFactureList(Facture filter)
        {
           
           return dao.getFilteredFactureList(filter);
        }

        public string getMessage() { return this.Message; }


        public bool Save(Facture facture)
        {
            Boolean saved = dao.Savefacture(facture);
            this.Message = dao.Message;

            return saved;

        }

      

        public bool EditFacture(Facture facture)
        {
            Boolean modified = dao.modifierFacture(facture);
            this.Message = dao.Message;

            return modified;
        }

        public ObservableCollection<Machine> GetFactureMachineList(Facture facture)
        {

            return dao.GetFactureMachineList(facture);

        }
    }
}
