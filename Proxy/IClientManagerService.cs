using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Collections.ObjectModel;

namespace Proxy
{
    public interface IClientManagerService
    {
        string getMessage();
        ObservableCollection<String> ClientList();
        Boolean AddClient(Client client);
        Client GetClientByEmail(String email);
    }
}
