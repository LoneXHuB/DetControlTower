using MySql.Data.MySqlClient;
using System;


namespace DAO
{
    public class Connection
    {
        private MySqlConnection cnx;

        public Connection(String url ,String user , String pass , string db)
        {

            MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder();
            builder.Server = url;
            builder.UserID = user;
            builder.Password = pass;
            builder.Database = db;

            cnx = new MySqlConnection( builder.ToString() );
            builder = null;

        }

        public MySqlConnection Cnx { get=>cnx; }


    }
}
