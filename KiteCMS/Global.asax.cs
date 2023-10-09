using System;
using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Net.Mail;
using System.Web.SessionState;
using System.Xml;
using System.Diagnostics;
using System.Configuration;
using KiteCMS.Admin;

namespace KiteCMS 
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global : System.Web.HttpApplication
	{
		public const bool isDemo = false;
        public const string version = "1.0.0" + (isDemo ? " demoversion" : "");
        public static bool hasBoxmodule = false;
        public static bool ValidXhtml = false;
        public static bool WAI = false;
        public static XmlDocument oMenuXml = new XmlDocument(); 
		public static ParameterCollection LanguageTexts = new ParameterCollection();
		public static string menuXmlLockSession = "";
		public static DateTime menuXmlLockTime = new DateTime();
		public static string adminXmlPath = "";
		public static string publicXmlPath = "";
		public static string DefaultStylesheet = "/default.css";

        public XmlDocument oLocalMenuXml = new XmlDocument();
        public EditModeEnum EditMode;

		public Global()
		{
			InitializeComponent();
		}	
		
		protected void Application_Start(Object sender, EventArgs e)
		{
			Debug.WriteLine("Application_Start kaldt");

            if (ConfigurationManager.AppSettings["adminXMLpath"] != null && ConfigurationManager.AppSettings["adminXMLpath"] != "")
            {
                if (ConfigurationManager.AppSettings["adminXMLpath"].IndexOf("/") > 0)
                    throw new Exception("adminXMLpath cannot contain '/' - must be '\\'");
                adminXmlPath = ConfigurationManager.AppSettings["adminXMLpath"];
            }
            else
                adminXmlPath = Server.MapPath(@"\admin\data");

            if (ConfigurationManager.AppSettings["publicXMLpath"] != null && ConfigurationManager.AppSettings["publicXMLpath"] != "")
            {
                if (ConfigurationManager.AppSettings["publicXMLpath"].IndexOf("/") > 0)
                    throw new Exception("publicXMLpath cannot contain '/' - must be '\\'");
                publicXmlPath = ConfigurationManager.AppSettings["publicXMLpath"];
            }
            else
                publicXmlPath = Server.MapPath(@"\data");

			if (ConfigurationManager.AppSettings["DefaultStylesheet"] != null && ConfigurationManager.AppSettings["DefaultStylesheet"] != "")
				DefaultStylesheet = ConfigurationManager.AppSettings["DefaultStylesheet"];

			if (ConfigurationManager.AppSettings["hasBoxmodule"] != null && ConfigurationManager.AppSettings["hasBoxmodule"] != "")
				hasBoxmodule = bool.Parse(ConfigurationManager.AppSettings["hasBoxmodule"]);

            if (ConfigurationManager.AppSettings["validXHTML"] == null || ConfigurationManager.AppSettings["validXHTML"].ToString() == "true")
                ValidXhtml = true;

            if (ConfigurationManager.AppSettings["WAI"] != null && ConfigurationManager.AppSettings["WAI"].ToString() == "true")
                WAI = true;

//            oMenuXml.PreserveWhitespace = true;
            if (System.IO.File.Exists(Global.publicXmlPath + "/website.xml"))
                oMenuXml.Load(publicXmlPath + "/website.xml");
			
			string defaultLanguage = "";
			defaultLanguage = ConfigurationManager.AppSettings["defaultAdminLanguage"];
			Functions.loadAdminTexts();
			
		}

        protected void Session_Start(Object sender, EventArgs e)
        {
            if (Request.UserAgent != null && Request.Browser != null)
            {
                Session["IEBrowser"] = (Request.UserAgent.IndexOf("MSIE") > -1);

                if (Session["IEBrowser"].ToString() == "True" && Request.Browser.MajorVersion >= 6)
                    Session["IsContentEditable"] = true;
                if (Request.UserAgent.IndexOf("Netscape/") >= 0 && Request.Browser.MajorVersion >= 5)
                    // netscape 7
                    Session["IsContentEditable"] = true;
                if (Request.UserAgent.IndexOf("Firefox/") >= 0 && Request.Browser.MajorVersion >= 5)
                    // firefox
                    Session["IsContentEditable"] = true;
            }
            else
            {
                Session["IEBrowser"] = false;
                Session["IsContentEditable"] = false;
            }

        }

		protected void Application_BeginRequest(Object sender, EventArgs e)
		{
        }

        protected void Application_PreRequestHandlerExecute(Object sender, EventArgs e)
        {
            if (!Admin.Admin.IsLoggedIn())
                this.EditMode = EditModeEnum.Public;
            else
                if ((HttpContext.Current.Request.Form["action"] != null && HttpContext.Current.Request.Form["action"].ToLower() == "editcontent") || (HttpContext.Current.Request.QueryString["action"] != null && HttpContext.Current.Request.QueryString["action"].ToLower() == "editcontent"))
                    this.EditMode = EditModeEnum.AdminEdit;
                else if ((HttpContext.Current.Request.Form["action"] != null && HttpContext.Current.Request.Form["action"].ToLower() == "editdraftcontent") || (HttpContext.Current.Request.QueryString["action"] != null && HttpContext.Current.Request.QueryString["action"].ToLower() == "editdraftcontent"))
                    this.EditMode = EditModeEnum.AdminEditDraft;
                else
                    this.EditMode = EditModeEnum.Admin;

		}

		protected void Application_EndRequest(Object sender, EventArgs e)
		{

		}

		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{

		}

        protected void Application_Error(object sender, EventArgs e)
        {
            // unlock menuXml
            Global.menuXmlLockSession = "";
            Global.menuXmlLockTime = new DateTime();

            string strError = "Error in: " + Request.Path + "\nUrl: " + Request.RawUrl + "\n\n";

            // Get the exception object for the last error message that occured.
            Exception ErrorInfo = Server.GetLastError().GetBaseException();
            if (ErrorInfo.TargetSite.ToString().ToLower() != "void checkvirtualfileexists(system.web.virtualpath)" && ErrorInfo.Message.IndexOf("A potentially dangerous Request.Form value was detected from the client") < 0)
            {
                strError += "Error Message: " + ErrorInfo.Message +
                    "\nError Source: " + ErrorInfo.Source +
                    "\nError Target Site: " + ErrorInfo.TargetSite +
                    "\n\nQueryString Data:\n-----------------\n";

                // Gathering QueryString information
                for (int i = 0; i < Context.Request.QueryString.Count; i++)
                    strError += Context.Request.QueryString.Keys[i] + ":\t\t" + Context.Request.QueryString[i] + "\n";
                strError += "\nPost Data:\n----------\n";

                // Gathering Post Data information
                for (int i = 0; i < Context.Request.Form.Count; i++)
                    strError += Context.Request.Form.Keys[i] + ":\t\t" + Context.Request.Form[i] + "\n";
                strError += "\n";

                if (User.Identity.IsAuthenticated) strError += "User:\t\t" + User.Identity.Name + "\n\n";

                strError += "Exception Stack Trace:\n----------------------\n" + Server.GetLastError().StackTrace + "\n\nServer Variables:\n-----------------\n";

                // Gathering Server Variables information
                for (int i = 0; i < Context.Request.ServerVariables.Count; i++)
                    if (Context.Request.ServerVariables.Keys[i] != "AUTH_PASSWORD")
                        strError += Context.Request.ServerVariables.Keys[i] + ":\t\t" + Context.Request.ServerVariables[i] + "\n";
                strError += "\n";

                if (ConfigurationManager.AppSettings["adminemail"] != null)
                {
                    // Sending error message to administration via e-mail
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.IsBodyHtml = false;
                    mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["adminemail"]);
                    mailMessage.To.Add(ConfigurationManager.AppSettings["adminemail"]);
                    mailMessage.Subject = HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + ": Error on site";
                    mailMessage.Body = strError;
                    // SMTP Authentication
                    SmtpClient emailClient = new SmtpClient(ConfigurationManager.AppSettings["smtpMailServer"]);
                    if (ConfigurationManager.AppSettings["smtpMailServerUsername"] != null && ConfigurationManager.AppSettings["smtpMailServerPassword"] != null)
                    {
                        System.Net.NetworkCredential SMTPUserInfo = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["smtpMailServerUsername"], ConfigurationManager.AppSettings["smtpMailServerPassword"]);
                        emailClient.UseDefaultCredentials = false;
                        emailClient.Credentials = SMTPUserInfo;
                    }
                    try
                    {
                        emailClient.Send(mailMessage);
                    }
                    catch { }
                }
            }
        }

		protected void Session_End(Object sender, EventArgs e)
		{

		}

		protected void Application_End(Object sender, EventArgs e)
		{

		}

   		public enum EditModeEnum : int
		{
			Public = 0, Admin = 1, AdminEdit = 2 , AdminEditDraft = 3
		}

		#region Web Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
		}
		#endregion
	}
}

