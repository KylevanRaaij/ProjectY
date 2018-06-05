using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class User
    {
        public string UserID { get; set; }
        public string SessionKey { get; set; }
        public DateTime DateCheck { get; set; }
    }
}