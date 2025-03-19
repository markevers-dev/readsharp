using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services
{
    public sealed class Service
    {
        private static readonly Service _instance = new();
        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new System.Uri("http://localhost:5272/api/")
        };

        public static Service Instance => _instance;

        private Service() { }

        public async Task<List<Book>> GetBooks()
        {
            return await _httpClient.GetFromJsonAsync<List<Book>>("book") ?? [];
        }

        public async Task<Stream?> GetCoverImage(int bookId)
        {
            var response = await _httpClient.GetAsync($"book/{bookId}/cover");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
            return null;
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _httpClient.GetFromJsonAsync<List<Author>>("author") ?? [];
        }

        public async Task<List<Genre>> GetGenres()
        {
            return await _httpClient.GetFromJsonAsync<List<Genre>>("genre") ?? [];
        }

        public async Task<List<Publisher>> GetPublishers()
        {
            return await _httpClient.GetFromJsonAsync<List<Publisher>>("publisher") ?? [];
        }

        public async Task DeleteBook(int bookId)
        {
            await _httpClient.DeleteAsync($"book/{bookId}");
        }
    }
}


