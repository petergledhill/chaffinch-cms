using Chaffinch.CQRS.Events;
using Chaffinch.CQRS.Unit.Mocks;
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
        private readonly Mock<IMongoCollection<IEvent>> _mongoCollection;

        private readonly MongoDBEventStore _mongoDBEventStore;

        public MongoDBEventStoreTests()
        {
            _publisher = new Mock<IEventPublisher>();
            _mongoDb = new Mock<IMongoDatabase>();
            _mongoCollection = new Mock<IMongoCollection<IEvent>>();

            _mongoDb.Setup(db => db.GetCollection<IEvent>("events", null))
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

        /*
         * I am unable to mock the mongo find extension methods so going to have  
         * 
        [Fact]
        public async Task Get_querys_mongo_for_the_events_by_aggregrate_id()
        {
            var aggregateId = Guid.NewGuid();        
            var cancellationToken = default(CancellationToken);
            Expression<Func<IEvent, bool>> findById = e => e.Id == aggregateId;
            var eventCursor = new Mock<IAsyncCursor<IEvent>>();
            
            _mongoCollection.Setup(c => c.FindAsync(findById, null, cancellationToken))
                .ReturnsAsync(eventCursor.Object);
            
            await _mongoDBEventStore.Get(aggregateId, 0, cancellationToken);
            
            _mongoCollection.Verify(c => c.FindAsync<IEvent>(findById, null, cancellationToken));            
        }*/
    }
}
