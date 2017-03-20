using System;

namespace Models.Models.Cards
{
    public class Reaction : IReaction
    {
        public string CardId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Time { get ; set; } = DateTime.Now;
    }
}