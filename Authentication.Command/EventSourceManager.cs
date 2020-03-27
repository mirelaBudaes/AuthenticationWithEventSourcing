using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;
using Authentication.SqlStore.Models;

namespace Authentication.Command
{
    public interface IEventSourceManager
    {
        void Log(EventAction whatHappened, string emailAddresss);
        void Log(EventAction whatHappened, string emailAddresss, Guid userId);
    }

    internal class EventSourceManager : IEventSourceManager
    {
        private readonly IAuthenticationEventRepository _eventRepository;
        private readonly UserSyncronizer _userSyncronizer;

        public EventSourceManager(IAuthenticationEventRepository eventRepository,
            UserSyncronizer userSyncronizer)
        {
            _eventRepository = eventRepository;
            _userSyncronizer = userSyncronizer;
        }

        public void Log(EventAction whatHappened, string emailAddresss)
        {
            Log(whatHappened, emailAddresss, Guid.NewGuid());

        }
        
        public void Log(EventAction whatHappened, string emailAddresss, Guid userId)
        {
            var authenticationEvent = EventBuilder.New(whatHappened, emailAddresss, userId);
            _eventRepository.Store(authenticationEvent);

            //// Sync up with read model
            var user = Replay(userId);

            if (whatHappened != EventAction.EmailUniqueValidationFailed
            && whatHappened != EventAction.EmailChangeRequested)
            {
                _userSyncronizer.Save(user);
            }
        }

        //todo: move to Syncronizer
        public User Replay(Guid userId)
        {
            var allEvents = _eventRepository.All(userId);
            if (!allEvents.Any())
                return null;

            var user = new User();
            foreach (var e in allEvents)
            {
                var whatHappened = (EventAction)Enum.Parse(typeof(EventAction), e.EventAction);
                switch (whatHappened)
                {
                    case EventAction.UserRegistered:
                        var userRegisteredEvent = e as UserRegisteredEvent;
                        user = new User(e.UserId, userRegisteredEvent.UserInfo.Email, userRegisteredEvent.UserInfo.EmailIsVerified);
                        user.CreatedDate = e.TimeStamp;
                        user.LastUpdatedDate = e.TimeStamp;
                        break;
                    case EventAction.EmailUniqueValidationFailed:
                        break;
                    case EventAction.EmailVerified:
                        var emailVerifiedEvent = e as EmailVerifiedEvent;
                        user = new User(e.UserId, emailVerifiedEvent.UserInfo.Email, emailVerifiedEvent.UserInfo.EmailIsVerified);
                        user.LastUpdatedDate = e.TimeStamp;
                        break;
                    case EventAction.EmailChangeRequested:
                        //We shouldn't change the user yet, since email address is not confirmed

                        //var emailchangeRequestedEvent = e as EmailChangeRequestedEvent;
                        //user = new User(e.UserId, emailchangeRequestedEvent.NewEmailAddress, false);
                        //user.LastUpdatedDate = e.TimeStamp;
                        break;

                }
            }
            return user;
        }
    }
}
