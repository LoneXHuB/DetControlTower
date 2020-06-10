using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;


namespace Server
{
    class MainClass
    {
        public static void Main(String[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("---------------------------------");
            Console.WriteLine("WARNING ! XEN SERVER UNLEASHED !");
            Console.WriteLine("---------------------------------");

            try { 

                TcpChannel channel = new TcpChannel(2019);
                ChannelServices.RegisterChannel(channel, false);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(ServiceAuthentification), "ServiceAuthentification", WellKnownObjectMode.Singleton);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(MachineManagerService), "MachineManagerService", WellKnownObjectMode.Singleton);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(ClientManagerService), "ClientManagerService", WellKnownObjectMode.Singleton);

                RemotingConfiguration.RegisterWellKnownServiceType(typeof(FactureManagerService), "FactureManagerService", WellKnownObjectMode.Singleton);





                Console.WriteLine("RegisteredTypes : ");
                foreach ( WellKnownServiceTypeEntry obj in RemotingConfiguration.GetRegisteredWellKnownServiceTypes())
                    {
                        Console.WriteLine("Object : "+obj.ObjectType);
                    }
                Console.WriteLine("---------------------------");
                Console.WriteLine("");

                System.Console.WriteLine("Server Running ...");

                while (true) { }

            } catch(Exception e){
                System.Console.WriteLine("Server Runtime Exception ...");
                System.Console.WriteLine(e.Message);
            }

        }


        public void emailNotify()
        {

        }


    }
}
