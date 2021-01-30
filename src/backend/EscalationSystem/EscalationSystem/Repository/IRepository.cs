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
// Endpoint para listar todos os incidentes
// Endpoint para retornar um incidente por Id

// CRUD Time
// CRUD Employee

// Integração InfoBip (pensar nas chaves/tentativas)
// Extra: Webhook Servicenow