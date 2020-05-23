using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Proxy;
using DAO;

namespace Server
{
    class ClientManagerService : MarshalByRefObject, IClientManagerService
    {
        private DataAccessObject dao = new DataAccessObject();
        private String message;

        public string Message { get => message; set => message = value; }

        public string getMessage() { return this.Message; }


        public bool AddClient(Client client)
        {
            Boolean added = dao.AddClient(client);
            this.Message = dao.Message;

            return added;
        }

        public ObservableCollection<string> ClientList()
        {
            return dao.ClientList();
        }

        public Client GetClientByEmail(string email)
        {
            return dao.GetClientByEmail(email);
        }
    }
}
