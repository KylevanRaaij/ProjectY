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
    public class DBpersonal
    {
        DatabaseConnectie dbconn = new DatabaseConnectie();

        /**
         * @Kyle van Raaij
         * gets surename, insertion, lastname, initials, callsign, emailgsm, tel, 
         * skypename, fax, birthplace and birthdate of a specific user.
         * 
         * @_userId: the id of the user where the database looks for.
         * @return: list with all avaliable general information.
         **/
        public List<string> GeneralInfo(string _userId)
        {
            try
            {
                FbConnection connection = dbconn.GetFbConnection();
                FbCommand command = new FbCommand("SELECT PNO_MEDERELATIENR, PNO_PERS_VNAAM, PNO_PERS_VOORV, PNO_PERS_ANAAM, PNO_PERS_VOORL, PNO_PERS_RNAAM, PNO_ADRES_EMAI, PNO_ADRES_GSM, PNO_ADRES_TELE, PNO_ADRES_SKYPE, PNO_ADRES_FAX, PNO_PERS_GEBPL, PNO_PERS_GEBDA FROM PNOMEDEWERKER WHERE PNO_MEDERELATIENR = @userid;", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@userid", _userId);
                connection.Open();
                FbDataReader reader = command.ExecuteReader();

                List<string> values = new List<string>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0)) { values.Add(reader.GetInt32(0).ToString()); } else { values.Add(""); } //RelatiepersoonID
                        if (!reader.IsDBNull(1)) { values.Add(reader.GetString(1)); } else { values.Add(""); }  //Voornaam
                        if (!reader.IsDBNull(2)) { values.Add(reader.GetString(2)); } else { values.Add(""); }  //Tussenvoegsel
                        if (!reader.IsDBNull(3)) { values.Add(reader.GetString(3)); } else { values.Add(""); }  //Achternaam
                        if (!reader.IsDBNull(4)) { values.Add(reader.GetString(4)); } else { values.Add(""); }  //Initialen
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(5)); } else { values.Add(""); }  //Roepnaam
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(6)); } else { values.Add(""); }  //E-mail
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(7)); } else { values.Add(""); }  //GSM
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(8)); } else { values.Add(""); }  //Telefoonnummer
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(9)); } else { values.Add(""); }  //Skypenaam
                        if (!reader.IsDBNull(5)) { values.Add(reader.GetString(10)); } else { values.Add(""); }  //Fax nummer
                        if (!reader.IsDBNull(6)) { values.Add(reader.GetString(11)); } else { values.Add(""); }  //Geboorteplaats
                        if (!reader.IsDBNull(7)) { values.Add(reader.GetDateTime(12).ToString("dd.MM.yyy")); } else { values.Add(""); }  //Geboortedatum (data)
                    }
                    connection.Close();
                    return values;
                }
                else
                {
                    connection.Close();
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
         * update pnomedechange surename, insertion, lastname, initials, callsign, email
         * gsm, tel, skypename, fax, birthplace and birthdate in the database.
         * when accepted by the administrator, the pnomedechange values will be updated
         * in the pnomedewerker table.
         * 
         * @_userId the id the user uses to log in
         * @_values list with the values that needs to be updated
         * @return true if succesfull, false if not
         **/
        public bool ChangeGeneralInfo(string _userId, List<string> _values)
        {
            try
            {
                FbConnection connection = dbconn.GetFbConnection();
                FbCommand command = new FbCommand("UPDATE PNOMEDEWERKER SET PNO_PERS_VNAAM = @value1, PNO_PERS_VOORV = @value2, PNO_PERS_ANAAM = @value3, PNO_PERS_VOORL = @value4, PNO_PERS_RNAAM = @value5, PNO_ADRES_EMAI = @value6, PNO_ADRES_GSM = @value7, PNO_ADRES_TELE = @value8, PNO_ADRES_SKYPE = @value9, PNO_ADRES_FAX = @value10, PNO_PERS_GEBPL = @value11, PNO_PERS_GEBDA = @value12 WHERE PNO_MEDERELATIENR = @userid; ", connection);
                command.CommandType = CommandType.Text;
                command.Parameters.Add("@value1", _values[0]);
                command.Parameters.Add("@value2", _values[1]);
                command.Parameters.Add("@value3", _values[2]);
                command.Parameters.Add("@value4", _values[3]);
                command.Parameters.Add("@value5", _values[4]);
                command.Parameters.Add("@value6", _values[5]);
                command.Parameters.Add("@value7", _values[6]);
                command.Parameters.Add("@value8", _values[7]);
                command.Parameters.Add("@value9", _values[8]);
                command.Parameters.Add("@value10", _values[9]);
                command.Parameters.Add("@value11", _values[10]);
                command.Parameters.Add("@value12", _values[11]);
                command.Parameters.Add("@userid", _userId);
                connection.Open();

                command.ExecuteNonQuery();


                connection.Close();
                return true;
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        /**
         * @Kyle van Raaij
         * gets all information that is related to family.
         * 
         * @_userId: the id of the user where the database looks for.
         * @return list with all avaliable family information.
         **/
        public List<string> FamilyInfo(string _userId)
        {
            //Do stuff
            return null;
        }

        /**
         * @Kyle van Raaij
         * gets all information that is related to clothing.
         * 
         * @_userId: the id of the user where the database looks for.
         * @return list with all avaliable family information.
         **/
        public List<string> ClothingInfo(string _userId)
        {
            //Do stuff
            return null;
        }

        /**
         * @Kyle van Raaij
         * gets all bank information.
         * 
         * @_userId: the id of the user where the database looks for.
         * @return list with all avaliable family information.
         **/
        public List<string> BankInfo(string _userId)
        {
            //Do stuff
            return null;
        }

        /**
         * @Kyle van Raaij
         * gets all information needed in a emergency.
         * 
         * @_userId: the id of the user where the database looks for.
         * @return list with all avaliable family information.
         **/
        public List<string> EmergencyInfo(string _userId)
        {
            //Do stuff
            return null;
        }
    }
}