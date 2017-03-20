using System;
using System.Collections.Generic;

namespace Models.Models.Imported
{
    public interface ITheme
    {
        string ThemeId { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        ICollection<Card> Cards { get; set; }
        ICollection<string> Organizers { get; set; }
        DateTime UpdatedOn { get; set; }
        DateTime CreatedOn { get; set; }
        bool IsEnabled { get; set; }
        bool Public { get; set;}
        ICollection<string> Tags {get; set;}
        string Username { get; set;}
    }
}