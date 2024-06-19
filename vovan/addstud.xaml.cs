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
    /// Логика взаимодействия для addstud.xaml
    /// </summary>
    public partial class addstud : Page
    {
        public string log1;
        
        public addstud()
        {
            InitializeComponent(); 
            LBST.ItemsSource = database.Students;
            selectschked();
            tbtel.MaxLength = 12;
            Binding binding = new Binding();
            binding.Source = database.Students;
            cblass.ItemsSource = database.Classe;
            SelectClass();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            database.Students.Clear();
            frame.Navigate(new CenterAdmin());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string name = tbname.Text;
            string surname = tbsurname.Text;
            string patronymic = tbpatronymic.Text;
            string login = tblog.Text;
            string pass = tbpass.Text;
            string addres = tbadres.Text;
            string telephone = tbtel.Text;
            string clas = cblass.Text; 
            NpgsqlCommand command = database.GetCommand("" +
             "INSERT INTO  \"Accounts\" ( \"name\", \"surname\", \"patronymic\", \"login\", \"password\", \"telephone\", \"adress\", \"role\" ) " +
                               "VALUES( @name, @surname, @patronymic, @log, @pass, @tel, @addres, 1 )");
            // command.Parameters.AddWithValue("@id_kid", NpgsqlDbType.Integer, database.id_kiid);
            command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, name);
            command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, surname);
            command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, patronymic);
            command.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, login);
            command.Parameters.AddWithValue("@pass", NpgsqlDbType.Varchar, pass);
            command.Parameters.AddWithValue("@addres", NpgsqlDbType.Varchar, addres);
            command.Parameters.AddWithValue("@tel", NpgsqlDbType.Varchar, telephone);
            var result = command.ExecuteNonQuery();
            NpgsqlCommand command1 = database.GetCommand("Insert into \"ycheniki\" (\"login\" , \"class\") values (@log, @clas ) ");
            command1.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, login);
            command1.Parameters.AddWithValue("@clas", NpgsqlDbType.Varchar, clas);
            command1.ExecuteNonQuery();
            if (result == 1)
            {

                MessageBox.Show("успешно внесен в базу");
                database.Students.Clear();
                selectschked();

            }
            else
            {

                MessageBox.Show("Упс не получилось");
            }
        }
        public void SelectClass() 
        {
            NpgsqlCommand command = database.GetCommand("SELECT \"name\" FROM \"classes\" Order by \"name\" ");
            NpgsqlDataReader aaa = command.ExecuteReader();
            if (aaa.HasRows) 
            {
                while(aaa.Read()) 
                {
                    Classes clas = new Classes();
                    clas.Namee = aaa.GetString(0);
                    database.Classe.Add(clas);
                }
                aaa.Close();
            }
            aaa.Close();
        }

      public void selectschked()
        {
            NpgsqlCommand command = database.GetCommand("SELECT name, surname, patronymic, \"Accounts\".\"login\", telephone, adress,  \"Accounts\".\"password\",   \"ycheniki\".\"class\"  " +
           "FROM \"ycheniki\" , \"Accounts\"   " +
           "WHERE \"ycheniki\".\"login\" = \"Accounts\".\"login\" " +
           " ORDER BY \"surname\"  ");
            NpgsqlDataReader aaa = command.ExecuteReader();
            if (aaa.HasRows)
            {
                while (aaa.Read())
                {
                    students Students = new students();
                    Students.name = aaa.GetString(0);
                    Students.log = aaa.GetString(3);
                    Students.surname = aaa.GetString(1);
                    Students.patronymic = aaa.GetString(2);
                    Students.telephone = aaa.GetString(4);
                    Students.addres = aaa.GetString(5);
                    Students.pass = aaa.GetString(6);
                    Students.clas = aaa.GetString(7);
                    database.Students.Add(Students);
                }
                aaa.Close();
            }
            aaa.Close();
        }
        private void LBST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBST.SelectedItem != null)
            {
                var selectitlb = LBST.SelectedItem as students;
                tbname.Text = selectitlb.name.ToString();
                tbpatronymic.Text = selectitlb.patronymic.ToString();
                tbsurname.Text = selectitlb.surname.ToString();
                tbadres.Text = selectitlb.addres.ToString();
                tbtel.Text = selectitlb.telephone.ToString();
                tblog.Text = selectitlb.log.ToString();
                tbpass.Text = selectitlb.clas.ToString();
                cblass.Text = selectitlb.clas.ToString();
            }
        }
        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            var select = LBST.SelectedItem as students;

            if (select == null)
            {

                MessageBox.Show("Выберите ребенка");
            }
            else
            {
                log1 = select.log;
                string log = tblog.Text.Trim();
                string password = tbpass.Text.Trim();
                string name = tbname.Text.Trim();
                string surname = tbsurname.Text.Trim();
                string patronymic = tbpatronymic.Text.Trim();
                string telephone = tbtel.Text.Trim();
                string addres = tbadres.Text.Trim();
                string clases = cblass.Text.Trim();
                NpgsqlCommand command1 = database.GetCommand("UPDATE \"ycheniki\" SET login= @id,  class = @class  WHERE login = @id1 ");
                command1.Parameters.AddWithValue("@id1", NpgsqlDbType.Varchar, log1);
                command1.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, log);
                command1.Parameters.AddWithValue("@class", NpgsqlDbType.Varchar, clases);
                command1.ExecuteNonQuery();
                NpgsqlCommand command = database.GetCommand("UPDATE  \"Accounts\" SET  login = @id, password = @password , name = @name, surname= @surname," +
                    " patronymic = @patronymic, telephone = @tel, adress = @ad " +
                    "   WHERE login = @id1 ");
                command.Parameters.AddWithValue("@id1", NpgsqlDbType.Varchar, log1);
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, log);
                command.Parameters.AddWithValue("@password", NpgsqlDbType.Varchar, password);
                command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, name);
                command.Parameters.AddWithValue("@surname", NpgsqlDbType.Varchar, surname);
                command.Parameters.AddWithValue("@patronymic", NpgsqlDbType.Varchar, patronymic);
                command.Parameters.AddWithValue("@tel", NpgsqlDbType.Varchar, telephone);
                command.Parameters.AddWithValue("@ad", NpgsqlDbType.Varchar, addres);
                var result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Успешно изменено");
                    database.Students.Clear();
                    selectschked();
                    tbname.Text = "";
                    tbpatronymic.Text = "";
                    tbsurname.Text = "";

                }
                else
                {
                    MessageBox.Show("Что то пошло не так");
                }
            }
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var select = LBST.SelectedItem as students;

            if (select == null)
            {

                MessageBox.Show("Выберите ребенка");
            }
            else
            {
                log1 = select.log;
                string name = tbname.Text.Trim();
                string surname = tbsurname.Text.Trim();
                string patronymic = tbpatronymic.Text.Trim();
                NpgsqlCommand command = database.GetCommand("DELETE FROM \"ycheniki\" WHERE \"ycheniki\".\"login\" = @log");
                command.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, log1);
                command.ExecuteNonQuery();
                NpgsqlCommand command1 = database.GetCommand("DELETE FROM \"Accounts\" WHERE \"Accounts\".\"login\" = @log");
                command1.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, log1);
                var result = command1.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Удалено");
                    database.Students.Remove(select);
                }
                else
                {
                    MessageBox.Show("Не получилось");

                }
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            database.Students.Clear();
            frame.Navigate(new auto());
        }
    }
}
