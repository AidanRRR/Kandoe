using System;

namespace Models.Models.Events
{
    public class SessionEndEvent : ISessionEvent
    {
        public Guid SessionId { get; set; }
        public string Type { get; set; } = "SESSION_END";
        public string UserId { get; set; } = null;
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; } = -1;
    }
}