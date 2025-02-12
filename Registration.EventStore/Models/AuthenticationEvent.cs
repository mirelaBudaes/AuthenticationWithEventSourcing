﻿using System;

namespace Authentication.EventStore.Models
{
    public class AuthenticationEvent
    {
        public Guid Id { get; set; }
        public string EventAction { get; set; }
        public Guid UserId { get; set; } //AggregateId
        public System.DateTime TimeStamp { get; set; }
    }
}
