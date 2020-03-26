using System;

namespace Authentication.EventStore.Models
{
    public class EventUserInfo
    {
        public Guid UserId { get; set; }

        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }
    }
}