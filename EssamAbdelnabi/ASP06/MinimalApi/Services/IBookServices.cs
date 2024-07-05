using MinimalApi.Models;

namespace MinimalApi.Services
{
    public interface IBookServices
    {
        Task<List<Book>> getAll();
    }
}