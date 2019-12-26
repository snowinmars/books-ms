using BookService.WebApi.Models;
using Entities;

namespace BookService.WebApi.Mapper
{
    internal static class BookMapper
    {
        public static Book ToBook(this BookModel model)
        {
            return new Book
            {
                Id = model.Id,
                Title = model.Title,
                Authors = model.Authors,
                Isbn = model.Isbn,
                Year = model.Year,
            };
        }

        public static BookModel ToBookModel(this Book book)
        {
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
                Authors = book.Authors,
                Isbn = book.Isbn,
                Year = book.Year,
            };
        }
    }
}
