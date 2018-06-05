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
    public class WebpagesController : ApiController
    {
        //Fields
        DBWebpages db = new DBWebpages();

        /**
         * @Kyle van Raaij
         * Gets the users personal menu out of the database.
         * 
         * @_userid the id the user uses to log in
         * @_sessionkey the sessioncode given when logging in.
         * @return IEnumerable list with menu items
         **/
        public IEnumerable<MenuItem> GetMenu(string _userId, string _sessionkey)
        {
            //Wat ik wil.
            //Stap 1: userId to identify
            //Stap 2: use a procedure to check rights and return a list with menuItems.
            //Stap 3: return given values

            List<MenuItem> menuItems = db.MakeMenu("1");
            return menuItems;
        }

        public bool GetChatAddMessage(string _userId, string _text)
        { 
            return db.ChatAddMessage(_userId, _text);
        }

        /**
         * @Kyle van Raaij
         * gets all the emplyee documents available.
         * 
         * @_relationID the id the user uses to log in
         * @_sessionCode the sessioncode given when logging in
         * @return list with documents.
         **/
        public List<Document> GetEmployeeDocuments(int _relationID, string _sessionCode)
        {
            return db.GetEmployeeDocuments(_relationID);
        }

        /**
         * @Kyle van Raaij
         * information will be acquired out of the database based of what _gridName is given.
         * the userid parameter will always be made with the name "MID". three other parameter
         * can be added if needed. parameter values need to be given with the value and parametername.
         * for example: parameter1 = "2017,YEAR".
         * 
         * 
         * @_gridName the name of the grid that needs to be looked up.
         * @_userId the id the user uses to log in.
         * @_parameter1 optional string parameter for query's with different parameters needed than _userID
         * @_parameter2 optional string parameter for query's with different parameters needed than _userID
         * @_parameter3 optional string parameter for query's with different parameters needed than _userID
         * @return grid with values
         **/
        public Grid GetGrid(string _gridName, int _userId, string _parameter1 = null, string _parameter2 = null, string _parameter3 = null)
        {
            List<ParameterValue> list = new List<ParameterValue>();
            QueryValues qValues = db.GetGridQuery(_gridName);

            ParameterValue value = new ParameterValue();
            value.ParameterName = "MID";
            value.Value = _userId.ToString();
            list.Add(value);

            

            if(_parameter1 != null)
            {
                string[] splitted = Splitter(_parameter1);
                value = new ParameterValue();
                value.ParameterName = splitted[1];
                value.Value = splitted[0];
                list.Add(value);
            }

            if (_parameter2 != null)
            {
                string[] splitted = Splitter(_parameter2);
                value = new ParameterValue();
                value.ParameterName = splitted[1];
                value.Value = splitted[0];
                list.Add(value);
            }
            //TODO: Parameter 1,2,3 splitten
            //Als ze bestaan toeveogen aan de list.



            //Database klasse gebruiken om de grid op te halen.
            Grid grid = db.GetGridByQuery(qValues, list);
            return grid;
        }

        /**
         * @Kyle van Raaij
         * splits the given text by comma. the values will be put in a list.
         * 
         * @_text the text that needs to be split
         * @return list with the splitted strings.
         **/
        private string[] Splitter(string _text)
        {
            try
            {
                string[] words = _text.Split(',');
                return words;
            }
            catch (NullReferenceException ex)
            {
                return null;
            }
            
        }



    }
}
