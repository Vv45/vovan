using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace vovan
{
    public class MainView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public static ICollectionView view;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableCollection<Books> Items
        {
            get { return database.books; }
            set
            {
                if (database.books != value)
                {
                    database.books = value;
                    OnPropertyChanged(nameof(Items));
                }
            }
        }
        private string searchKeyword;
       
        
        public string SearchKeyword
        {
            get { return searchKeyword; }
            set
            {
                if (searchKeyword != value)
                {
                    searchKeyword = value;
                    OnPropertyChanged(nameof(SearchKeyword));
                    ApplyFilterName();
                }
            }
        }
        private void ApplyFilterName()
        {
            view = CollectionViewSource.GetDefaultView(Items);
            if (!string.IsNullOrEmpty(SearchKeyword))
            {
                // Используйте предикат для фильтрации элементов по заданному критерию поиска
                view.Filter = item =>
                {
                    Books currentItem = item as Books;
                    return currentItem.janr.Contains(SearchKeyword) || currentItem.name.Contains(SearchKeyword) || currentItem.author.Contains(SearchKeyword) || currentItem.status.Contains(SearchKeyword); 
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