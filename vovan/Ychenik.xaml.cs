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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace vovan
{
    /// <summary>
    /// Логика взаимодействия для Ychenik.xaml
    /// </summary>
    public partial class Ychenik : Page
    {
        public Ychenik()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = database.books;
            List1.ItemsSource = database.books;
            Selectbook();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        public void Selectbook() 
        {
            try 
            {
            NpgsqlCommand command = database.GetCommand("SELECT id, \"Name\", \"author\", \"publishing\", \"image\", \"status\", \"janr\", \"Year\", \"count\" FROM \"books\" WHERE \"books\".\"status\" = 'Не выдана' ORDER BY id");
            NpgsqlDataReader reader = command.ExecuteReader();
            if(reader.HasRows) 
            {
                while(reader.Read()) 
                {
                    Books book = new Books();
                    book.id = reader.GetInt32(0);
                    book.name = reader.GetString(1);
                    book.author = reader.GetString(2);
                    book.published = reader.GetString(3);
                    book.imagee = reader.GetString(4);
                    book.status = reader.GetString(5);
                    book.janr = reader.GetString(6);
                    book.data = reader.GetString(7);
                    book.count = reader.GetString(8);
                    database.books.Add(book);
                }
                reader.Close();
            }
            reader.Close() ;

            }catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
