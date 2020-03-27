using Authentication.SqlStore.Models;
using Authentication.Web.Models;
using System.Collections.Generic;
using System.Linq;
using Authentication.EventStore.Data;

namespace Authentication.Web.Mappers
{
    public class UsersViewModelMapper
    {
        public HomeViewModel Map(IList<LoggedEvent> events, IList<User> users)
        {
            return new HomeViewModel()
            {
                AuthenticationEvents = events.Select(Map).ToList(),
                Users = users.Select(Map).ToList()
            };
        }

        public AuthenticationEventViewModel Map(LoggedEvent authEvent)
        {
            return new AuthenticationEventViewModel()
            {
                EventAction = authEvent.Action,
                TimeStamp = authEvent.TimeStamp,
                UserInfo = authEvent.Data
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
