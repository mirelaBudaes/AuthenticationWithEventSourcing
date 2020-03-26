using Authentication.EventStore.Models;
using Authentication.SqlStore.Models;
using Authentication.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Web.Mappers
{
    public class UsersViewModelMapper
    {
        public HomeViewModel Map(IList<AuthenticationEvent> events, IList<User> users)
        {
            return new HomeViewModel()
            {
                AuthenticationEvents = events.Select(Map).ToList(),
                Users = users.Select(Map).ToList()
            };
        }

        public AuthenticationEventViewModel Map(AuthenticationEvent authEvent)
        {
            return new AuthenticationEventViewModel()
            {
                EventAction = authEvent.EventAction,
                TimeStamp = authEvent.TimeStamp,
                UserInfo = Map(authEvent.UserInfo)
            };
        }

        public StoredUserViewModel Map(EventUserInfo eventUser)
        {
            if (eventUser == null) return null;

            return new StoredUserViewModel()
            {
                Email = eventUser.Email,
                EmailIsVerified = eventUser.EmailIsVerified,
                UserId = eventUser.UserId
            };
        }

        public StoredUserViewModel Map(User user)
        {
            return new StoredUserViewModel()
            {
                UserId = user.UserId,
                Email = user.Email,
                EmailIsVerified = user.EmailIsVerified,
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate
            };
        }
    }
}
