using System;
using Authentication.EventStore.Models;

namespace Authentication.Command
{
    class EventBuilder
    {
        public static AuthenticationEvent New(EventAction eventOccurred, string emailAddresss, Guid userId)
        {
            var authenticationEvent = new AuthenticationEvent()
            {
                EventAction = eventOccurred.ToString(),
                UserId = userId,
                UserInfo = new EventUserInfo()
                {
                    Email = emailAddresss,
                    UserId = userId
                }
            };

            switch (eventOccurred)
            {
                case EventAction.UserRegistered:
                    authenticationEvent.UserInfo.EmailIsVerified = false;
                    break;

                case EventAction.EmailVerified:
                    authenticationEvent.UserInfo.EmailIsVerified = true;
                    break;

            }
            return authenticationEvent;
        }
    }
}
