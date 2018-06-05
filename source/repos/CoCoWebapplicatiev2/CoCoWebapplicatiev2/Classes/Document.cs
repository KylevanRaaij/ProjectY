using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class Document
    {
        //property's
        public int ID { get; set; }
        public int Year { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }

        //constructor
        public Document() { }
    }
}

