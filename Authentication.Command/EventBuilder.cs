using System;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;

namespace Authentication.Command
{
    class EventBuilder
    {
        public static AuthenticationEvent New(EventAction eventOccurred, string emailAddresss, Guid userId)
        {
            switch (eventOccurred)
            {
                case EventAction.UserRegistered:
                    var authenticationEvent = new UserRegisteredEvent()
                    {
                        EventAction = eventOccurred.ToString(),
                        UserId = userId,
                        UserInfo = new EventUserInfo()
                        {
                            Email = emailAddresss,
                            UserId = userId,
                            EmailIsVerified = false
                        }
                    };
                    return authenticationEvent;

                case EventAction.EmailUniqueValidationFailed:
                    var validationFailedEvent = new EmailUniqueValidationFailedEvent()
                    {
                        EventAction = eventOccurred.ToString(),
                        UserId = userId,
                        Email = emailAddresss
                    };
                    return validationFailedEvent;

                default:
                    return null;
            }
        }
    }
}
