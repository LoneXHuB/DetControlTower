using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Proxy;

namespace DAO
{
    public class DataAccessObject
    {
        // private MySqlConnection cnx =  new Connection("remotemysql.com  ", "V7szH3JQU3", "IgoEOmSAwE", "V7szH3JQU3").Cnx;
        private MySqlConnection cnx = new Connection("localhost", "root", "admin", "detcontroltower").Cnx;
        private String message = "";

        public string Message { get => message; set => message = value; }
        
        public DataAccessObject() { }
        
        public void CheckConnection()
        {
            Console.WriteLine("Checking Connection Validity...");
            Console.WriteLine("Pinging database Server");
            
             cnx.Close();
            
            Console.WriteLine(cnx.Ping());
            Console.WriteLine(cnx.Ping());

            Console.WriteLine("Connecting to database ....");
            if (!cnx.Ping())
               cnx.Open();
            

        }
        
        public bool modifierFacture(Facture facture)
        {
            try
            {
                String query1 = String.Format("UPDATE facture SET state=\"{2}\" WHERE (id_facture=\"{0}\" AND type=\"{1}\")"
                                                                ,facture.IdFacture ,facture.Type, facture.State);

                Console.WriteLine("==>Query: " + query1);


                MySqlCommand command = new MySqlCommand(query1, cnx);
                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Facture Modifié : " + facture.IdFacture;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }

        public ObservableCollection<string> TacheList()
        {
            ObservableCollection<String> providerList = new ObservableCollection<String>();

            String query = " SELECT * FROM tache";
            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("==>Query: " + query);

            while (reader.Read())
            {
                String providerName = reader["designation"].ToString();

                providerList.Add(providerName);
            }
            cnx.Close();

            return providerList;
        }

        public DataTable GetTacheList(Tache filter)
        {
            String query = "SELECT * FROM tache";

            Console.WriteLine("==>Query: " + query);

            MySqlCommand command = new MySqlCommand(query, cnx);

            DataTable dataTable = new DataTable();
            this.CheckConnection();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);


            dataAdapter.Fill(dataTable);
            cnx.Close();
            return dataTable;
        }

        public bool AddTache(Tache tache)
        {
            String query = "INSERT INTO tache (designation , prix, date_creation) VALUES (\"" + tache.Designation + "\" , \"" + tache.Pdv + "\", \"" + tache.CreationDate.ToString("yyyy-MM-dd HH:mm:ss") + "\")";

            MySqlCommand command = new MySqlCommand(query, cnx);

            Console.WriteLine("==>Query: " + query);

            try
            {
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Ajouté avec succès!";

                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;

            }
        }

        public DataTable getFilteredFactureList(Facture filter)
        {
            String query = String.Format("SELECT * FROM facture LEFT JOIN client ON client_email = email" +
                    " WHERE ( ( id_facture LIKE \"{0}%\") AND name LIKE\"{1}%\") ORDER BY name", filter.IdFacture , filter.Client.Name);

            MySqlCommand command = new MySqlCommand(query, cnx);

            DataTable dataTable = new DataTable();
            this.CheckConnection();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            Console.WriteLine("==>Query: " + query);

            dataAdapter.Fill(dataTable);

            cnx.Close();
            return dataTable;
        }

        public Boolean AddMachine(Machine machine)
        {
            String query = String.Format("INSERT INTO machine  (serial , arrival , categ , designation , etat , namef , num_arrivage ," +
                "pdr , pdv , ref ,remarque) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\" ,\"rien a signaler\")"
            , machine.Serial, machine.ArrivalDate.ToString("yyyy-MM-dd"), machine.Categ, machine.Designation, machine.State, machine.NameF, machine.ArrivalNumber
            , machine.Pdr.ToString("F4", CultureInfo.InvariantCulture), machine.Pdv.ToString("F4", CultureInfo.InvariantCulture), machine.Refference);

            MySqlCommand command = new MySqlCommand(query, cnx);

            Console.WriteLine("==>Query: " + query);
            try
            {
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Ajouté avec succès!";

                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;

            }

        }
        public Boolean AddPiece(Piece piece)
        {
            String query = "INSERT INTO piece_rechange (refference , description , date_ajout) VALUES (\"" + piece.Refference + "\" , \"" + piece.Description+ "\", \"" + piece.DateAjout.ToString("yyyy-MM-dd HH:mm:ss") + "\")";

            MySqlCommand command = new MySqlCommand(query, cnx);

            Console.WriteLine("==>Query: " + query);
            
            try
            {
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Ajouté avec succès!";

                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;

            }
        }

        public ObservableCollection<String> ProviderList()
        {
            ObservableCollection<String> providerList = new ObservableCollection<String> ();

            String query = " SELECT * FROM machine";
            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();
            Console.WriteLine("==>Query: " + query);

            while (reader.Read())
            {
                String providerName = reader["nameF"].ToString();

                providerList.Add(providerName);
            }
            cnx.Close();

            return providerList;
        }

        public DataTable GetMachineList(Machine filter, Boolean availableOnly)
        {
            String query = "";

            if (availableOnly)
            {
                query = String.Format("SELECT * FROM machine WHERE NOT etat=\"Facturé\"" +
                                          "AND ( ref LIKE \"{0}%\" OR ref IS NULL )" +
                                          " AND (categ LIKE \"{1}%\" OR categ IS NULL) " +
                                          "AND (namef LIKE \"%{2}%\" OR namef IS NULL) " +
                                          "AND (num_arrivage LIKE \"%{3}%\" OR num_arrivage IS NULL) ", filter.Refference
                                          , filter.Categ, filter.NameF, filter.ArrivalNumber);
            }
            else
            {
                query = String.Format("SELECT * FROM machine WHERE ( (ref LIKE \"{0}%\" OR ref IS NULL)" +
                                          " AND (categ LIKE \"{1}%\" OR categ IS NULL) " +
                                          "AND (namef LIKE \"%{2}%\" OR namef IS NULL) " +
                                          "AND (num_arrivage LIKE \"%{3}%\" OR num_arrivage IS NULL) )", filter.Refference
                                     , filter.Categ, filter.NameF, filter.ArrivalNumber);
            }

            Console.WriteLine("==>Query: " + query);

            MySqlCommand command = new MySqlCommand(query, cnx);

            DataTable dataTable = new DataTable();
            this.CheckConnection();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);


            dataAdapter.Fill(dataTable);
            cnx.Close();
            return dataTable;
        }

