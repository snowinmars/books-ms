using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Repository.Abstractions
{
    public interface IRepository<T>
        where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, CancellationToken token = default);

        Task<T> GetOrReturnDefaultAsync(Expression<Func<T, bool>> filter, CancellationToken token = default);

        Task<T> AddAsync(T item, CancellationToken token = default);

        Task<T> UpdateAsync(T freshItem, CancellationToken token = default);

        Task<T> DeleteAsync(T item, CancellationToken token = default);

        Task<T[]> ListAsync(CancellationToken token = default);

        Task SaveAsync(CancellationToken token = default);
    }
}
