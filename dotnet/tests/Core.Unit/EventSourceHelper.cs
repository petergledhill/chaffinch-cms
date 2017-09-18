using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRSlite.Commands;
using CQRSlite.Domain;
using CQRSlite.Events;
using CQRSlite.Snapshotting;

namespace Chaffinch.Core.Unit
{
  
    public class EventSourceHelper
    {
        public ISession Session { get; }
        public IList<IEvent> EventDescriptors { get; }
        public IList<IEvent> PublishedEvents { get; }

        public EventSourceHelper(List<IEvent> existingEvents)
        {
            var eventpublisher = new SpecEventPublisher();
            var eventstorage = new SpecEventStorage(eventpublisher, existingEvents);

            var snapshotstorage = new SpecSnapShotStorage(null);

            var snapshotStrategy = new DefaultSnapshotStrategy();
            var repository = new SnapshotRepository(snapshotstorage, snapshotStrategy, new Repository(eventstorage), eventstorage);

            Session = new Session(repository);
            PublishedEvents = eventpublisher.PublishedEvents;
            EventDescriptors = eventstorage.Events;
        }
    }

    internal class SpecSnapShotStorage : ISnapshotStore
    {
        public SpecSnapShotStorage(Snapshot snapshot)
        {
            Snapshot = snapshot;
        }

        public Snapshot Snapshot { get; set; }

        public Task<Snapshot> Get(Guid id, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(Snapshot);
        }

        public Task Save(Snapshot snapshot, CancellationToken cancellationToken = default(CancellationToken))
        {
            Snapshot = snapshot;
            return Task.CompletedTask;
        }
    }

    internal class SpecEventPublisher : IEventPublisher
    {
        public SpecEventPublisher()
        {
            PublishedEvents = new List<IEvent>();
        }

        public Task Publish<T>(T @event, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IEvent
        {
            PublishedEvents.Add(@event);
            return Task.CompletedTask;
        }

        public IList<IEvent> PublishedEvents { get; set; }
    }

    internal class SpecEventStorage : IEventStore
    {
        private readonly IEventPublisher _publisher;

        public SpecEventStorage(IEventPublisher publisher, List<IEvent> events)
        {
            _publisher = publisher;
            Events = events;
        }

        public List<IEvent> Events { get; set; }

        public Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            Events.AddRange(events);
            return Task.WhenAll(events.Select(evt => _publisher.Publish(evt, cancellationToken)));
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            var events = Events.Where(x => x.Id == aggregateId && x.Version > fromVersion);
            return Task.FromResult(events);
        }
    }
}
