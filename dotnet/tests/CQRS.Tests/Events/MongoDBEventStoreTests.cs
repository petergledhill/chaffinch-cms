using Chaffinch.CQRS.Events;
using Chaffinch.CQRS.Tests.Mocks;
using CQRSlite.Events;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Chaffinch.CQRS.Unit.Events
{
    public class MongoDBEventStoreTests
    {
        private readonly Mock<IEventPublisher> _publisher;
        private readonly Mock<IMongoDatabase> _mongoDb;
        private readonly Mock<IMongoCollection<BaseEvent>> _mongoCollection;

        private readonly MongoDBEventStore _mongoDBEventStore;

        public MongoDBEventStoreTests()
        {
            _publisher = new Mock<IEventPublisher>();
            _mongoDb = new Mock<IMongoDatabase>();
            _mongoCollection = new Mock<IMongoCollection<BaseEvent>>();

            _mongoDb.Setup(db => db.GetCollection<BaseEvent>("events", null))
                .Returns(_mongoCollection.Object);
            
            _mongoDBEventStore = new MongoDBEventStore(_publisher.Object, _mongoDb.Object);
        }

        [Fact]
        public async Task Save_should_publish_the_provided_events()
        {
            var event1 = new MockEvent { Id = Guid.NewGuid() };
            var event2 = new MockEvent { Id = Guid.NewGuid() };
            var cancellationToken = default(CancellationToken);

            await _mongoDBEventStore.Save(new List<IEvent> { event1, event2 }, cancellationToken);

            _publisher.Verify(m => m.Publish<IEvent>(event1, cancellationToken));
            _publisher.Verify(m => m.Publish<IEvent>(event2, cancellationToken));
        }

        [Fact]
        public async Task Save_persists_events()
        {
            var event1 = new MockEvent { Id = Guid.NewGuid() };
            var event2 = new MockEvent { Id = Guid.NewGuid() };
            var cancellationToken = default(CancellationToken);

            await _mongoDBEventStore.Save(new List<IEvent> { event1, event2 }, cancellationToken);

            _mongoCollection.Verify(c => c.InsertOneAsync(event1, null, cancellationToken));
            _mongoCollection.Verify(c => c.InsertOneAsync(event2, null, cancellationToken));
        }
        
        [Fact]
        public async Task Save_requires_event_to_be_base_event()
        {
            var simpleEvent = new EventWithoutBaseEvent { Id = Guid.NewGuid() };

            await Assert.ThrowsAsync<InvalidCastException>(async () => await _mongoDBEventStore.Save(new List<IEvent> { simpleEvent }));                        
        }      
    }
}
