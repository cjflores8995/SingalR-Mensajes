using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppWeb.Models
{
    public class UserHub
    {
        public string UserId { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }
}