using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Models.Models.Events.Dto
{
    public class SessionEventDto
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Guid SessionId { get; set; }
        public ISessionEvent SessionEvent { get; set; }
    }
}