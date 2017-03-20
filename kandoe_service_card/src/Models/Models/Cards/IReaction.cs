using System;

namespace Models.Models.Cards
{
    public interface IReaction
    {
        string CardId { get; set;}
        string Username { get; set; }
        string Message { get; set; }
        DateTime Time { get; set; }
    }
}