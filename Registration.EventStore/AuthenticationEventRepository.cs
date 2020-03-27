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
                    authenticationEvent.Id = loggedEvent.Id;
                    authenticationEvent.EventAction = loggedEvent.Action;
                    authenticationEvent.TimeStamp = loggedEvent.TimeStamp;
                    authenticationEvent.UserId = loggedEvent.AggregateId;
                    break;

                case EventAction.EmailUniqueValidationFailed:
                    authenticationEvent = JsonConvert.DeserializeObject<EmailUniqueValidationFailedEvent>(loggedEvent.Data);
                    authenticationEvent.Id = loggedEvent.Id;
                    authenticationEvent.EventAction = loggedEvent.Action;
                    authenticationEvent.TimeStamp = loggedEvent.TimeStamp;
                    authenticationEvent.UserId = loggedEvent.AggregateId;
                    break;

                case EventAction.EmailChangeRequested:
                    authenticationEvent = JsonConvert.DeserializeObject<EmailChangeRequestedEvent>(loggedEvent.Data);
                    //authenticationEvent.Id = loggedEvent.Id;
                    //authenticationEvent.EventAction = loggedEvent.Action;
                    //authenticationEvent.TimeStamp = loggedEvent.TimeStamp;
                    //authenticationEvent.UserId = loggedEvent.AggregateId;
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
