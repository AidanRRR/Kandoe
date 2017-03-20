using System;

namespace Models.Models.Events 
{
    public class MoveEvent: ISessionEvent
    {
        public string CardId { get; set; }
        public MoveEvent()
        {
            this.Type = EventTypes.GetMoveEventType();
        }
        public Guid SessionId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; }
    }
}