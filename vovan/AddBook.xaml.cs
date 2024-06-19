﻿using Microsoft.Win32;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
    /// Логика взаимодействия для AddBook.xaml
    /// </summary>
    public partial class AddBook : Window
    {
        public AddBook()
        {
            InitializeComponent();
            dataPick.MaxLength = 4;
            Binding binding = new Binding();
            binding.Source = database.books;
            CbJanr.ItemsSource = database.janrs;
            CBstatus.ItemsSource = database.statuses;
            CbAuthor.ItemsSource = database.authors;
            SelectJanr();
            SelectStatus();
            SelectAuthor();
            Closing += AddBooks_Closing;
        }
        private void AddBooks_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            database.janrs.Clear();
            database.statuses.Clear();
            database.authors.Clear();


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
            reader.Close() ;
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
         string image = Tbimage.Text.Trim();
         string header = TbHeader.Text.Trim();
         string author = CbAuthor.Text.Trim();
         string isdat = tbizdat.Text.Trim();
         string data = dataPick.Text.Trim();
         string isbn = tbISBN.Text.Trim();
         string janr = CbJanr.Text.Trim();
         string status = CBstatus.Text.Trim();
            string counts = TBcount.Text.Trim();
            try {
                 Books books = new Books();
                NpgsqlCommand getMaxIdcmd = database.GetCommand("SELECT MAX(id) FROM books");
                {
                    int MaxId = Convert.ToInt32(getMaxIdcmd.ExecuteScalar());
                    int newId = MaxId +1;
                NpgsqlCommand cmd1 = database.GetCommand("" + "INSERT INTO books (\"id\", \"Name\", \"author\", \"Year\", isbn, \"publishing\", \"status\", \"image\", \"janr\", \"count\") VALUES (@id, @Name, @author, @Year, @isbn, @publishing, @status, @image, @janr, @count)");
                cmd1.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, newId);
                cmd1.Parameters.AddWithValue("@Name", NpgsqlDbType.Varchar,  header);
                cmd1.Parameters.AddWithValue("@author", NpgsqlDbType.Varchar, author);
                cmd1.Parameters.AddWithValue("@Year", NpgsqlDbType.Varchar, data);
                cmd1.Parameters.AddWithValue("@isbn", NpgsqlDbType.Varchar, isbn);
                cmd1.Parameters.AddWithValue("@publishing", NpgsqlDbType.Varchar, isdat);
                cmd1.Parameters.AddWithValue("@status", NpgsqlDbType.Varchar, status);
                cmd1.Parameters.AddWithValue("@image", NpgsqlDbType.Varchar, image);
                cmd1.Parameters.AddWithValue("@janr", NpgsqlDbType.Varchar, janr);
                cmd1.Parameters.AddWithValue("@count", NpgsqlDbType.Varchar, counts);
                var result = cmd1.ExecuteNonQuery();
                if (result < newId)
                {
                   
                    MessageBox.Show("Добавлено");
                    database.books.Add(books);
                    database.books.Clear();
                    selectBooks();
                       
                }
                else                
                {
                    MessageBox.Show(" Не Добавлено");
                }
                }
                 }
            catch(Exception ex) 
            {
                MessageBox.Show("Ошибка" + ex.Message);
            }

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