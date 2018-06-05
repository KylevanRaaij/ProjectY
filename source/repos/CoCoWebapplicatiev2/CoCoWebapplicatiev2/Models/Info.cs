using CoCoWebapplicatiev2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace CoCoWebapplicatiev2.Models
{
    public class Info
    {
        public IEnumerable<MenuItem> MenuItem()
        {
            MenuItem mi1 = new MenuItem();
            mi1.Name = "Uren";
            mi1.Page = "#Uren";

            MenuItem mi2 = new MenuItem();
            mi2.Name = "Agenda";
            mi2.Page = "#agenda";

            MenuItem mi3 = new MenuItem();
            mi3.Name = "Chat";
            mi3.Page = "#chat";

            MenuItem mi4 = new MenuItem();
            mi4.Name = "Persoonlijk";
            mi4.Page = "#persoonlijk";

            MenuItem mi4a = new MenuItem();
            mi4a.Name = "Documenten";
            mi4a.Page = "#documenten";

            MenuItem mi4b = new MenuItem();
            mi4b.Name = "Loonstrook";
            mi4b.Page = "#loonstrook";

            MenuItem mi4c = new MenuItem();
            mi4c.Name = "Jaaropgave";
            mi4c.Page = "#jaaropgave";

            mi4.AddSubItem(mi4a);
            mi4.AddSubItem(mi4b);
            mi4.AddSubItem(mi4c);

            MenuItem mi5 = new MenuItem();
            mi5.Name = "IT groep";
            mi5.Page = "#itgroep";

            MenuItem mi6 = new MenuItem();
            mi6.Name = "CoCoZine";
            mi6.Page = "#cocozine";

            List<MenuItem> list = new List<Classes.MenuItem>();
            list.Add(mi1);
            list.Add(mi2);
            list.Add(mi3);
            list.Add(mi4);
            list.Add(mi5);
            list.Add(mi6);

            return list;
        }

    }
}