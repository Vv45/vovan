using Npgsql;
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
using System.Windows.Shapes;

namespace vovan
{
    /// <summary>
    /// Логика взаимодействия для bookEx.xaml
    /// </summary>
    public partial class bookEx : Window
    {
        public bookEx(Books selectedBook)
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = database.extr;
            TextBoxAuthor.Text = Biblioteka.author;
            TextBoxIsdanie.Text=Biblioteka.published;
            TextBoxIsbn.Text = Biblioteka.isbn;
            TextBoxName.Text = Biblioteka.name;
            CbClass.ItemsSource = database.Classe;
            CbStudent.ItemsSource = database.Students;
            Closing += BookEx_Closing;
            selectStudents();
            selectcalsses();    
            
        }

        private void BookEx_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        
        }
        public void selectcalsses() 
        {
            NpgsqlCommand command = database.GetCommand("SELECT name FROM classes");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) 
            {
                while (reader.Read()) 
                {
                    Classes classes = new Classes();
                    classes.Namee = reader.GetString(0);
                    database.Classe.Add(classes);
                }
                reader.Close();

            }
            reader.Close ();    
        }

        public void selectBooks()
        {
            NpgsqlCommand command = database.GetCommand("SELECT \"id\", \"Name\", author, \"Year\", isbn, publishing, image, status, janr, count FROM \"books\" ORDER BY \"id\" ");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Books boooks = new Books();
                    boooks.id = reader.GetInt32(0);
                    boooks.name = reader.GetString(1);
                    boooks.author = reader.GetString(2);
                    boooks.data = reader.GetString(3);
                    boooks.isbn = reader.GetString(4);
                    boooks.published = reader.GetString(5);
                    boooks.imagee = reader.GetString(6);
                    boooks.status = reader.GetString(7);
                    boooks.janr = reader.GetString(8);
                    boooks.count = reader.GetString(9);
                    database.books.Add(boooks);
                }
                reader.Close();
            }
            reader.Close();
        }
        public void slectedClass() 
        {
            NpgsqlCommand cmd = database.GetCommand("SELECT login, class FROM \"ycheniki\" ");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows) 
            {
                while (reader.Read()) 
                {
               students students = new students();
               students.clas = reader.GetString(1);
               students.log = reader.GetString(0);
               database.Students.Add(students);
                
                }
            }
        }
        public void selectStudents()
        {
            NpgsqlCommand cmd = database.GetCommand("SELECT login, name, class FROM \"ycheniki\", \"classes\" WHERE \"ycheniki\".\"class\"=\"classes\".\"name\" ");
            NpgsqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    students students = new students();
                    students.clas = reader.GetString(1);
                    students.log = reader.GetString(0);
                    database.Students.Add(students);

                }
                reader.Close ();
            }
            reader.Close() ;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
        
        }
        
    }
}
