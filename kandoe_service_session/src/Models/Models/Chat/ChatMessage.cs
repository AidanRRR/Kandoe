using System;

namespace Models.Models.Chat
{
    public class ChatMessage
    {
        public DateTime Timestamp { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
    }
}