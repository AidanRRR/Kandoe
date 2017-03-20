using System;
namespace Models.Models.Events
{
    public class CardPickEvent : ISessionEvent
    {
        public Guid SessionId { get; set; }
        public string Type { get; set; } = "CARD_PICK";
        public string UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public int TurnNr { get; set; } = 0;
        public string[] Cards { get; set; }
        public CardPickEvent()
        {
        }
    }
}