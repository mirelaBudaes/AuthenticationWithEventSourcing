using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Web.Models
{
    public class UserViewModel
    {
        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }
    }
}
