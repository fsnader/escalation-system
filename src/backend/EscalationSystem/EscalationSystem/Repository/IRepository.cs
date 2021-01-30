using System;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.Repository
{
    public interface IRepository<T>
    {
        public Task<T> CreateOrUpdateAsync(T entity, CancellationToken cancellationToken);

        public Task<T> GetByIdAsync(Guid Id, CancellationToken cancellationToken);

        public Task DeleteByIdAsync(Guid Id);
    }
}
