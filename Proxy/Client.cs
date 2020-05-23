using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    [Serializable]
    public class Client
    {
        
        private String activity;
        private String address;
        private String article;
        private String email;
        private String mobile;
        private String name;
        private String nif;
        private String regNumber;
        private string cartArt;

        public Client()
        {
        }

        public Client(string activity, string address
            , string article, string email, string mobile
            , string name, string nif, string regNumber , string cartArt)
        {
          
            this.Activity = activity;
            this.Address = address;
            this.Article = article;
            this.Email = email;
            this.Mobile = mobile;
            this.Name = name;
            this.Nif = nif;
            this.RegNumber = regNumber;
            this.CartArt = cartArt;
        }

        public string CartArt { get => cartArt; set => cartArt = value; }
        public string Activity { get => activity; set => activity = value; }
        public string Address { get => address; set => address = value; }
        public string Article { get => article; set => article = value; }
        public string Email { get => email; set => email = value; }
        public string Mobile { get => mobile; set => mobile = value; }
        public string Name { get => name; set => name = value; }
        public string Nif { get => nif; set => nif = value; }
        public string RegNumber { get => regNumber; set => regNumber = value; }
    }
}
