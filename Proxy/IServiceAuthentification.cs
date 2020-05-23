using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proxy
{
    public interface IServiceAuthentification
    {
        Boolean Authentifier(String user, String pass);
        Boolean AuthentifierBiblio(String user, String pass);
        String getMessage();


    }
}
