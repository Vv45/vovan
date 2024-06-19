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
    /// Логика взаимодействия для teacherpage.xaml
    /// </summary>
    public partial class teacherpage : Page
    {
        public string log;

        public teacherpage()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = database.Students;
            LBST.ItemsSource = database.Students;
            tb.Text = "Здравствуйте! " + database.nameuser;
            tb2.Text = database.patronymicuser;
            tb1.Text = "Список учеников " + database.classes;
            tbphone.MaxLength = 13;
            selectschked();
        }
        

        public void selectschked()
        {
            NpgsqlCommand command = database.GetCommand("SELECT name, surname, patronymic, \"Accounts\".\"login\", telephone, adress  " +
           "FROM \"ycheniki\" , \"Accounts\"   " +
           "WHERE \"ycheniki\".\"login\" = \"Accounts\".\"login\" AND  \"ycheniki\".\"class\" = @group" +
           " ORDER BY \"surname\"  ");
            command.Parameters.AddWithValue("@group", NpgsqlDbType.Varchar, database.classes);
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
                    database.Students.Add(Students);
                }
                aaa.Close();
            }
            aaa.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            database.Students.Clear();
            frame.Navigate(new auto());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var select = LBST.SelectedItem as students;

            if (select == null)
            {

                MessageBox.Show("Выберите ребенка");
            }
            else
            {
                log = select.log;

                string telephone = tbphone.Text.Trim();
                string addres = tbadres.Text.Trim();
                NpgsqlCommand command = database.GetCommand("UPDATE  \"Accounts\" SET telephone = @tel, adress = @ad " +
                    "   WHERE login = @id ");
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Varchar, log);
                command.Parameters.AddWithValue("@tel", NpgsqlDbType.Varchar, telephone);
                command.Parameters.AddWithValue("@ad", NpgsqlDbType.Varchar, addres);

                var result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Успешно изменено");
                    database.Students.Clear();
                    selectschked();


                }
                else
                {
                    MessageBox.Show("Что то пошло не так");
                }
            }
        }

        private void LBST_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LBST.SelectedItem != null)
            {
                var selectitlb = LBST.SelectedItem as students;
              
                tbadres.Text = selectitlb.addres.ToString();
                tbphone.Text = selectitlb.telephone.ToString();
            }
        }

        //private void Button_Click_2(object sender, RoutedEventArgs e)
        //{
        //    var select = LBST.SelectedItem as students;

        //    if (select == null)
        //    {

        //        MessageBox.Show("Выберите ребенка");
        //    }
        //    else
        //    {
        //        log = select.log;
        //        NpgsqlCommand command = database.GetCommand("DELETE FROM \"ycheniki\" WHERE \"ycheniki\".\"login\" = @log");
        //        command.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, log);
        //        command.ExecuteNonQuery();
        //        NpgsqlCommand command1 = database.GetCommand("DELETE FROM \"Accounts\" WHERE \"Accounts\".\"login\" = @log");
        //        command1.Parameters.AddWithValue("@log", NpgsqlDbType.Varchar, log);
        //        var result = command1.ExecuteNonQuery();
        //        if (result == 1)
        //        {
        //            MessageBox.Show("Удалено");
        //            database.Students.Remove(select);
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не получилось");

        //        }
        //    }
        //}

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new addstud());
        }
    }
}
