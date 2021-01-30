using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EscalationSystem.Repository
{
    public class MongoRepository<T> : IRepository<T>
        where T : IdentifiedEntity
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly string _collectionName;

        public MongoRepository()
        {
            //TODO: Move mongoDbConfigurations to interface
            MongoClient dbClient = new MongoClient("mongodb+srv://teste:teste@escalationmongo.o7v6m.mongodb.net/EscalationMongo?retryWrites=true&w=majority");
            _mongoDatabase = dbClient.GetDatabase("EscalationMongo");
            _collectionName = typeof(T).Name;
        }

        public async Task<T> CreateOrUpdateAsync(T entity, CancellationToken cancellationToken)
        {
            try
            {
                if (entity.Id == default)
                {
                    entity.Id = Guid.NewGuid();

                    await _mongoDatabase
                        .GetCollection<T>(_collectionName)
                        .InsertOneAsync(entity, null, cancellationToken);
                }
                else
                {
                    await _mongoDatabase
                        .GetCollection<T>(_collectionName)
                        .UpdateOneAsync(e => e.Id == entity.Id, JsonConvert.SerializeObject(entity), null, cancellationToken);
                }

                return entity;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task DeleteByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            var result = await _mongoDatabase
                .GetCollection<T>(_collectionName)
                .DeleteOneAsync(e => e.Id == Id, cancellationToken);
        }

        public async Task<T> GetByIdAsync(Guid Id, CancellationToken cancellationToken)
        {
            var result = await _mongoDatabase
                .GetCollection<T>(_collectionName)
                .FindAsync(f => f.Id == Id);

            return await result.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> ListAllAsync(CancellationToken cancellationToken)
        {
            var result = await _mongoDatabase
                .GetCollection<T>(_collectionName)
                .FindAsync(f => true);

            return await result.ToListAsync();
        }
    }
}
