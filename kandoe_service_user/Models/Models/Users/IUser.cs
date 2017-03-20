using System;

namespace Models.Models.Users
{
    public interface IUser
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
		bool Notifications { get; set; }
        DateTime UpdatedOn { get; set; }
        DateTime CreatedOn { get; set; }
        bool IsEnabled { get; set; }
    }
}