using MinimalApi.Models;

namespace MinimalApi.Services
{
    public class BookServices : IBookServices
    {
        public async Task<List<Book>> getAll()
        {
            await Task.Delay(500);
            var books = new List<Book>()
            {

                new Book { Id = 1, Title = "1984", Author = "George Orwell", Genre = "Dystopian", Price = 9.99M },
                new Book { Id = 2, Title = "To Kill a Mockingbird", Author = "Harper Lee", Genre = "Classic", Price = 7.99M },
                new Book { Id = 3, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Genre = "Classic", Price = 10.99M },
                new Book { Id = 4, Title = "The Catcher in the Rye", Author = "J.D. Salinger", Genre = "Classic", Price = 8.99M },
                new Book { Id = 5, Title = "Pride and Prejudice", Author = "Jane Austen", Genre = "Romance", Price = 6.99M }
            };
            return books;
        }
    }
}
