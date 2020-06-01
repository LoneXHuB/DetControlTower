using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
     public interface IFactureManagerService
     {
        Boolean Save(Facture facture);
        String getMessage();
        int getLastIdFacture(string type);
        DataTable getFilteredFactureList(Facture facture);
        bool EditFacture(Facture facture);
        ObservableCollection<Machine> GetFactureMachineList(Facture facture);
        ObservableCollection<Tache> GetFactureTacheList(Facture facture);
    }
}
