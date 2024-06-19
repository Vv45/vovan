using Microsoft.Win32;
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
using System.Windows.Shapes;

namespace vovan
{
    /// <summary>
    /// Логика взаимодействия для EditBooks.xaml
    /// </summary>
    public partial class EditBooks : Window

    {
        public MainView MainView;
        public EditBooks()
        {
            InitializeComponent();
            CbAuthor.ItemsSource = database.authors;
            CbJanr.ItemsSource = database.janrs;
            CBstatus.ItemsSource = database.statuses;
            dataPick.MaxLength = 4;
            Binding binding = new Binding();
            binding.Source = database.books;
            SelectStatus();
            SelectJanr();
            SelectAuthor();
            MainView = new MainView();
            DataContext = MainView;
            Closing += EditBooks_Closing;

        }
         
        

        private void EditBooks_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            database.janrs.Clear();
            database.statuses.Clear();
            database.authors.Clear();
        }

         public void SelectAuthor() 
        {
            NpgsqlCommand command = database.GetCommand("SELECT \"author\" FROM \"author\" ORDER BY \"author\" ");
            NpgsqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows) 
            {
                while (reader.Read()) 
                {
                    Author author = new Author();
                    author.authors = reader.GetString(0);
                    database.authors.Add(author);
                }
                reader.Close();
            }
            reader.Close();

        }
        public void SelectJanr() 
        {
            NpgsqlCommand command = database.GetCommand("SELECT  \"janrs\" FROM \"janrs\" Order by \"janrs\" ");
            NpgsqlDataReader aaa = command.ExecuteReader();
            if (aaa.HasRows)
            {
                while (aaa.Read())
                {
                    Janr janr = new Janr();
                    janr.janrss = aaa.GetString(0);
                    database.janrs.Add(janr);
                    
                }
                
                aaa.Close();
            }
            aaa.Close();
        }

        public void SelectStatus() 
        {
            NpgsqlCommand command = database.GetCommand("SELECT \"status\" FROM \"status\" Order by \"status\" ");
            NpgsqlDataReader aaa = command.ExecuteReader();
            if (aaa.HasRows)
            {
                while (aaa.Read())
                {

                    Status status = new Status();
                    status.statuse = aaa.GetString(0);
                    database.statuses.Add(status);
                    
                }
                aaa.Close();
            }
            aaa.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)

        {
            int id = database.books.Count;
            string TbHeaders = TbHeader.Text.Trim();
            string cbauthor = CbAuthor.Text.Trim();
            string tbisnb = tbISBN.Text.Trim();
            string tbisda = tbizdat.Text.Trim();
            string janr = CbJanr.Text.Trim();
            string image = Tbimage.Text.Trim();
            string status = CBstatus.Text.Trim();
            string data = dataPick.Text.Trim();
            string count = TBcount.Text.Trim();
            try 
            {
            NpgsqlCommand command = database.GetCommand("UPDATE \"books\" SET id=@id, \"Name\"=@name, author=@author, isbn=@isnb, publishing=@publish, janr=@janr, image=@image, status=@status, \"Year\"=@Year, count = @count WHERE \"books\".\"id\"=@id ");
            command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
            command.Parameters.AddWithValue("@name", NpgsqlDbType.Varchar, TbHeaders);
            command.Parameters.AddWithValue("@author", NpgsqlDbType.Varchar, cbauthor);
            command.Parameters.AddWithValue("@isnb", NpgsqlDbType.Varchar, tbisnb);
            command.Parameters.AddWithValue("@publish",NpgsqlDbType.Varchar, tbisda);
            command.Parameters.AddWithValue("@janr", NpgsqlDbType.Varchar, janr);
            command.Parameters.AddWithValue("@image", NpgsqlDbType.Varchar, image);
            command.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, status);
            command.Parameters.AddWithValue("@Year", NpgsqlDbType.Varchar, data);
            command.Parameters.AddWithValue("@count", NpgsqlDbType.Varchar, count);
            command.ExecuteNonQuery();
            var result = command.ExecuteNonQuery();
                if (result <= id)
            {
               
                    MessageBox.Show("Успешно изменено");
                    database.books.Clear();
                    selectBooks();
                    
                    
             
            }
            else
            {
                MessageBox.Show("не  изменено");

                }
            }catch (Exception ex) 
            {
            MessageBox.Show("" + ex.Message, "Ошибка");
            }
           
        }
        public void selectBooks()
        {
            NpgsqlCommand command = database.GetCommand("SELECT id, \"Name\", author, \"Year\", isbn, publishing, image, status, janr, count FROM \"books\" ORDER BY id");
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Выберите изображение";
                openFileDialog.Filter = "Изображение | (*.jpeg;*.jpg;*.png)*.jpeg;*.jpg;*.png";
                if (openFileDialog.ShowDialog() == true)
                {
                    Tbimage.Text = openFileDialog.FileName;
                }
            
        }
    }
}
