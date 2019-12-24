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
            };
        }

        public static BookModel ToBookModel(this Book book)
        {
            return new BookModel
            {
                Id = book.Id,
                Title = book.Title,
            };
        }
    }
}
