using System;

namespace Authentication.SqlStore.Models
{
    public class User
    {
        public User()
        { }

        public User(Guid userId, string emailAddress, bool emailIsVerified)
        {
            UserId = userId;
            Email = emailAddress;
            EmailIsVerified = emailIsVerified;
        }

        public Guid UserId { get; set; }

        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }
    }
}
