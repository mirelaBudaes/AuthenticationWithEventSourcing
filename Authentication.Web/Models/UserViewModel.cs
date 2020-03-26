using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Authentication.EventStore.Models;

namespace Authentication.Web.Models
{
    public class UserViewModel
    {
        public List<AuthenticationEventViewModel> History { get; set; }
        public StoredUserViewModel StoredUser { get; set; }
    }
}
