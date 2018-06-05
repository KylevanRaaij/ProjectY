using CoCoWebapplicatiev2.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web;


namespace CoCoWebapplicatiev2.Controllers
{
    public class AutorisationController : ApiController
    {
        DBAutorisation db = new DBAutorisation();

        public AutorisationController()
        {
            string salt = RandomString(20); //create salt
            string encryptedText = Sha256Encrypt("Pass123word" + salt); //encrypt text
        }

        /**
         * @Kyle van Raaij 
         * Updates the session to the currentTime. If the
         * session doesn't exist, one will be made.
         * 
         * @_sessionKey The key used to identify the session
         * @_ip The public ip adres of the cliënt.
         * @return true if the session is updated, false if not.
         **/
        public bool GetUpdateSession(string _sessionKey, string _ip)
        {
            //((Global)HttpContext.Current.ApplicationInstance).sessions.UpdateSession("1",_sessionKey);
            db.UpdateSession(_sessionKey, _ip);
            return true;
        }

        /**
         * @Kyle van Raaij
         * Check if session is active.
         * 
         * @_SessionKey The key used to identify the session
         * @return true if session is active, false if not.
         **/
        public bool GetCheckSession(string _sessionKey)
        {
            int tmpRight = db.CheckSession(_sessionKey);
            if(tmpRight == 1)
            {
                return true;
            } else
            {
            return false;
            }

        }

        /**
         * @Kyle van Raaij
         * check if the username and password are correct.
         * 
         * @_username the username the user filled in.
         * @_password the password the user filled in.
         * @return sessionkey if login is accepted. null if not.
         **/
        public string GetLogin(string _username, string _password) {

            try
            {
                string sessionkey = CheckLoginRights(_username, _password);
                return sessionkey;

            } catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }

        /**
         * @Kyle van Raaij
         * Gets the rights needed to get acces to certain parts of the application
         * when _isMenu is true the menurights will be looked up, if false then the
         * normal rights will be acquired.
         * 
         * @_username the id the user uses to log in
         * @_sessionkey the sessioncode given when logging in
         * @_isMenu check if the rights are for a menu or not
         * @return  List with int values, this are the right id's stored in the database
         **/
        public List<int> GetRights(string _username, string _sessionkey, bool _isMenu)
        {
            try
            {
                //TODO: Check if sessionkey is correct
                if (_isMenu)
                {
                    List<int> rightsList = db.GetMenuIds(_username);
                    return rightsList;
                } else
                {
                    List<int> rightsList = db.GetRights(_username);
                    return rightsList;
                }
               
            }catch(Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }
        }


        /**
         * @Kyle van Raaij
         * Stops the session. the user will be logged out.
         * 
         * @_sessionkey: the key the user used to acces the website
         **/
        public void UserLogout(string _sessionkey) {
            //1. Log de gebruiker uit

        }

        /**
         * @Kyle van Raaij
         * Checks if the username and password are correct. if so the user will
         * get acces to application through a sessionkey.
         * 
         * @_username the username of the user
         * @_password the password of the user
         * @return string with a sessionkey if acces is granted, null if not.
         **/
        private string CheckLoginRights(string _username, string _password)
        {
            DBAutorisation db = new DBAutorisation();
            List<string> list = db.Login(_username);

            try
            {
                string salt = list[0];
                string pass = list[1];

                string hashed = Sha256Encrypt(_password + salt);

                if (hashed == pass)
                {
                    string sessionkey = RandomString(20);
                    return sessionkey;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null;
            }

            return null;
        }

        /**
         * @Kyle van Raaij
         * Create a new random string that can be used as sessionkey or salt.
         * 
         * @_legnth the length the string return string will be.
         * @Return random string
         **/
        static string RandomString(int _length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (_length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }

            return res.ToString();
        }

        /**
         * @Kyle van Raaij
         * Encrypts the string to a sha256 algorithm text.
         * 
         * @_randomString the string that will be encrypted
         * @return encrypted string
         **/
        static string Sha256Encrypt(string _randomString)
        {
            byte[] data = Encoding.UTF8.GetBytes(_randomString);
            using (HashAlgorithm sha = new SHA256Managed())
            {
                byte[] encryptedBytes = sha.TransformFinalBlock(data, 0, data.Length);
                return Convert.ToBase64String(sha.Hash);
            }
        }



    }
}
