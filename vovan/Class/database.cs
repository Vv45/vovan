using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vovan
{
    public class database
    {
        public static string classes;
        public static string nameuser;
        public static string patronymicuser;
        public static NpgsqlConnection Connection;

        public static ObservableCollection<students> Students { get; set; } = new ObservableCollection<students>();
        public static ObservableCollection<Classes> Classe { get; set; } = new ObservableCollection<Classes> ();
        public static ObservableCollection<Books> books { get; set; } = new ObservableCollection<Books> ();
        public static ObservableCollection<Status> statuses { get; set; } = new ObservableCollection<Status>();
        public static ObservableCollection<Janr> janrs { get; set; } = new ObservableCollection<Janr> ();
        public static ObservableCollection<Author> authors { get; set; } = new ObservableCollection<Author> ();
        public static ObservableCollection<Extradition> extr {  get; set; } = new ObservableCollection<Extradition> ();
        public static void Connect(string host, string port, string user, string password, string database)
        {
            if (Connection == null)
            {
                string db = string.Format("Server={0}; Port={1}; User Id={2}; Password={3}; Database={4}", host, port, user, password, database);
                Connection = new NpgsqlConnection(db);
                Connection.Open();
            }
        }
        public static NpgsqlCommand GetCommand(string sql)
        {
            NpgsqlCommand command = new NpgsqlCommand();
            command.Connection = Connection;
            command.CommandText = sql;
            return command;
        }
    }
}
