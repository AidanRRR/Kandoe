using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Models.Users
{
    public class User : IUser
    {
        [BsonId]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
		public bool Notifications { get; set; } = true;
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;

        public User()
        {
            Notifications = true;
            IsEnabled = true;
        }
    }
}