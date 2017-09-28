using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB;
using MongoDB.Driver;

namespace Chaffinch.CQRS.Events
{
    public class MongoDBEventStore : IEventStore
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IMongoDatabase _mongoDatabase;

        public MongoDBEventStore(IEventPublisher eventPublisher, IMongoDatabase mongoDatabase)
        {
            _mongoDatabase = mongoDatabase;
            _eventPublisher = eventPublisher;
        }

        public async Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var collection = _mongoDatabase.GetCollection<IEvent>("events", null);
            
            await collection.FindAsync(e => e.Id == aggregateId, null, cancellationToken);

            return new List<IEvent>();
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            var collection = _mongoDatabase.GetCollection<IEvent>("events");
           
            foreach (var @event in events)
            {
                await collection.InsertOneAsync(@event, null, cancellationToken);
                await _eventPublisher.Publish(@event, cancellationToken);
            }
        }
    }
}
