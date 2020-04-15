using System;
using System.Collections.Generic;
using System.Text;
using Authentication.EventStore.Models;
using Authentication.Infrastructure;

namespace Authentication.EventStore.Events
{
    public class EmailChangeRequestedEvent : AuthenticationEvent
    {
        public EmailChangeRequestedEvent()
        {
            this.EventActionName = Authentication.EventStore.Models.EventAction.EmailChangeRequested.ToString();
        }

        public string NewEmailAddress { get; set; }
        //public string OldEmailAddress { get; set; }
    }
}
