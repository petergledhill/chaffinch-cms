using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Chaffinch.CQRS.Events
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly Dictionary<Guid, List<IEvent>> _inMemoryDb = new Dictionary<Guid, List<IEvent>>();

        public InMemoryEventStore(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public Task<IEnumerable<IEvent>> Get(Guid aggregateId, int fromVersion, CancellationToken cancellationToken = default(CancellationToken))
        {
            _inMemoryDb.TryGetValue(aggregateId, out var events);
            IEnumerable<IEvent> returnedEvents = events?.Where(x => x.Version > fromVersion) ?? new List<IEvent>();
            return Task.FromResult(returnedEvents);
        }

        public async Task Save(IEnumerable<IEvent> events, CancellationToken cancellationToken = default(CancellationToken))
        {
            foreach(var @event in events)
            {
                _inMemoryDb.TryGetValue(@event.Id, out var list);
                if (list == null)
                {
                    list = new List<IEvent>();
                    _inMemoryDb.Add(@event.Id, list);
                }
                list.Add(@event);
                await _eventPublisher.Publish(@event, cancellationToken);
            }
            
        }
    }
}
