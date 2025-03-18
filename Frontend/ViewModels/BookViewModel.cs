using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media.Imaging;
using Frontend.Models;
using Frontend.Services;

namespace Frontend.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private readonly Service _service = Service.Instance;
        private BitmapImage? _coverImage;
        private readonly Action _onCoverLoaded;

        public int Id { get; }
        public string Title { get; }
        public string? Description { get; }
        public decimal Price { get; }
        public int PageCount { get; }
        public int Rating { get; }
        public int? PublicationYear { get; }
        public ICollection<Author> Authors { get; }
        public string AuthorsText { get; }
        public ICollection<Genre> Genres { get; }
        public string GenresText { get; }
        public Publisher Publisher { get; }
        public string PublisherText { get; }

        public BitmapImage? CoverImage
        {
            get => _coverImage;
            private set
            {
                _coverImage = value;
                OnPropertyChanged();
            }
        }

        public BookViewModel(Book book, Action OnCoverLoaded)
        {
            Id = book.Id;
            Title = book.Title;
            Description = book.Description;
            Price = book.Price;
            PageCount = book.PageCount;
            Rating = book.Rating;
            PublicationYear = book.PublicationYear;
            Authors = book.Authors;
            AuthorsText = book.Authors != null && book.Authors.Count != 0
            ? string.Join(", ", book.Authors.Select(a => a.Name))
            : "Unknown Author(s)";
            Genres = book.Genres;
            GenresText = book.Genres != null && book.Genres.Any()
            ? string.Join(", ", book.Genres.Select(g => g.Name))
            : "No Genres";
            Publisher = book.Publisher;
            PublisherText = book.Publisher?.Name ?? "No Publisher";

            _onCoverLoaded = OnCoverLoaded;
            Task.Run(LoadCoverImageAsync);
        }

        private async void LoadCoverImageAsync()
        {
            var imageStream = await _service.GetCoverImage(Id);
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (imageStream != null)
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = imageStream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    bitmap.Freeze();

                    CoverImage = bitmap;
                }

                _onCoverLoaded?.Invoke();
            });
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
