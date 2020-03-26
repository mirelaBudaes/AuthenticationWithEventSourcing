using System;

namespace Authentication.EventStore.Models
{
    public class AuthenticationEvent
    {
        public Guid Id { get; set; }
        public string EventAction { get; set; }
        public Guid UserId { get; set; } //AggregateId
        public UserInfo UserInfo { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }

    public class UserInfo
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }
    }
}
