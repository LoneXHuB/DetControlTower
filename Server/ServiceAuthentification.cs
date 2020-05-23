using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAO;
using Proxy;


namespace Server
{
    public class ServiceAuthentification : MarshalByRefObject, IServiceAuthentification
    {
           private String message;

        public string Message { get => message; set => message = value; }

        public string getMessage() { return this.Message; }

        public bool AuthentifierBiblio(String username, String pass)
        {
            if (username.Equals("Ahmed") && pass.Equals("123"))
                return true;
            else
                return false;

        }

        public bool Authentifier(string user, string pass)
        {
            throw new NotImplementedException();
        }
    }
}
