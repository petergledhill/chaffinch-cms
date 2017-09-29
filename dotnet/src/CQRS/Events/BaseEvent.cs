using CQRSlite.Events;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chaffinch.CQRS.Events
{
    public class BaseEvent : IEvent
    {
        [BsonId]
        public ObjectId ObjectId { get; set; }
        
        [BsonElement("AggregateId")]
        public Guid Id { get; set; }
        public int Version { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}
