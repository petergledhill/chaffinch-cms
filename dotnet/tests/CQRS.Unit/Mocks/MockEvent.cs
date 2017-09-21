using CQRSlite.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chaffinch.CQRS.Unit.Mocks
{
    public class MockEvent : IEvent
    {
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
