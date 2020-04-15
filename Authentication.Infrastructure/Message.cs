using System;

namespace Authentication.Infrastructure
{
    public class Message
    {
        public Guid Id { get; set; }
        public string EventActionName { get; set; }
    }
}