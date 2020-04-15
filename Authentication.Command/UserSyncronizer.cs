using System;
using System.Linq;
using Authentication.EventStore.Events;
using Authentication.EventStore.Models;
using Authentication.Infrastructure;
using Authentication.SqlStore.Models;

namespace Authentication.Command
{
    public class UserSyncronizer
    {
        private readonly UserSqlRepository _userSqlRepository;
        private readonly IAuthenticationEventRepository _eventRepository;

        public UserSyncronizer(UserSqlRepository userSqlRepository, IAuthenticationEventRepository eventRepository)
        {
            _userSqlRepository = userSqlRepository;
            _eventRepository = eventRepository;
        }
        public void Save(User user)
        {
            if (user == null)
                return;

            _userSqlRepository.Save(user);
        }

        public User Replay(Guid userId)
        {
            var allEvents = _eventRepository.All(userId)
                .OrderBy(x => x.TimeStamp);
            if (!allEvents.Any())
                return null;

            var user = new User();
            foreach (var e in allEvents)
            {
                var whatHappened = (EventAction)Enum.Parse(typeof(EventAction), e.EventActionName);
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
                        user.EmailIsVerified = emailVerifiedEvent.UserInfo.EmailIsVerified;
                        user.LastUpdatedDate = e.TimeStamp;
                        break;
                    case EventAction.EmailChangeRequested:
                        //We shouldn't change the user yet, since email address is not confirmed


                        break;

                }
            }
            return user;
        }
    }
}
