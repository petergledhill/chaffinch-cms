using System;
using System.Collections.Generic;
using System.Text;
using CQRSlite.Events;

namespace Chaffinch.Core.Events
{
    public class DocumentTypeCreated : IEvent
    {
        public readonly string Name;

        public DocumentTypeCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