        public DataTable GetPieceList(Piece filter)
        {
            String query = String.Format("SELECT * FROM piece_rechange WHERE (refference LIKE \"{0}%\" OR refference IS NULL)", filter.Refference);
           
            Console.WriteLine("==>Query: " + query);

            MySqlCommand command = new MySqlCommand(query, cnx);

            DataTable dataTable = new DataTable();
            this.CheckConnection();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(command);

            dataAdapter.Fill(dataTable);
            cnx.Close();
            return dataTable;
        }


        public ObservableCollection<Machine> GetFactureMachineList(Facture filter)
        {
            String query = String.Format("SELECT * FROM proformaarc AS p LEFT JOIN machine ON id=id_machine WHERE p.id_facture=\"{0}\" AND p.type_facture=\"{1}\" ORDER BY ref", filter.IdFacture , filter.Type );
            
            MySqlCommand command = new MySqlCommand(query, cnx);

            Console.WriteLine("==>Query: " + query);
           
            this.CheckConnection();
            
            MySqlDataReader reader = command.ExecuteReader();

           

            ObservableCollection<Machine> machineCollection = new ObservableCollection<Machine>();

            while (reader.Read())
            {
                Machine machine = new Machine();

                machine.Id = reader["id"].ToString();
                machine.Categ = reader["categ"].ToString();
                machine.Designation = reader["designation"].ToString();
                machine.State = reader["etat"].ToString();
                machine.NameF = reader["namef"].ToString();
                machine.ArrivalNumber = reader["num_arrivage"].ToString();
                machine.Refference = reader["ref"].ToString();
                machine.Serial = reader["serial"].ToString();
                machine.Pdv = Double.Parse(reader["pdv"].ToString());

                Console.WriteLine("machine Added Id = " + machine.Id);
                Console.WriteLine("              ref = " + machine.Refference);

                machineCollection.Add(machine);
            }



            cnx.Close();
            cnx.Close();
            return machineCollection;
        }

