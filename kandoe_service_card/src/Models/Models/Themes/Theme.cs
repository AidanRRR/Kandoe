﻿using System;
using System.Collections.Generic;
using Models.Models.Cards;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models.Themes
{
    public class Theme : ITheme
    {
        [BsonId]
        public string ThemeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<ICard> Cards { get; set; }
        public ICollection<string> Organizers {get; set;}
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;
        public bool IsPublic { get; set; }
        public ICollection<string> Tags { get; set; }
        public string Username { get; set; }

        public Theme()
        {
            ThemeId = Guid.NewGuid().ToString();
            UpdatedOn = DateTime.UtcNow;
            CreatedOn = DateTime.UtcNow;
            IsEnabled = true;
        }
    }
}
