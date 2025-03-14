using Backend.Models;

namespace Backend.Repositories
{
    public class BookRepository(DBContext context, IWebHostEnvironment environment) : GenericRepository<Book>(context), IBookRepository
    {
        private readonly DBContext _context = context;
        private readonly IWebHostEnvironment _environment = environment;
        private readonly List<string> allowedExtensions = [".png", ".webp", ".jpg"];
        private readonly string extensionError = "Only PNG, JPG and WebP images are allowed.";
        private readonly string imageFolder = "BookCoverImages";

        public async Task<(byte[]? ImageData, string? ContentType)> GetCoverImageAsync(int bookId)
        {
            var book = await _context.Books.FindAsync(bookId)
                       ?? throw new Exception($"Book with ID {bookId} not found.");

            if (string.IsNullOrEmpty(book.CoverImagePath))
                return (null, null);

            var filePath = Path.Combine(imageFolder, book.CoverImagePath);

            if (!System.IO.File.Exists(filePath))
                return (null, null);

            var imageBytes = await File.ReadAllBytesAsync(filePath);
            var contentType = GetContentType(filePath);

            return (imageBytes, contentType);
        }

        private static string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            return extension switch
            {
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
        }

        public async Task<string> UploadCoverImageAsync(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception(extensionError);

            var newFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(_environment.WebRootPath, imageFolder, newFileName);

            if (System.IO.File.Exists(filePath))
                throw new Exception("A file with this name already exists.");

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{imageFolder}/{newFileName}";
        }

        public async Task<string> UpdateCoverImageAsync(string existingFileName, IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
                throw new Exception(extensionError);

            var existingFilePath = Path.Combine(_environment.WebRootPath, imageFolder, existingFileName);

            if (!System.IO.File.Exists(existingFilePath))
                throw new Exception("The file does not exist and cannot be updated.");

            System.IO.File.Delete(existingFilePath);

            using (var stream = new FileStream(existingFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"{imageFolder}/{existingFileName}";
        }
    }

}
