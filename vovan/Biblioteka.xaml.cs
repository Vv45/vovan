
using Microsoft.Win32;
using Microsoft.Xaml.Behaviors.Core;
using Npgsql;
using Npgsql.Internal;
using NpgsqlTypes;
using OfficeOpenXml;
using OfficeOpenXml.ExternalReferences;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Database;
using OfficeOpenXml.ThreadedComments;
using Prism.Regions.Behaviors;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Eventing.Reader;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Versioning;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;
using LicenseContext = System.ComponentModel.LicenseContext;

namespace vovan
{
    /// <summary>
    /// Логика взаимодействия для Biblioteka.xaml
    /// </summary>
    public partial class Biblioteka : Page
    {
        public static string name;
        public static string author;
        public static string published;
        public static string isbn;

        public MainView MainView;
        private Books selectedBook;
        public Biblioteka()
        {
            InitializeComponent();
            Binding binding = new Binding();
            binding.Source = database.books;
            List.ItemsSource = database.books;
            MainView = new MainView();
            DataContext = MainView;
            tbtext.Text = "Здравствуйте " + database.nameuser;
            tbtext1.Text = database.patronymicuser;
            selectBooks();
        }
        public void selectBooks()
        {
            try
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
            catch (Exception ex) { MessageBox.Show("Проверьте поля" + ex); }
        }
        private void List_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var select = List.SelectedItem as Books;
            if (List.SelectedItem != null)
            {
                EditBooks editBooks = new EditBooks();
                editBooks.TbHeader.Text = select.name.ToString();
                editBooks.Tbimage.Text =select.imagee.ToString();
                editBooks.CbAuthor.Text = select.author.ToString();
                editBooks.CbJanr.Text = select.janr.ToString();
                editBooks.dataPick.Text = select.data.ToString();
                editBooks.tbISBN.Text = select.isbn.ToString();
                editBooks.tbizdat.Text = select.published.ToString();
                editBooks.CBstatus.Text = select.status.ToString();
                editBooks.TBcount.Text = select.count.ToString();
                editBooks.Show();
                
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddBook addBook = new AddBook();
            addBook.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var select = List.SelectedItem as Books;
            int id = select.id;
            try
            {
                NpgsqlCommand command = database.GetCommand("DELETE FROM \"books\" WHERE id=@id ");
                command.Parameters.AddWithValue("@id", NpgsqlDbType.Integer, id);
                var result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("Удалено");
                    database.books.Remove(select);
                }
                else
                {
                    MessageBox.Show("Не удалено");
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

        }
        //private void ExportToExcel(DataTable data)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog();
        //    saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx";

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        FileInfo file = new FileInfo(saveFileDialog.FileName);

        //        using (ExcelPackage package = new ExcelPackage(file))
        //        {
        //            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");
        //            worksheet.Cells["A1"].Value = "Ид";
        //            worksheet.Cells["B1"].Value = "Название";
        //            int row = 2;
        //            foreach(var item ida) 
        //            {
        //                worksheet.Cells[string.Format("A{0}", row)].Value = item.database.books.id;
        //                worksheet.Cells[string.Format("B{0}", row)].Value=item.database.books.name;
        //            }
        //            using (FileStream fs = new FileStream("exported_data.xlsx", FileMode.Create))
        //            {
        //                package.SaveAs(fs);
        //            }
        //        }

        //        MessageBox.Show("Data exported successfully!");
        //    }
        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
            ExcelPackage excel = new ExcelPackage();
            ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("Sheet1");SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel файлы (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog.Title = "Сохранить Excel файл";

            if (saveFileDialog.ShowDialog() != null)
            {

                // Добавляем заголовки
                worksheet.Cells[1, 1].Value = "Номер";
                worksheet.Cells[1, 2].Value = "Название";
                worksheet.Cells[1, 3].Value = "Автор";
                worksheet.Cells[1, 4].Value = "Издание";
                worksheet.Cells[1, 5].Value = "Год выпуска";
                worksheet.Cells[1, 6].Value = "ISBN";
                worksheet.Cells[1, 7].Value = "Жанр";
                worksheet.Cells[1, 8].Value = "Количество";

                // Добавляем данные (предположим, что данные находятся в ListBox)
                int row = 2;
                foreach (var item in database.books)
                {
                    worksheet.Cells[row, 1].Value = item.id;
                    worksheet.Cells[row, 2].Value = item.name;
                    worksheet.Cells[row, 3].Value = item.author;
                    worksheet.Cells[row, 4].Value = item.published;
                    worksheet.Cells[row, 5].Value = item.data;
                    worksheet.Cells[row, 6].Value = item.isbn;
                    worksheet.Cells[row, 7].Value=item.janr;
                    worksheet.Cells[row, 8].Value = item.count;
                    row++;
                }

                // Сохраняем Excel файл
                var filePath = ".xlsx";
                FileInfo excelFile = new FileInfo(filePath);
                excel.SaveAs(saveFileDialog.FileName);
                excel.Dispose();

                MessageBox.Show("Экспорт данных в Excel завершен.");
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new auto());
            database.books.Clear();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
                bookEx book = new bookEx(selectedBook);
                book.Show();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (selectedBook != null) 
            {
            selectedBook = List.SelectedItem as Books;
            name = selectedBook.name;
            author = selectedBook.author;
            published = selectedBook.published;
            isbn = selectedBook.isbn;
            }
            
        }
    }
}

