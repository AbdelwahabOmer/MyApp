using Microsoft.EntityFrameworkCore;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly myDbContext _dbContext;

        public BookRepository(myDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddBook(BookModel bookModel)
        {
            var data = new Books()
            {
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description,
            };

            await _dbContext.Books.AddAsync(data);
            await _dbContext.SaveChangesAsync();
            return data.Id;
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            return await _dbContext.Books.Select(bookModel => new BookModel
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description
            }).ToListAsync();
        }

        public async Task<BookModel> GetBookById(int Id)
        {
            return await _dbContext.Books.Where(a => a.Id == Id).Select(bookModel => new BookModel
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description
            }).FirstOrDefaultAsync();
        }

        public async Task<List<BookModel>> GetTopBooksAsync(int count)
        {
            return await _dbContext.Books.Select(bookModel => new BookModel
            {
                Id = bookModel.Id,
                Title = bookModel.Title,
                Author = bookModel.Author,
                Description = bookModel.Description
            }).Take(count).ToListAsync();
        }

        public List<BookModel> SearchBook(string title, string author)
        {
            return null;
        }
    }
}
