using CoCoWebapplicatiev2.App_Start;
using CoCoWebapplicatiev2.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using System.Web.SessionState;

namespace CoCoWebapplicatiev2
{
    public class Global : System.Web.HttpApplication
    {
        public Sessions sessions;
        

        protected void Application_Start(object sender, EventArgs e)
        {
            Sessions sessions = new Sessions();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }


        protected void Session_Start(object sender, EventArgs e)
        {
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //string clientAddress = HttpContext.Current.Request.UserHostAddress;

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}