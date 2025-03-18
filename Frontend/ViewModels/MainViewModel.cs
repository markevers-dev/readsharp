using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Frontend.Models;
using Frontend.Services;
using Frontend.Helpers;
using System.Windows;

namespace Frontend.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Service _service = Service.Instance;

        private List<BookViewModel> AllBooks { get; } = [];
        public ObservableCollection<BookViewModel> Books { get; } = [];
        public ObservableCollection<Genre> Genres { get; } = [];
        public ObservableCollection<Publisher> Publishers { get; } = [];

        private Genre? _selectedGenre;
        public Genre? SelectedGenre
        {
            get => _selectedGenre;
            set { _selectedGenre = value; OnPropertyChanged(); }
        }

        private Publisher? _selectedPublisher;
        public Publisher? SelectedPublisher
        {
            get => _selectedPublisher;
            set { _selectedPublisher = value; OnPropertyChanged(); }
        }

        private string _searchText = string.Empty;
        public string SearchText
        {
            get => _searchText;
            set { _searchText = value; OnPropertyChanged(); }
        }

        private double _booksLoadedProgress;
        public double BooksLoadedProgress
        {
            get => _booksLoadedProgress;
            set { _booksLoadedProgress = value; OnPropertyChanged(); }
        }

        private int _coversLoaded;
        public int CoversLoaded
        {
            get => _coversLoaded;
            set { _coversLoaded = value; OnPropertyChanged(); }
        }

        private bool _isLoadingCovers;
        public bool IsLoadingCovers
        {
            get => _isLoadingCovers;
            set { _isLoadingCovers = value; OnPropertyChanged(); }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get => _errorMessage;
            set { _errorMessage = value; OnPropertyChanged(); }
        }

        private bool _isErrorVisible;
        public bool IsErrorVisible
        {
            get => _isErrorVisible;
            set { _isErrorVisible = value; OnPropertyChanged(); }
        }

        public ICommand ApplyFiltersCommand { get; }

        public ICommand ResetFiltersCommand { get; }

        public ICommand ReloadBooksCommand { get; }

        public MainViewModel()
        {
            ApplyFiltersCommand = new RelayCommand(async () => await ApplyFilters());
            ResetFiltersCommand = new RelayCommand(async () => await ResetFilter());
            ReloadBooksCommand = new RelayCommand(async () => await ReloadBooks());

            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                await LoadBooksAsync();
                await LoadGenresAsync();
                await LoadPublishersAsync();
            }
            catch (Exception)
            {
                ShowError($"An error occurred while fetching the books, please try again later!");
            }
        }

        private async void ShowError(string message)
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                ErrorMessage = $"❌ {message}";
                IsErrorVisible = true;
            });

            await Task.Delay(5000);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                IsErrorVisible = false;
            });
        }

        private async Task LoadBooksAsync()
        {
            var books = await _service.GetBooks();

            IsLoadingCovers = true;
            CoversLoaded = 0;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                AllBooks.Clear();
                Books.Clear();
            });

            var totalBooks = books.Count;

            foreach (var book in books)
            {
                var bookViewModel = new BookViewModel(book, () =>
                {
                    CoversLoaded++;
                    if (CoversLoaded >= Books.Count)
                    {
                        IsLoadingCovers = false;
                    }
                });

                await Application.Current.Dispatcher.InvokeAsync(() =>
                {
                    AllBooks.Add(bookViewModel);
                    Books.Add(bookViewModel);
                });
            }
        }

        private async Task LoadGenresAsync()
        {
            var genres = await _service.GetGenres();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Genres.Clear();
                foreach (var genre in genres)
                {
                    Genres.Add(genre);
                }
            });
        }

        private async Task LoadPublishersAsync()
        {
            var publishers = await _service.GetPublishers();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Publishers.Clear();
                foreach (var publisher in publishers)
                {
                    Publishers.Add(publisher);
                }
            });
        }

        private async Task ApplyFilters()
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Books.Clear();
                foreach (var book in AllBooks)
                {
                    bool matchesGenre = SelectedGenre == null || book.Genres.Any(g => g.Name.Contains(SelectedGenre.Name, StringComparison.OrdinalIgnoreCase));
                    bool matchesPublisher = SelectedPublisher == null || book.Publisher.Name.Equals(SelectedPublisher.Name, StringComparison.OrdinalIgnoreCase);
                    bool matchesSearch = string.IsNullOrWhiteSpace(SearchText) ||
                                         book.Title.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                         book.AuthorsText.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

                    if (matchesGenre && matchesPublisher && matchesSearch)
                    {
                        Books.Add(book);
                    }
                }
            });
        }

        private async Task ResetFilter()
        {
            Books.Clear();
            SelectedGenre = null;
            SelectedPublisher = null;

            await ApplyFilters();
        }

        private async Task ReloadBooks()
        {
            await LoadDataAsync();
            await ApplyFilters();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
