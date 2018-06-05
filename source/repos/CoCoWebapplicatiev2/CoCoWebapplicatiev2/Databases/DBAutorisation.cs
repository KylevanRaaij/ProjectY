using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using CoCoWebapplicatiev2.Classes;
using FirebirdSql.Data.FirebirdClient;

namespace CoCoWebapplicatiev2.Databases
{
    public class DBAutorisation
    {
        DatabaseConnectie dbconn = new DatabaseConnectie();

        /**
         * @Kyle van Raaij
         * checks what the password and salt are and send a the salt string and the hashed password
         * string back. This method is used so that there can be checked if the given password and
         * this password are the same if the salt is added.
         * 
         * @_userId the id the user uses to log in
         * @Return List with the salt and hashed password
         **/
        public List<string> Login(string _userId)
        {
            
            try
            {
                FbConnection conn = dbconn.GetFbConnection();

                FbCommand cmd = new FbCommand("select RPSALT, RPHASHED_PASSWORD from RELATIEPERSOON WHERE RPNR = @userid;", conn);

                //FbCommand cmd = new FbCommand("insert into t1(id, text) values (@id, @text);");
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@userid", _userId);
                conn.Open();
                FbDataReader reader = cmd.ExecuteReader();

                List<string> values = new List<string>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        values.Add(reader.GetString(0)); //salt
                        values.Add(reader.GetString(1)); //hashed pass
                    }
                    conn.Close();
                    return values;
                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /**
         * @Kyle van Raaij
         * Checks what rights the user has in the database. The database returns 1 if the user has
         * the right to do that action and 0 if not. when the database returns 1 the actionID is
         * added to a list. When all ActionIds are checked the list with rights will be returned.
         * 
         * @_userid the id the user uses to log in
         * @Return list with al the actionID rights
         **/
        public List<int> GetRights(string _userid)
        {
            try
            {
                //TODO : Deze haalt nu nog alleen de menurechten op. QUERY VERANDEREN
                //-----------------------------------------------------------------
                //-----------------------------------------------------------------
                FbConnection conn2 = dbconn.GetFbConnection();

                FbCommand cmd2 = new FbCommand("SELECT DISTINCT CCDA_MR_AR_ID FROM CCDA_MENURIGHT;", conn2);

                cmd2.CommandType = CommandType.Text;
                conn2.Open();
                FbDataReader reader = cmd2.ExecuteReader();
                List<int> cocoAppsPagesList = new List<int>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        cocoAppsPagesList.Add(reader.GetInt16(0));

                    }
                }
                conn2.Close();
                //-----------------------------------------------------------------
                //-----------------------------------------------------------------



                //List<int> cocoAppsPagesList = new List<int>() { 117, 118, 119, 120, 121, 122, 123, 124, 125, 126, 131, 146, 131, 132, 133, 134, 135, 136, 137, 138, 139, 140, 141, 142, 143, 144 };
                List<int> hasRightsList = new List<int>();

                FbConnection conn = dbconn.GetFbConnection();

                using (FbCommand cmnd = new FbCommand("IS_ROLE_ACTIVE_RELATION", conn))
                {
                    conn.Open();
                    foreach (int item in cocoAppsPagesList)
                    {
                        cmnd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmnd.Parameters.Add("ROLETOCHECK", FbDbType.Integer).Value = item; //ID in DYN_MEDEWERKER2ROLE
                        cmnd.Parameters.Add("RELATIONTOCHECK", FbDbType.VarChar).Value = _userid; //PNO_MEDERELATIENR in PNO_MEDEWERKER TABEl

                        int value = (int)cmnd.ExecuteScalar(); //1: yes, 0: no
                        if (value == 1)
                        {
                            hasRightsList.Add(item);
                        }
                        cmnd.Parameters.Clear();
                    }
                    conn.Close();
                }
                return hasRightsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /**
         * @Kyle van Raaij
         * Checks what MenuItem ID's the user has acces to.
         * 
         * @_userid The ID the user uses to log in.
         * @Return List with al the menuIds where the user has the right to.
         **/
        public List<int> GetMenuIds(string _userid)
        {
            try
            {
                List<int> tmpList = new List<int>();
                List<int> tmpList2 = new List<int>();

                //PART 1 : Get all the ID's of the CCDA_MENURIGHT
                FbConnection connection = dbconn.GetFbConnection(); // Connection with the database
                FbCommand command = new FbCommand("SELECT DISTINCT CCDA_MR_AR_ID FROM CCDA_MENURIGHT;", connection);
                command.CommandType = CommandType.Text;

                connection.Open();
                FbDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tmpList.Add(reader.GetInt16(0));

                    }
                }

                command.Parameters.Clear();
                connection.Close();

                //PART 2: Get all the ID's of the DYN_ACCESS_ROLE where the user has the right to
                using (command = new FbCommand("IS_ROLE_ACTIVE_RELATION", connection))
                {
                    connection.Open();
                    foreach (int item in tmpList)
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add("ROLETOCHECK", FbDbType.Integer).Value = item; //ID in DYN_MEDEWERKER2ROLE
                        command.Parameters.Add("RELATIONTOCHECK", FbDbType.VarChar).Value = _userid; //PNO_MEDERELATIENR in PNO_MEDEWERKER TABEl

                        int value = (int)command.ExecuteScalar(); //1: yes, 0: no
                        if (value == 1)
                        {
                            tmpList2.Add(item);
                        }
                        command.Parameters.Clear();
                    }
                    connection.Close();
                }

