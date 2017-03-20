using System;
using System.Collections.Generic;
using Models.Models.Cards;

namespace Models.Models.Themes
{
    public interface ITheme
    {
        string ThemeId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<ICard> Cards { get; set; }
        ICollection<string> Organizers { get; set; }
        DateTime UpdatedOn { get; set; }
        DateTime CreatedOn { get; set; }
        bool IsEnabled { get; set; }
        bool IsPublic { get; set;}
        ICollection<string> Tags {get; set;}
        string Username { get; set;}
    }
}