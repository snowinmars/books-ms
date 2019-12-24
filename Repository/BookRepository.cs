using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BookService.CommonEntities.Exceptions;
using Entities;
using Microsoft.EntityFrameworkCore;
using Repository.Abstractions;

namespace Repository
{
    internal class BookRepository : IBookRepository {
        private readonly BookContext context;

        private DbSet<Book> Books => context.Set<Book>();

        public BookRepository(BookContext context)
        {
            this.context = context;
        }

        public async Task<Book> GetAsync(Expression<Func<Book, bool>> filter, CancellationToken token = default)
        {
            return await GetOrReturnDefaultAsync(filter, token) ?? throw new NotFoundException();
        }

        public async Task<Book> GetOrReturnDefaultAsync(Expression<Func<Book, bool>> filter, CancellationToken token = default)
        {
            return await Books.Where(filter).FirstOrDefaultAsync( token);
        }

        public async Task<Book> AddAsync(Book item, CancellationToken token = default)
        {
            var freshItem = await Books.AddAsync(item, token);

            return freshItem.Entity;
        }

        public Task<Book> UpdateAsync(Book freshItem, CancellationToken token = default)
        {
            var updatedItem = Books.Update(freshItem);

            return Task.FromResult(updatedItem.Entity);
        }

        public Task<Book> DeleteAsync(Book item, CancellationToken token = default)
        {
            var removedItem = Books.Remove(item);

            return Task.FromResult(removedItem.Entity);
        }

        public async Task<Book[]> ListAsync(CancellationToken token = default)
        {
            return await Books.ToArrayAsync( token);
        }

        public async Task SaveAsync(CancellationToken token = default)
        {
            await context.SaveChangesAsync(token);
        }
    }
}
