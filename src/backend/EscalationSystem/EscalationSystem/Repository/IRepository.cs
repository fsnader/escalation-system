using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.Repository
{
    public interface IRepository<T>
    {
        public Task<T> CreateOrUpdateAsync(T entity, CancellationToken cancellationToken);

        public Task<T> GetByIdAsync(Guid Id, CancellationToken cancellationToken);

        public Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken);

        public Task DeleteByIdAsync(Guid Id, CancellationToken cancellationToken);
    }
}

// Endpoint pra escalonar/criar incidente

// Integração InfoBip (pensar nas chaves/tentativas)
// Extra: Webhook Servicenow