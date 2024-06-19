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
    /// Логика взаимодействия для auto.xaml
    /// </summary>
    public partial class auto : Page
    {
        public auto()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string Log = logtb.Text.Trim();
            string Pass = passwordBox1.Password.Trim();
            if (Log.Length <= 0 || Pass.Length <= 0)
            {
                MessageBox.Show("заполните поля");
            }
            else
            {
                NpgsqlCommand command = database.GetCommand("SELECT  \"Accounts\".\"login\", \"password\", \"role\".\"name\"  ," +
                    " \"teacheres\".\"classes\",  \"Accounts\".\"name\", \"surname\"," +
                "\"patronymic\", \"telephone\", \"adress\" " +
                "FROM  \"admin\", \"Accounts\", \"role\", \"teacheres\"" +
                "WHERE \"Accounts\".\"role\" = \"role\".\"id\" AND  \"Accounts\".password = @password AND  \"Accounts\".\"login\"= @login");
                //" AND \"Accounts\".\"login\" = \"teacheres\".\"login\" OR \"Accounts\".\"login\" = \"admin\".\"login\" ");
                command.Parameters.AddWithValue("@password", NpgsqlDbType.Varchar, Pass);
                command.Parameters.AddWithValue("@login", NpgsqlDbType.Varchar, Log);
                NpgsqlDataReader result = command.ExecuteReader();
                if (result.HasRows)
                {
                    result.Read();
                    database.nameuser = result.GetString(4);
                    database.classes = result.GetString(3);
                    database.patronymicuser = result.GetString(6);
                    string role = result.GetString(2);
                    switch (role)
                    {
                        case "Администрация":
                            result.Close();
                            frame.Navigate(new CenterAdmin());
                            break;
                        case "Ученик":
                            result.Close();
                            frame.Navigate(new Ychenik());
                            break;
                        case "Учитель":
                            result.Close();
                            frame.Navigate(new teacherpage());
                            break;
                        case "Библиотекарь":
                            result.Close();
                            frame.Navigate(new Biblioteka());
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("неверный логин или пароль");
                }
                result.Close();
            }
        }

        private void frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
