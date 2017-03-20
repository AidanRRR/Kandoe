using System;
using Models.Models.Session;
using Models.Models.Imported;
using System.Collections.Generic;

namespace Models.Models.Events
{
    public class SessionStartEvent : ISessionEvent
    {
        public Guid SessionId { get; set; }
        public string Type { get; set; } = "SESSION_START";
        public string UserId { get; set; } = null;
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; } = 0;
        public ISession Session { get; set; }
        public List<User> Players { get; set; }
    }
}