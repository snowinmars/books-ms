using System;
using System.Threading;
using System.Threading.Tasks;

namespace Logic.Abstractions
{
    public interface ILogic<T>
    {
        Task<T> GetAsync(Guid id, CancellationToken cancellationToken = default);

        Task<T> AddAsync(T item, CancellationToken cancellationToken = default);

        Task<T> UpdateAsync(T freshItem, CancellationToken cancellationToken = default);

        Task<T> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<T[]> ListAsync(CancellationToken cancellationToken = default);
    }
}
