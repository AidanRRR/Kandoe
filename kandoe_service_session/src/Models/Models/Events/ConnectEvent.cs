using System;
using Models.Models.Session;
namespace Models.Models.Events 
{
    public class ConnectEvent: ISessionEvent
    {
        public SessionState State { get; set; }
        public ConnectEvent()
        {
            this.Type = EventTypes.GetConnectEventType();
        }
        public Guid SessionId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; }
    }
}