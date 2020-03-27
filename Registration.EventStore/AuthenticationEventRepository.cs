using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore.Data;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;
using Newtonsoft.Json;

namespace Authentication.EventStore
{
    internal class AuthenticationEventRepository : IAuthenticationEventRepository
    {
        private readonly ILoggedEventRepository _eventDb;
        public AuthenticationEventRepository(ILoggedEventRepository eventDb)
        {
            _eventDb = eventDb;
        }

        private AuthenticationEvent Map(LoggedEvent loggedEvent)
        {
            var authenticationEvent = new AuthenticationEvent();
            var whatHappened = (EventAction)Enum.Parse(typeof(EventAction), loggedEvent.Action);
            switch (whatHappened)
            {
                case EventAction.UserRegistered:
                    authenticationEvent = JsonConvert.DeserializeObject<UserRegisteredEvent>(loggedEvent.Data);
                    break;
                case EventAction.EmailUniqueValidationFailed:
                    authenticationEvent = JsonConvert.DeserializeObject<EmailUniqueValidationFailedEvent>(loggedEvent.Data);
                  break;

                case EventAction.EmailVerified:
                    authenticationEvent = JsonConvert.DeserializeObject<EmailVerifiedEvent>(loggedEvent.Data);
                    break;

                case EventAction.EmailChangeRequested:
                    authenticationEvent = JsonConvert.DeserializeObject<EmailChangeRequestedEvent>(loggedEvent.Data);
                    break;

            }

            return authenticationEvent;
        }

        public IList<AuthenticationEvent> All(Guid aggregateId)
        {
            var loggedEvents = _eventDb.GetAll(aggregateId);
            return loggedEvents.Select(Map).ToList();
        }

        public void Store(AuthenticationEvent authenticationEvent)
        {
            authenticationEvent.TimeStamp = DateTime.Now;

            var loggedEvent = new LoggedEvent()
            {
                Action = authenticationEvent.EventAction,
                AggregateId = authenticationEvent.UserId,
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(authenticationEvent),
                TimeStamp = authenticationEvent.TimeStamp
            };

            _eventDb.Save(loggedEvent);
        }
    }
}
