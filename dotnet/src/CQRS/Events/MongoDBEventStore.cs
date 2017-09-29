using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

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
            var collection = _mongoDatabase.GetCollection<BaseEvent>("events", null);

            var events = await collection.AsQueryable().Where(u => u.Id == aggregateId && u.Version > fromVersion).ToListAsync();

            return events;
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            var collection = _mongoDatabase.GetCollection<BaseEvent>("events");

            foreach (var @event in events)
            {
                if (!(@event is BaseEvent)) throw new InvalidCastException("Events must extend BaseEvent");

                await collection.InsertOneAsync((BaseEvent)@event, null, cancellationToken);
                await _eventPublisher.Publish(@event, cancellationToken);
            }
        }
    }
}
