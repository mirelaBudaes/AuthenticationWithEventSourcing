using Authentication.EventStore.Models;
using Authentication.SqlStore.Models;
using Authentication.Web.Models;
using System.Collections.Generic;
using System.Linq;

namespace Authentication.Web.Mappers
{
    public class UsersViewModelMapper
    {
        public UsersViewModel Map(IList<AuthenticationEvent> events, IList<User> users)
        {
            return new UsersViewModel()
            {
                AuthenticationEvents = events.Select(Map).ToList(),
                Users = users.Select(Map).ToList()
            };
        }

        EventViewModel Map(AuthenticationEvent authEvent)
        {
            return new EventViewModel()
            {
                EventAction = authEvent.EventAction,
                TimeStamp = authEvent.TimeStamp,
                UserInfo = Map(authEvent.UserInfo)
            };
        }

        UserViewModel Map(UserInfo user)
        {
            return new UserViewModel()
            {
                Email = user.Email,
                EmailIsVerified = user.EmailIsVerified
            };
        }

        UserViewModel Map(User user)
        {
            return new UserViewModel()
            {
                Email = user.Email,
                EmailIsVerified = user.EmailIsVerified
            };
        }
    }
}
