using System;

namespace Models.Models.Events 
{
    public class TurnStartEvent: ISessionEvent
    {
        public TurnStartEvent()
        {
            this.Type = EventTypes.GetTurnStartEventType();
        }
        public Guid SessionId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.Now;
        public int TurnNr { get; set; }
    }
}