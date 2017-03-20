using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models.Events 
{
    public interface ISessionEvent
    {
        [BsonId]
        Guid SessionId { get; set; }
        string Type { get; set; }
        string UserId { get; set; }
        DateTime Timestamp { get; set; }
        int TurnNr { get; set; }
    }
}