        public ObservableCollection<Tache> GetFactureTacheList(Facture filter)
        {
            String query = String.Format("SELECT * FROM tache_facture AS p LEFT JOIN tache ON id=id_tache WHERE p.id_facture=\"{0}\" AND p.type_facture=\"{1}\" ORDER BY designation", filter.IdFacture, filter.Type);

            MySqlCommand command = new MySqlCommand(query, cnx);

            Console.WriteLine("==>Query: " + query);

            this.CheckConnection();

            MySqlDataReader reader = command.ExecuteReader();

            ObservableCollection<Tache> machineCollection = new ObservableCollection<Tache>();

            while (reader.Read())
            {
                Tache tache = new Tache();

                tache.Id = Int32.Parse(reader["id_tache"].ToString());
                tache.Designation = reader["designation"].ToString();
                tache.Pdv = Double.Parse(reader["pdv"].ToString());

                Console.WriteLine("machine Added Id = " + tache.Id);

                machineCollection.Add(tache);
            }

            cnx.Close();
            cnx.Close();
            return machineCollection;
        }

        public Boolean EditMachine(Machine machine)
        {
            try
            {
                String query1 = String.Format("UPDATE machine SET ref=\"{1}\" " +
                                                                ", categ=\"{2}\"" +
                                                                 " , designation=\"{3}\" " +
                                                                 ", remarque =\"{4}\" "+
                                                                "WHERE (id=\"{0}\")"
                                                                , machine.Id , machine.Refference, machine.Categ, machine.Designation , machine.Remarque);
                Console.WriteLine("==>Query: " + query1);


                MySqlCommand command = new MySqlCommand(query1, cnx);

                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Machine Modifié : " + machine.Id;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }

        public Boolean RemoveMachine (Facturable machine)
        {
            try
            {
                String query1 = String.Format("DELETE FROM machine WHERE id=\"{0}\"", machine.Id);

                MySqlCommand command = new MySqlCommand(query1, cnx);

                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();
                Console.WriteLine("==>Query: " + query1);

                this.Message = "Machine supprimé : " + machine.Id;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }

        public Boolean RemovePiece(Piece piece)
        {
            try
            {
                String query1 = String.Format("DELETE FROM piece_rechange WHERE id={0}", piece.Id);

                MySqlCommand command = new MySqlCommand(query1, cnx);

                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();
                Console.WriteLine("==>Query: " + query1);

                this.Message = "Piece supprimé : " + piece.Id;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }

        public ObservableCollection<String> ClientList()
        {
            ObservableCollection<String> providerList = new ObservableCollection<String>();

            String query = "SELECT email FROM client";
            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {

                String providerName = reader["email"].ToString();

                providerList.Add(providerName);
            }
            cnx.Close();
            Console.WriteLine("==>Query: " + query);

            return providerList;
        }



        public Boolean AddClient(Client client)
        {

            if(this.CheckRCCA(client.RegNumber , client.CartArt))
            {
                this.Message = "Le numero de carte d\"artisant ou le numero de registre doivent etre uniques !";
                return false;
            }
           
            String query = String.Format("INSERT INTO client  (name , address , email , mobile , article , nif , reg_number ," +
                "activity , carteArt) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\")"
            , client.Name , client.Address , client.Email , client.Mobile , client.Article , client.Nif , client.RegNumber , client.Activity, client.CartArt);

            MySqlCommand command = new MySqlCommand(query, cnx);


            try
            {
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();
                Console.WriteLine("==>Query: " + query);

                this.Message = "Ajouté avec succès!";

                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;

            }

        }



        public Client GetClientByEmail(String email)
        {

            Client client = new Client();

            String query = " SELECT * FROM client Where email =\"" + email + "\" ";
            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                client.Name = reader["name"].ToString();
                client.Activity = reader["activity"].ToString();
                client.Article = reader["article"].ToString();
                client.Nif = reader["nif"].ToString();
                client.RegNumber = reader["reg_number"].ToString();
                client.Mobile = reader["mobile"].ToString();
                client.Address = reader["address"].ToString();
                client.Email = reader["email"].ToString();
                client.CartArt = reader["carteArt"].ToString();
            }

            Console.WriteLine("==>Query: " + query);


            cnx.Close();

            return client;
        }


        //RCCA isn\"t that dope it\"s just reg.Commerce and carteA :(
        public Boolean CheckRCCA(string reg , string carteArt)
        {
            if (reg.Equals("") && carteArt.Equals(""))
                return false;
           

            String query = "SELECT email FROM client Where reg_number =\"" + reg + "\" AND carteArt=\""+ carteArt + "\"";
            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();

            Boolean found = reader.Read();

            cnx.Close();
            Console.WriteLine("==>Query: " + query);

            return found;
        }

        public bool UpdateMachine(Machine machine )
        {
            try
            {
                String query1 = "";
                String query2 = String.Format("INSERT INTO proformaarc (id_facture , type_facture , id_machine )" +
                    " VALUES ( \"{0}\" , \"{1}\" , \"{2}\" )", machine.IdFacture, machine.TypeFacture, machine.Id);

                if (machine.IdFacture == "none")
                {
                    // in case the machine.IdFacture is default 
                     query1 = String.Format("UPDATE machine SET etat=\"{1}\"  " +
                                                                "WHERE (id=\"{0}\")"
                                                                , machine.Id, machine.State);
                }
                else
                {
                     query1 = String.Format("UPDATE machine SET etat=\"{1}\" ,id_facture=\"{2}\" ,type_facture=\"{3}\" " +
                                                                   "WHERE (id=\"{0}\")"
                                                                   , machine.Id, machine.State, machine.IdFacture,machine.TypeFacture);
                    
                }



                MySqlCommand command = new MySqlCommand(query1, cnx);
                MySqlCommand command1 = new MySqlCommand(query2, cnx);

                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command.ExecuteNonQuery();
                command1.ExecuteNonQuery();
                cnx.Close();
                Console.WriteLine("==>Query: " + query1);
                Console.WriteLine("==>Query: " + query2);

                this.Message = "Machine Modifié : " + machine.Id;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }

        public bool FacturerTache(Tache tache)
        {
            try
            {
                String query = String.Format("INSERT INTO tache_facture (id_facture , type_facture , id_tache )" +
                    " VALUES ( \"{0}\" , \"{1}\" , \"{2}\" )", tache.IdFacture, tache.TypeFacture, tache.Id);
                
                MySqlCommand command1 = new MySqlCommand(query, cnx);

                // cnx.Close();
                // cnx.Open();
                this.CheckConnection();
                command1.ExecuteNonQuery();
                cnx.Close();
                Console.WriteLine("==>Query: " + query);

                this.Message = "Machine Modifié : " + tache.Id;
                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;
            }
        }


        public bool Savefacture(Facture facture)
        {
            Console.WriteLine("facture saved id: " + facture.IdFacture);


            String query = String.Format("INSERT INTO facture (id_facture , date_fact , pay_method , payed , state , trial , type ,"+
                " client_email , remise , waranty , timbre ) values (\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\")", facture.IdFacture,
                facture.DateFacture.ToString("yyyy-MM-dd hh:mm") , facture.PayMethod , facture.Payed , facture.State , 0
                , facture.Type  , facture.Client.Email,facture.Remise , facture.Waranty , facture.Timbre);


            Console.WriteLine("==>Query: " + query);

            MySqlCommand command = new MySqlCommand(query, cnx);
 
            try
            {
                this.CheckConnection();
                command.ExecuteNonQuery();
                cnx.Close();

                this.Message = "Ajouté avec succès!";

                return true;
            }
            catch (MySqlException e)
            {
                this.Message = e.Message;
                return false;

            }
        }



        public int getLastIdFacture(string type)
        {

            string lastIdFull = "(SELECT id_facture FROM facture WHERE type=\"" + type + "\" AND YEAR(date_fact) = \"" + DateTime.Now.Year + "\" ORDER BY LENGTH(id_facture) DESC , id_facture DESC LIMIT 1)";

            String query = "SELECT SUBSTR(" + lastIdFull +", 1, (length(" + lastIdFull +") - " +  " length(SUBSTRING_INDEX((" + lastIdFull + "), \"/\", -1))-1)) AS CUT";


            MySqlCommand command = new MySqlCommand(query, cnx);

            this.CheckConnection();
            MySqlDataReader reader = command.ExecuteReader();

            int id = -1;
            Console.WriteLine("==>Query: " + query);

            while (reader.Read())
            {
                if (reader[0] == null)
                    return 0;
                try
                {
                    Int32.TryParse(reader.GetString(0) , out id);
                }catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    return 0;
                }
              
            }
           
            cnx.Close();

            return id;
        }







    }
}
