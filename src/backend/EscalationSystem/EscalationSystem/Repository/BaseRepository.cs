using AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.Repository
{
    public class BaseRepository<T> : IRepository<T>
    {

        public BaseRepository()
        {

        }

        public async Task<T> CreateOrUpdateAsync(T entity, CancellationToken cancellationToken)
        {
            return entity;
        }

        public Task DeleteByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            return new Fixture().Create<T>();
        }

        public Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
