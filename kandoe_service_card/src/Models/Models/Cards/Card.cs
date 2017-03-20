using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models.Cards
{
    public class Card : ICard
    {
        [BsonId]
        public string CardId { get; set; }
        public string ImageUrl { get; set; }
        public string Text { get; set; }
        public string ThemeId { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
        public ICollection<Reaction> Reactions { get ; set ; }

        public Card()
        {
            CardId = Guid.NewGuid().ToString();
            CreatedOn = DateTime.UtcNow;
            IsEnabled = true;
            Reactions = new List<Reaction>();
        }
    }
}
