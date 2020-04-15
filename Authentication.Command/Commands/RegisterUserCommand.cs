using System;
using System.Collections.Generic;
using System.Text;
using Authentication.EventStore.Models;

namespace Authentication.Command.Commands
{
    public class RegisterUserCommand : Infrastructure.Command
    {
        public RegisterUserCommand(string emailAddresss)
        {
            EventActionName = EventAction.UserRegistered.ToString();
            EmailAddresss = emailAddresss;
        }

        public string EmailAddresss { get; protected set; }
    }
}
