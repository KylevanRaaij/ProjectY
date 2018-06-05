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
    public class DBWebpages
    {

        private DatabaseConnectie dbconn = new DatabaseConnectie();


        /**
         * @Kyle van Raaij
         * Checks what the rights are and looks in the database if there are menuitems for that right.
         *  if so the menuitem will be created and added to the list. when all the rights are checked
         *  a list with menuitems wil be returned.
         *  
         * @_userid the id of the user
         * @return List with menuitems
         **/
        public List<MenuItem> MakeMenu(string _userid)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            FbConnection connection = dbconn.GetFbConnection();
            DBAutorisation autorisation = new DBAutorisation();

            foreach (int item in autorisation.GetRights(_userid))
            {
                FbCommand cmd = new FbCommand("SELECT * FROM CCDA_MENUITEM WHERE CCDA_M_ID IN (SELECT CCDA_MR_M_ID FROM CCDA_MENURIGHT WHERE CCDA_MR_AR_ID = @id); ", connection)
                {
                    CommandType = CommandType.Text
                };
                cmd.Parameters.Add("@id", item);
                connection.Open();
                FbDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {

                    while (reader.Read() && reader.GetInt16(0) != 0)
                    {
                        MenuItem menuItem = new MenuItem();
                        if (!reader.IsDBNull(0)) { menuItem.Id = reader.GetInt16(0); } //Menu id 
                        if (!reader.IsDBNull(1)) { menuItem.Name = reader.GetString(1); } //Description
                        if (!reader.IsDBNull(2)) { menuItem.ParentID = reader.GetInt16(2); } //Parent id
                        if (!reader.IsDBNull(3)) { menuItem.Page = reader.GetString(3); } //Action
                        menuItems.Add(menuItem);
                    }

                }
                connection.Close();
            }

            //Zet alle menuItems op de juiste plaats neer.
            List<MenuItem> menuItems2 = new List<MenuItem>();
            //menuItems.RemoveAt(menuItems.Count - 1);
            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ParentID == 0)
                {
                    menuItems2.Add(menuItem);
                }
            }

            foreach (MenuItem menuItem in menuItems)
            {
                if (menuItem.ParentID != 0)
                {
                    foreach (MenuItem item in menuItems2)
                    {
                        if (menuItem.ParentID == item.Id)
                        {
                            item.AddSubItem(menuItem);
                        }
                    }
                }

            }

            return menuItems2;
        }


        //TODO: Maak dit netter, procedure ofzo, code beter maken
        public List<MenuItem> GetMenu(int _right)
        {
            try
            {
                FbConnection conn = dbconn.GetFbConnection();

                FbCommand cmd = new FbCommand("SELECT CCDA_MR_M_ID FROM CCDA_MENURIGHT WHERE CCDA_MR_AR_ID = @right;", conn);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@right", _right);
                conn.Open();

                FbDataReader reader = cmd.ExecuteReader();
                string query2 = "SELECT * FROM CCDA_MENUITEM WHERE CCDA_M_ID =";
                bool isFirst = true;

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (isFirst)
                        {
                            query2 = query2 + reader.GetInt16(0).ToString();
                            isFirst = false;
                        }
                        else
                        {
                            query2 = query2 + "OR CCDA_M_ID = " + reader.GetInt16(0).ToString();
                        }
                    }
                    query2 = query2 + ";";

                    FbCommand cmd2 = new FbCommand(query2, conn);
                    FbDataReader reader2 = cmd2.ExecuteReader();
                    List<MenuItem> Items = new List<MenuItem>();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            MenuItem item = new MenuItem();
                            try { item.Name = reader2.GetString(1); } catch (Exception ex) { Console.WriteLine(ex); }
                            try { item.Id = reader2.GetInt16(0); } catch (Exception ex) { Console.WriteLine(ex); }
                            try { item.ParentID = reader2.GetInt16(2); } catch (Exception ex) { Console.WriteLine(ex); }
                            Items.Add(item);
                        }
                    }
                    return Items;

                }
                else
                {
                    conn.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }
        
        /**
         * @Kyle van Raaij
         * Gets all documents of a specific user.
         * 
         * @_relationID is the ID of the user
         * @return list with the doc id, year, title and version
         **/
        public List<Document> GetEmployeeDocuments(int _relationID)
        {
            FbConnection conn = dbconn.GetFbConnection();
            try
            {
                DateTime CurrentDate = DateTime.Now;
                var Timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();
                FbCommand cmd = new FbCommand("DOC_GET_DOCLIST_FOR_EMPLOYEES", conn);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@relationID", _relationID);
                conn.Open();
                FbDataReader reader = cmd.ExecuteReader();


                if (reader.HasRows)
                {
                    List<Document> tmpList = new List<Document>();
                    while (reader.Read())
                    {

                        Document tmpDoc = new Document();

                        tmpDoc.ID = reader.GetInt16(0);
                        tmpDoc.Year = reader.GetInt16(1);
                        tmpDoc.Title = reader.GetString(2);
                        tmpDoc.Version = reader.GetString(3);
                        tmpList.Add(tmpDoc);

                    }
                    return tmpList;
                }
                return null;
            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                return null;
            }
            finally
            {
                conn.Close();
            }
        }



        public bool ChatAddMessage(string _userId, string  _text)
        {
            try
            {
                FbConnection connection = dbconn.GetFbConnection();
                FbCommand command = new FbCommand("INSERT INTO PNOCHAT (PNOMID, PNOCHATTEXT, PNODATETIME, PNODATE) VALUES ((SELECT PNOMID FROM PNOMEDEWERKER WHERE PNO_MEDERELATIENR = @MID), @TEXT, current_timestamp , current_date );", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@MID", _userId);
                command.Parameters.Add("@TEXT", _text);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /**
         * @Kyle van Raaij
         * Gets a query out of the database table with as name _gridName.
         * All the parameters connected to the database table _gridName will be acquired as well.
         * 
         * @_gridName the name of the grid that is stored in the database
         * @return the query used to get all gridvalues
         **/
        public QueryValues GetGridQuery(string _gridName)
        {
            FbConnection conn = dbconn.GetFbConnection();
            try
            {
                
                FbCommand cmd = new FbCommand("CCDA_GRIDQUERY", conn);
                //FbCommand cmd2 = new FbCommand("SELECT P.CCDA_P_NAME, p.CCDA_P_ID FROM CCDA_GRID G LEFT JOIN CCDA_PARAMETER P ON G.CCDA_G_NAME = P.CCDA_P_PARENTNAME", conn);
                FbCommand cmd2 = new FbCommand("SELECT CCDA_P_NAME, CCDA_P_ID FROM CCDA_PARAMETER WHERE CCDA_P_PARENTNAME = @GRID_NAME", conn);
                cmd2.CommandType = CommandType.Text;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@GRID_NAME", _gridName);
                cmd2.Parameters.Add("@GRID_NAME", _gridName);
                conn.Open();
                string query = (string) cmd.ExecuteScalar();
                FbDataReader reader = cmd2.ExecuteReader();

                QueryValues qValues = new QueryValues();
                qValues.Query = query;
                List<string> values = new List<string>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        values.Add(reader.GetString(0));
                    }
                }
                conn.Close();
                qValues.Parameteres = values;

                return qValues;
            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                conn.Close();
                return null;
            }
            catch(NullReferenceException ex)
            {
                Console.WriteLine(ex);
                conn.Close();
                return null;
            }
        }

        /**
         * @Kyle van Raaij
         * This method will run the given query with the given parameter values.
         * The values will be put in a grid class and given back to the caller.
         * 
         * @_values query and parameters
         * @_parameterValues the values that are put in the given parameters
         * @return grid with the data of the returnvalue of the given databasequery
         **/
        public Grid GetGridByQuery(QueryValues _values, List<ParameterValue> _parameterValues)
        {
            Grid grid = new Grid();

            FbConnection connection = dbconn.GetFbConnection();
            try
            {
                FbCommand command = new FbCommand(_values.Query, connection);
                command.CommandType = CommandType.Text;

                foreach(string parameter in _values.Parameteres)
                {
                    if (_values.Query.Contains("@" + parameter))
                    {
                        foreach(ParameterValue pValue in _parameterValues)
                        {
                            if(pValue.ParameterName == parameter)
                            {
                                command.Parameters.Add("@" + parameter, pValue.Value);
                            }
                        }
                    }
                }

                connection.Open();
                FbDataReader reader = command.ExecuteReader();

                int fieldCount = reader.FieldCount;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Row row = new Row();
                        for (int i = 0; i < fieldCount; i++)
                        {
                            row.Values.Add(reader.GetString(i));
                        }
                        grid.Rows.Add(row);
                    }
                }
                grid.RowCount = fieldCount;
                connection.Close();
                return grid;


            }
            catch (FbException ex)
            {
                Console.WriteLine(ex);
                connection.Close();
                return null;
            }
        }

    }
}