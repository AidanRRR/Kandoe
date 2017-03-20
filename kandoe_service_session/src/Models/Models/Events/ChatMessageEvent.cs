using System;

namespace Models.Models.Events 
{
    public class ChatMessageEvent: ISessionEvent
    {
        public string Message { get; set; }
        public ChatMessageEvent() 
        {
           this.Type = EventTypes.GetChatEventType();
        }
        public Guid SessionId { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; }
    }
}