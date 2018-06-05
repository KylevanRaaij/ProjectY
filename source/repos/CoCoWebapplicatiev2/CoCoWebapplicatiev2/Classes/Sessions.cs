using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoCoWebapplicatiev2.Classes
{
    public class Sessions
    {

        private static List<User> users = new List<User>();

        public Sessions()
        {
            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromMinutes(5);

            var timer = new System.Threading.Timer((e) =>
            {
                CheckSession();
            }, null, startTimeSpan, periodTimeSpan);

        }

        public void CheckSession()
        {
            try
            {
                DateTime date = new DateTime();
                date = date.AddMinutes(-10);
                List<User> tmpUsers = new List<User>();

                foreach (User user in users)
                {
                    if (date < user.DateCheck)
                    {
                        tmpUsers.Add(user);
                    }
                }
                users = tmpUsers;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }


        public bool UpdateSession(string _userid, string _session)
        {
            try
            {
                foreach(User user in users)
                {
                    if(user.SessionKey == _session && user.UserID == _userid)
                    {
                        user.DateCheck = new DateTime();
                        return true;
                    }
                }
                return false;
            }
            catch( Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return false;
            }
        }

        public bool SetSession(string _userid, string _session)
        {
            try
            {
                User user = new User();
                user.UserID = _userid;
                user.SessionKey = _session;
                user.DateCheck = new DateTime();
                users.Add(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
                return false;
            }

        }

        public static string RemoveSession(string _userid, string _session)
        {

            return null;
        }

    }
}