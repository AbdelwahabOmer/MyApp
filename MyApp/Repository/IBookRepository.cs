using MyApp.Models;

namespace MyApp.Repository
{
    public interface IBookRepository
    {
        Task<int> AddBook(BookModel bookModel);
        Task<List<BookModel>> GetAllBooks();
        Task<BookModel> GetBookById(int Id);
        Task<List<BookModel>> GetTopBooksAsync(int count);
        List<BookModel> SearchBook(string title, string author);
    }
}