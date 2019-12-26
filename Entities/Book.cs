using System;
using NodaTime;

namespace Entities
{
    // ReSharper disable once AllowPublicClass
    public class Book
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Isbn { get; set; }

        public string Authors { get; set; }

        public int Year { get; set; }

        public Instant CreatedDate { get; set; }

        public Instant UpdatedDate { get; set; }
    }
}
