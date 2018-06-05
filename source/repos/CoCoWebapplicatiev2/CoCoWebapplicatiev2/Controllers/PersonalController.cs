using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoCoWebapplicatiev2.Classes;
using CoCoWebapplicatiev2.Databases;
using CoCoWebapplicatiev2.Models;

namespace CoCoWebapplicatiev2.Controllers
{
    public class PersonalController : ApiController
    {
        //Fields
        DBpersonal db = new DBpersonal();

        //Methods
        public void GetAllWebpages()
        { }
        


        public List<string> GetGeneralInfo(string _userid, string _sessionCode)
        {
            return db.GeneralInfo(_userid);
            //Haalt alle informatie uit de database
            //Zet geef deze info aan de browser.
        }

        public bool GetChangeGeneralInfo(string _userid, string _sessionCode, string _values)
        {
            List<string> SplittedItems = new List<string>();
            string[] values = _values.Split('?');
            foreach (string item in values)
            {
                item.Trim();
                SplittedItems.Add(item);
            }

            return db.ChangeGeneralInfo(_userid, SplittedItems);
        }

    }
}
