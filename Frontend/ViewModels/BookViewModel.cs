using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frontend.Models;
using Frontend.Services;
using Frontend.Helpers;

namespace Frontend.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Book> _books;
        public ObservableCollection<Book> Books
        {
            get => _books;
            set
            {
                _books = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadBooksCommand { get; }

        public BookViewModel()
        {
            LoadBooksCommand = new RelayCommand(LoadBooks);
            Books = [];
        }

        private async void LoadBooks()
        {
            List<Book> books = await BookService.GetBooks();
            Books = new ObservableCollection<Book>(books);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
