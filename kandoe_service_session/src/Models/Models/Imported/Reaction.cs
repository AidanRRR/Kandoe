using System;

namespace Models.Imported
{
    public class Reaction : IReaction
    {
        public string CardId { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime Time { get ; set; }
    }
}