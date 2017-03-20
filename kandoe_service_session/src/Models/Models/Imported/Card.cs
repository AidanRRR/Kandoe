using System;
using System.Collections.Generic;
using Models.Imported;

namespace Models.Models.Imported
{
    // Gekopieerd van cardservice
    public class Card : ICard
    {
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
            IsEnabled = true;
             Reactions = new List<Reaction>();
        }
    }
}