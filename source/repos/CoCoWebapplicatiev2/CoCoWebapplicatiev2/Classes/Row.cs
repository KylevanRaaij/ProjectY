using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class Row
    {
        public List<string> Values { get; set; }

        public Row() { Values = new List<string>(); }
    }
}