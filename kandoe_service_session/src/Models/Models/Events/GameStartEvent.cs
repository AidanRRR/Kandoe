using System;

namespace Models.Models.Events
{
    public class GameStartEvent : ISessionEvent
    {
        public Guid SessionId { get; set; }
        public string Type { get; set; } = "GAME_START";
        public string UserId { get; set; } = null;
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; } = 0;
        public string[] Cards { get; set; }
    }
}