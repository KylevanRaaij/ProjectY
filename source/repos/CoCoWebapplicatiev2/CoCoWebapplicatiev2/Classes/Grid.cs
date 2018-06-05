using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class Grid
    {
        public string Name { get; set; }
        public List<Row> Rows { get; set; }
        public int RowCount { get; set; }

        public Grid() { Rows = new List<Row>(); }

    }
}