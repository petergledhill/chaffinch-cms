using Chaffinch.CQRS.Events;
using CQRSlite.Events;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Chaffinch.CQRS.Tests.Mocks;

namespace Chaffinch.CQRS.Unit.Events
{
    public class InMemoryEventStoreTests
    {
        private readonly Mock<IEventPublisher> _publisher;
        private readonly InMemoryEventStore _inMemoryDb;

        public InMemoryEventStoreTests()
        {
            _publisher = new Mock<IEventPublisher>();
            _inMemoryDb = new InMemoryEventStore(_publisher.Object);
        }

        [Fact]
        public async Task Save_should_publish_the_provided_events()
        {           
            var event1 = new MockEvent { Id = Guid.NewGuid() };
            var event2 = new MockEvent { Id = Guid.NewGuid() };
            var cancellationToken = default(CancellationToken);

            await _inMemoryDb.Save(new List<IEvent> { event1, event2 }, cancellationToken);

            _publisher.Verify(m => m.Publish<IEvent>(event1, cancellationToken));
            _publisher.Verify(m => m.Publish<IEvent>(event2, cancellationToken));
        }

        [Fact]
        public async Task Saved_events_can_retreived()
        {    
            var event1 = new MockEvent { Id = Guid.NewGuid(), Version = 1 };
            var cancellationToken = default(CancellationToken);

            await _inMemoryDb.Save(new List<IEvent> { event1 });

            var retreivedEvents = await _inMemoryDb.Get(event1.Id, 0, cancellationToken);

            Assert.Single(retreivedEvents);
            Assert.Same(event1, retreivedEvents.ToList().ElementAt(0));
        }

        [Fact]
        public async Task Can_retreive_events_after_a_version_number()
        {
            var aggregateId = Guid.NewGuid();
            var event1 = new MockEvent { Id = aggregateId, Version = 1 };
            var event2 = new MockEvent { Id = aggregateId, Version = 2 };
            var event3 = new MockEvent { Id = aggregateId, Version = 3 };
            var cancellationToken = default(CancellationToken);

            await _inMemoryDb.Save(new List<IEvent> { event1, event2, event3 });

            var retreivedEvents = await _inMemoryDb.Get(event1.Id, 1, cancellationToken);

            Assert.Equal(2, retreivedEvents.Count());
            Assert.Same(event2, retreivedEvents.ToList().ElementAt(0));
            Assert.Same(event3, retreivedEvents.ToList().ElementAt(1));
        }

    }
}