                //PART 3: Get all the menuItems where the user has the right to.
                tmpList.Clear();

                connection.Open();
                foreach (int item in tmpList2)
                {
                    command = new FbCommand("SELECT CCDA_M_ID FROM CCDA_MENUITEM WHERE CCDA_M_PARENT IS NULL AND CCDA_M_ID IN (SELECT CCDA_MR_M_ID FROM CCDA_MENURIGHT WHERE CCDA_MR_AR_ID = @id);", connection);
                    command.Parameters.Add("@id", FbDbType.Integer).Value = item;

                    object value = command.ExecuteScalar();
                    tmpList.Add((int)value);

                    command.Parameters.Clear();
                }
                connection.Close();


                return tmpList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /**
         * @Kyle van Raaij
         * Checks in the database if the session is active. 
         * 
         * @_sessionKey The key used to check if the user is active.
         * @return 0 if session doesn't exist or is expired, 
         * 1 if the session is ongoing and 404 if there was an error.
         **/
        public int CheckSession(string _sessionKey)
        {
            FbConnection conn = dbconn.GetFbConnection();
            try
            {
                int status = 404;
                DateTime CurrentDate = DateTime.Now;
                var Timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                FbCommand cmd = new FbCommand("CCDA_SESSION_CHECK", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userid", _sessionKey);
                conn.Open();
                status = (int)cmd.ExecuteScalar();
                conn.Close();

                return status;
            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                return 404;
            }
            finally
            {
                conn.Close();
            }
        }

        /**
         * @Kyle van Raaij
         * Updates the session in the database if the user exists.
         * if the user doesn't exist the session cannot be updated.
         * 
         * @_sessionKey The key used to check if the user is active.
         * @_userIP The IP the cliënt uses.
         * @return true if the session is updated, false if not.
         **/
        public bool UpdateSession(string _sessionKey, string _userIP)
        {
            FbConnection conn = dbconn.GetFbConnection();

            try
            {
                DateTime CurrentDate = DateTime.Now;
                var Timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                FbCommand cmd = new FbCommand("CCDA_SESSION_UPDATE", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@userid", _sessionKey);
                cmd.Parameters.Add("@userIp", _userIP);
                cmd.Parameters.Add("@timestamp", CurrentDate);
                conn.Open();
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}