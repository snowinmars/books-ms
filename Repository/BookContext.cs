using EmptyService.CommonEntities;
using EmptyService.Configuration.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Repository
{
    internal class BookContext : DbContext
    {
        private readonly IDatabaseConfig config;

        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

    }
}
