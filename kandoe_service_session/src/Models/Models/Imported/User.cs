using System;

namespace Models.Models.Imported
{
    // Gekopieerd van user service
    public class User
    {
        //public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime UpdatedOn { get; set; } = DateTime.Now;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public bool IsEnabled { get; set; } = true;

        public User()
        {
            //UserId = Guid.NewGuid().ToString();
            IsEnabled = true;
        }
    }
}