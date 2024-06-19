using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vovan
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string host = "194.113.34.92";
        public static string port = "5432";           //короче я пытался сделать настраиваемое подключение и обкакался и поэтому такой кусок кода
        public static string user = "postgres";
        public static string password = "123";
        public static string dbname = "postgres";
        public MainWindow()
        {
            InitializeComponent();
            database.Connect(host, port, user, password, dbname);
            frame.Navigate(new auto());
        }

    }
}
