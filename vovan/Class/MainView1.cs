using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace vovan
{
    public class MainView1 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Status> Items
        {
            get { return database.statuses; }
            set
            {
                if (database.statuses != value)
                {
                    database.statuses = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        private ICollectionView view;
        private  string cbselectedval;

        public string Combo
        {
            get { return cbselectedval; }
            set
            {
                if (cbselectedval != value)
                {
                    cbselectedval = value;
                    OnPropertyChanged(nameof(cbselectedval));
                    ApplyFilterName();
                }
            }
        }
        public void ApplyFilterName()
        {
            view = CollectionViewSource.GetDefaultView(Items);
            if (!string.IsNullOrEmpty(Combo))
            {
                // Используйте предикат для фильтрации элементов по заданному критерию поиска 
                view.Filter = item =>
                {
                    Status currentItem = item as Status;

                    return currentItem.statuse.Contains(Combo);
                };
            }
            else
            {
                // Сбросьте фильтр, если критерий поиска пуст 
                view.Filter = null;
            }
        }
       


    }
}
