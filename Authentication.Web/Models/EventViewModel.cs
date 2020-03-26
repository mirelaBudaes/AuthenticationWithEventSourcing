using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Authentication.Web.Models
{
    public class EventViewModel
    {
        public string EventAction { get; set; }
        public UserViewModel UserInfo { get; set; }
        public System.DateTime TimeStamp { get; set; }
    }
}
