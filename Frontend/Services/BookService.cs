using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Frontend.Models;

namespace Frontend.Services
{
    public static class BookService
    {
        private static readonly HttpClient _httpClient = new()
        {
            BaseAddress = new System.Uri("https://your-backend-url.com/api/")
        };

        public static async Task<List<Book>> GetBooks()
        {
            return await _httpClient.GetFromJsonAsync<List<Book>>("book") ?? new List<Book>();
        }
    }
}


