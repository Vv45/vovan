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
    /// Логика взаимодействия для CenterAdmin.xaml
    /// </summary>
    public partial class CenterAdmin : Page
    {
        public CenterAdmin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            stat.Visibility = Visibility.Hidden;
            stat1.Visibility = Visibility.Hidden;
            uchitel.Visibility = Visibility.Hidden;
            uchiteladd.Visibility = Visibility.Hidden;  
            frame.Navigate(new addstud());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
            stat.Visibility = Visibility.Hidden;
            stat1.Visibility = Visibility.Hidden;
            uchitel.Visibility = Visibility.Hidden;
            uchiteladd.Visibility = Visibility.Hidden;

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
           
            lb1.Visibility = Visibility.Hidden;  
            bt1.Visibility = Visibility.Hidden;
            stat.Visibility = Visibility.Hidden;
            stat1.Visibility = Visibility.Hidden;
            uchitel.Visibility = Visibility.Hidden;
            uchiteladd.Visibility = Visibility.Hidden;
   
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
            stat.Visibility = Visibility.Hidden;
            stat1.Visibility = Visibility.Hidden;
            uchitel.Visibility = Visibility.Hidden;
            uchiteladd.Visibility = Visibility.Hidden;

        }

        private void bt2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bt_Click(object sender, RoutedEventArgs e)
        {
        }

        private void out_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(new auto());
        }
    }
}
