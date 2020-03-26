using System;
using System.Collections.Generic;

namespace Authentication.Web.Models
{
    public class HomeViewModel
    {
        public List<AuthenticationEventViewModel> AuthenticationEvents { get; set; }
        public List<StoredUserViewModel> Users { get; set; }
    }
}
