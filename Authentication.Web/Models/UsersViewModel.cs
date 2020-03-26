using System;
using System.Collections.Generic;

namespace Authentication.Web.Models
{
    public class UsersViewModel
    {
        public List<EventViewModel> AuthenticationEvents { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}
