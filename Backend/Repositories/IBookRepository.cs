using Backend.Data;
using Backend.Models;

namespace Backend.Repositories
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<List<BookDTO>> GetBooksWithRelationsAsync();
        Task<BookDTO?> GetBookWithRelationsAsync(int id);
        Task<(byte[]? ImageData, string? ContentType)> GetCoverImageAsync(int bookId);
        Task<string> UploadCoverImageAsync(IFormFile file);
        Task<string> UpdateCoverImageAsync(string existingFileName, IFormFile file);
    }
}
