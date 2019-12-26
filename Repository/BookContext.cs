using BookService.CommonEntities;
using BookService.Configuration.Abstractions;
using Entities;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using Npgsql;

namespace Repository
{
    internal class BookContext : DbContext
    {
        private readonly IDatabaseConfig config;

        public BookContext(DbContextOptions<BookContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                        .ToTable("books");

            modelBuilder.Entity<Book>().Property(x => x.Id).IsRequired();
            modelBuilder.Entity<Book>().Property(x => x.Title).IsRequired();

            modelBuilder.Entity<Book>()
                        .Property(x => x.CreatedDate)
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasConversion(x => x.ToDateTimeOffset().ToUnixTimeSeconds(),
                                       x => Instant.FromUnixTimeSeconds(x));

            modelBuilder.Entity<Book>()
                        .Property(x => x.UpdatedDate)
                        .HasColumnType("bigint")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("round(extract(epoch from now()))")
                        .HasConversion(x => x.ToDateTimeOffset().ToUnixTimeSeconds(),
                                       x => Instant.FromUnixTimeSeconds(x));
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}
