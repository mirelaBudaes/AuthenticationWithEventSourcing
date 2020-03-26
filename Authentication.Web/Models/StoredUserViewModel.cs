using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Web.Models
{
    public class StoredUserViewModel
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }

        public bool EmailIsVerified { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }
}
