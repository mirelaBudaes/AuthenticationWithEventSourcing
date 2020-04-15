using System;
using System.Collections.Generic;
using System.Text;
using Authentication.EventStore.Models;
using Authentication.Infrastructure;

namespace Authentication.EventStore.Events
{
    public class EmailUniqueValidationFailedEvent :AuthenticationEvent
    {
        public string Email { get; set; }
    }
}
