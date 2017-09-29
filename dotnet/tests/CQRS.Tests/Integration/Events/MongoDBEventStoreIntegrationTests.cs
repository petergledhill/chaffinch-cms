using Chaffinch.CQRS.Events;
using Chaffinch.CQRS.Tests.Integration;
using CQRSlite.Events;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Chaffinch.CQRS.Tests.Mocks;

namespace Chaffinch.CQRS.Integration.Events
{
    public class MongoDBEventStoreIntegrationTests : MongoIntegrationTest, IDisposable
    {
        private readonly Mock<IEventPublisher> _eventPublisher;
        private readonly MongoDBEventStore _mongoDBEventStore;

        public MongoDBEventStoreIntegrationTests()
        {
            CreateConnection();

            _eventPublisher = new Mock<IEventPublisher>();
            _mongoDBEventStore = new MongoDBEventStore(_eventPublisher.Object, _database);
        }

        public void Dispose()
        {
            _runner.Dispose();
        }

        [Fact]
        public async Task Save_should_persist_test_event()
        {
            var evt = new MockEvent { Id = Guid.NewGuid(), Version = 1, SomeInfo = "Hello" };

            await _mongoDBEventStore.Save(new List<IEvent> { evt });

            var collection = _database.GetCollection<BaseEvent>("events");
            var savedEvents = collection.Find(p => 1 == 1).ToList();

            Assert.Single(savedEvents);
            Assert.Equal(evt.Id, savedEvents.First().Id);
            Assert.Equal(1, savedEvents.First().Version);
            Assert.Equal("Hello", ((MockEvent)savedEvents.First()).SomeInfo);
        }

        [Fact]
        public async Task Save_should_persist_different_test_event()
        {
            var evt = new MockEvent { Id = Guid.NewGuid(), Version = 1, SomeInfo = "Hello" };
            var evt2 = new AnotherMockEvent { Id = Guid.NewGuid(), Version = 1, DifferentInfo = "World" };

            await _mongoDBEventStore.Save(new List<IEvent> { evt, evt2 });

            var collection = _database.GetCollection<BaseEvent>("events");
            var savedEvents = collection.Find(p => 1 == 1).ToList();

            Assert.Equal(2, savedEvents.Count);

            Assert.Equal("Hello", ((MockEvent)savedEvents.First()).SomeInfo);            
            Assert.Equal("World", ((AnotherMockEvent)savedEvents.ElementAt(1)).DifferentInfo);
        }

        [Fact]
        public async Task Get_should_retreive_existing_events_for_an_aggregate()
        {
            var evt = new BaseEvent { Id = Guid.NewGuid(), Version = 1 };
            var evt2 = new BaseEvent { Id = Guid.NewGuid(), Version = 1 };
            var collection = _database.GetCollection<BaseEvent>("events");

            collection.InsertOne(evt);
            collection.InsertOne(evt2);

            var retreivedEvents = await _mongoDBEventStore.Get(evt.Id, 0);

            Assert.Single(retreivedEvents);
            Assert.Equal(evt.Id, retreivedEvents.First().Id);
            Assert.Equal(1, retreivedEvents.First().Version);
        }

        [Fact]
        public async Task Get_should_retreive_multiple_existing_events_for_an_aggregate()
        {
            var evt = new BaseEvent { Id = Guid.NewGuid(), Version = 1 };
            var evt2 = new BaseEvent { Id = evt.Id, Version = 2 };

            var evt3 = new BaseEvent { Id = Guid.NewGuid(), Version = 1 };

            var collection = _database.GetCollection<BaseEvent>("events");

            collection.InsertOne(evt);
            collection.InsertOne(evt2);
            collection.InsertOne(evt3);

            var retreivedEvents = await _mongoDBEventStore.Get(evt.Id, 0);

            Assert.Equal(2, retreivedEvents.Count());

            Assert.Equal(evt.Id, retreivedEvents.First().Id);
            Assert.Equal(1, retreivedEvents.First().Version);

            Assert.Equal(evt.Id, retreivedEvents.ElementAt(1).Id);
            Assert.Equal(2, retreivedEvents.ElementAt(1).Version);
        }

        [Fact]
        public async Task Get_should_retreive_events_based_on_versionId()
        {
            var evt = new BaseEvent { Id = Guid.NewGuid(), Version = 1 };
            var evt2 = new BaseEvent { Id = evt.Id, Version = 2 };
            var evt3 = new BaseEvent { Id = evt.Id, Version = 3 };

            var collection = _database.GetCollection<BaseEvent>("events");

            collection.InsertOne(evt);
            collection.InsertOne(evt2);
            collection.InsertOne(evt3);

            var retreivedEvents = await _mongoDBEventStore.Get(evt.Id, 2);

            Assert.Single(retreivedEvents);

            Assert.Equal(evt3.Id, retreivedEvents.First().Id);
            Assert.Equal(3, retreivedEvents.First().Version);            
        }

    }
}
