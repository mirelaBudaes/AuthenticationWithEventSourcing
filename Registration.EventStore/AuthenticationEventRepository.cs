using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore.Data;
using Authentication.EventStore.Models;
using Newtonsoft.Json;

namespace Authentication.EventStore
{
    internal class AuthenticationEventRepository : IAuthenticationEventRepository
    {
        private readonly IEventStore _eventDb;
        public AuthenticationEventRepository(IEventStore eventDb)
        {
            _eventDb = eventDb;
        }

        public IList<AuthenticationEvent> All()
        {
            var loggedEvents = _eventDb.GetAll();
            return loggedEvents.Select(Map).ToList();
        }

        private AuthenticationEvent Map(LoggedEvent loggedEvent)
        {
            var authenticationEvent = new AuthenticationEvent();
            var whatHappened = (EventAction)Enum.Parse(typeof(EventAction), loggedEvent.Action);
            switch (whatHappened)
            {
                case EventAction.UserRegistered:
                case EventAction.EmailUniqueValidationFailed:
                    authenticationEvent.Id = loggedEvent.Id;
                    authenticationEvent.EventAction = loggedEvent.Action;
                    authenticationEvent.TimeStamp = loggedEvent.TimeStamp;
                    authenticationEvent.UserId = loggedEvent.AggregateId;
                    authenticationEvent.UserInfo = JsonConvert.DeserializeObject<EventUserInfo>(loggedEvent.Data);
                    break;


            }

            return authenticationEvent;
        }

        public IList<AuthenticationEvent> GetLastEvents(int topX)
        {
            var loggedEvents =_eventDb.GetAll(topX);
            return loggedEvents.Select(Map).ToList();
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
                Data = Newtonsoft.Json.JsonConvert.SerializeObject(authenticationEvent.UserInfo),
                TimeStamp = authenticationEvent.TimeStamp
            };

            _eventDb.Save(loggedEvent);
        }
    }
}
