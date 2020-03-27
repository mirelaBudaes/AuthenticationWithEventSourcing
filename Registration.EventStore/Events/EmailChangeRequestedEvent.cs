using System;
using System.Collections.Generic;
using System.Text;
using Authentication.EventStore.Models;

namespace Authentication.EventStore.Events
{
    public class EmailChangeRequestedEvent : AuthenticationEvent
    {
        public EmailChangeRequestedEvent()
        {
            this.EventAction = Authentication.EventStore.Models.EventAction.EmailChangeRequested.ToString();
        }

        public string NewEmailAddress { get; set; }
        //public string OldEmailAddress { get; set; }
    }
}
