using Chaffinch.CQRS.Events;
using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chaffinch.CQRS.Tests.Mocks
{
    public class MockEvent : BaseEvent
    {
        public string SomeInfo { get; set; }
    }

    public class AnotherMockEvent : BaseEvent
    {
        public string DifferentInfo { get; set; }
    }

    public class EventWithoutBaseEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
