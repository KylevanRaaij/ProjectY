using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class MenuItem
    {
        //Properties
        public int Id { get; set; }
        public string Name { get; set; }
        public string Page { get; set; }
        public int ParentID { get; set; }
        public List<MenuItem> SubItem { get; set; }

        //Constructor
        public MenuItem()
        {
            SubItem = new List<MenuItem>();
        }

        public void AddSubItem(MenuItem _menuItem)
        {
            SubItem.Add(_menuItem);
        }

        public void ClearSubItems()
        {
            SubItem = new List<MenuItem>();
        }

    }
}