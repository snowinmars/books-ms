using System;
using System.Threading;
using System.Threading.Tasks;
using Entities;
using Logic.Abstractions;
using Repository.Abstractions;

namespace Logic
{
    internal class BookLogic : IBookLogic
    {
        private readonly IBookRepository repository;

        public BookLogic(IBookRepository repository)
        {
            this.repository = repository;
        }

        public async Task<Book> GetAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await repository.GetAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<Book> AddAsync(Book item, CancellationToken cancellationToken = default)
        {
            var freshItem = await repository.AddAsync(item, cancellationToken);

            await repository.SaveAsync(cancellationToken);

            return freshItem;
        }

        public async Task<Book> UpdateAsync(Book freshItem, CancellationToken cancellationToken = default)
        {
            var updatedItem = await repository.UpdateAsync(freshItem, cancellationToken);

            await repository.SaveAsync(cancellationToken);

            return updatedItem;
        }

        public async Task<Book> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var item = await GetAsync(id, cancellationToken);

            var deletedItem = await repository.DeleteAsync(item, cancellationToken);

            await repository.SaveAsync(cancellationToken);

            return deletedItem;
        }

        public async Task<Book[]> ListAsync(CancellationToken cancellationToken = default)
        {
            return await repository.ListAsync(cancellationToken);
        }
    }
}
