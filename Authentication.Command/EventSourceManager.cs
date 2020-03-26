using System;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore;
using Authentication.EventStore.Models;
using Authentication.SqlStore.Models;

namespace Authentication.Command
{
    public interface IEventSourceManager
    {
        void Log(EventAction whatHappened, string emailAddresss);

        IEnumerable<AuthenticationEvent> GetEvents(int topX);
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

        public IEnumerable<AuthenticationEvent> GetEvents(int topX)
        {
            return _eventRepository.GetLastEvents(topX);
        }

        public void Log(EventAction whatHappened, string emailAddresss, Guid userId)
        {
            var authenticationEvent = EventBuilder.New(whatHappened, emailAddresss, userId);
            _eventRepository.Store(authenticationEvent);

            //// Sync up with read model
            var user = Replay(userId);
            _userSyncronizer.Save(user);
        }

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
                        user = new User(e.UserId, e.UserInfo.Email, e.UserInfo.EmailIsVerified);
                        break;
                        //case EventType.Start:
                        //    user.Start();
                        //    break;
                        //case EventType.End:
                        //    user.Finish();
                        //    break;
                        //case EventType.NewPeriod:
                        //    user.StartPeriod();
                        //    break;
                        //case EventType.EndPeriod:
                        //    user.EndPeriod();
                        //    break;
                        //case EventType.Goal1:
                        //    user.Goal(TeamId.Home);        // Should add player reference too
                        //    break;
                        //case EventType.Goal2:
                        //    user.Goal(TeamId.Visitors);    // Should add player reference too
                        //    break;
                        //case EventType.Timeout1:
                        //    user.Timeout(TeamId.Home);
                        //    break;
                }
            }
            return user;
        }
    }
}
