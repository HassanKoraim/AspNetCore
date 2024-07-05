using ControllerApi.Models;

namespace ControllerApi.Services
{
    public interface IBookServices
    {
        Task<List<Book>> getAll();
    }
}