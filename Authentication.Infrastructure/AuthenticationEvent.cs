using System;

namespace Authentication.Infrastructure
{
    public class AuthenticationEvent : Message
    {
        //public Guid Id { get; set; }
        //public string EventActionName { get; set; }
        public Guid UserId { get; set; } //AggregateId
        public System.DateTime TimeStamp { get; set; }
    }
}
