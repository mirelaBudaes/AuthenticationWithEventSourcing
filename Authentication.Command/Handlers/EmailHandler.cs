using System;
using System.Collections.Generic;
using System.Text;
using Authentication.Command.Commands;
using Authentication.EventStore;
using Authentication.EventStore.Events;
using Authentication.Infrastructure;

namespace Authentication.Command.Handlers
{
   public class EmailHandler  : Handler,
       IHandleMessage<UserRegisteredEvent>
   {
        public EmailHandler(IAuthenticationEventRepository eventRepo) : base(eventRepo)
        {
        }

        public void Handle(UserRegisteredEvent authenticationEvent)
        {
            //Send Email
            //EventRepo.Store(authenticationEvent);
        }
   }
}
