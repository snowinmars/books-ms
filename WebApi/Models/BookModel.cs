using System;

namespace BookService.WebApi.Models
{
    internal class BookModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public string Authors { get; set; }

        public int Year { get; set; }
    }
}
