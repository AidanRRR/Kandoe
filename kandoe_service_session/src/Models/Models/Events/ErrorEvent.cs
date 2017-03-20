using System;

namespace Models.Models.Events
{
    public class ErrorEvent: ISessionEvent
    {
        public Guid SessionId { get; set; }
        public string Type { get; set; } = "ERROR";
        public string UserId { get; set; } = null;
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; } = -1;
        public string Message { get; set; }
        public string ErrorType { get; set; }
    }
}