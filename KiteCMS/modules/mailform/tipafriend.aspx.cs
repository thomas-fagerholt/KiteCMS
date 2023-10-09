using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.Mail;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for mailform.
	/// </summary>
    public class TipAFriend : System.Web.UI.Page
	{
		protected Literal lbContent;

		private void Page_Load(object sender, System.EventArgs e)
		{
			string mailFrom = "";
            string domain = "";
			string returnUrl = "";
            string mailto = "";
            string subject = "";
            bool requeredPassed = true;
            bool useCaptcha = false;
			string body;

            if (HttpContext.Current.Request["requeredList"] != null)
            {
                foreach (string field in HttpContext.Current.Request["requeredList"].Split(','))
                    if ( HttpContext.Current.Request[field.Trim()] == null || HttpContext.Current.Request[field.Trim()].ToString() == "")
                        requeredPassed = false;
            }
            if (HttpContext.Current.Request["mailTo"]!= null)
                mailto = HttpContext.Current.Request["mailTo"];

            if (ConfigurationManager.AppSettings["useCaptcha"] != null)
                useCaptcha = bool.Parse(ConfigurationManager.AppSettings["useCaptcha"]);

            // if CaptchaFixedValue is given, check if value is in request
            if (useCaptcha && ConfigurationManager.AppSettings["CaptchaFixedValue"] != null)
            {
                if (HttpContext.Current.Request["captcha"] != null && HttpContext.Current.Request["captcha"] == ConfigurationManager.AppSettings["CaptchaFixedValue"])
                    useCaptcha = false;
            }

            if (useCaptcha && ( HttpContext.Current.Request["captcha"] == null || this.Session["CaptchaImageText"] == null || HttpContext.Current.Request["captcha"].CompareTo(Session["CaptchaImageText"].ToString()) != 0))
            {
                if (ConfigurationManager.AppSettings["useCaptchaErrortext"] != null)
                    HttpContext.Current.Response.Write(ConfigurationManager.AppSettings["useCaptchaErrortext"]);
                else
                    HttpContext.Current.Response.Write("Control characters (Captcha) not correct. Please use the \"back\"-button to return to the page<br/>");
                HttpContext.Current.Response.End();
            }
            else if (!requeredPassed)
                HttpContext.Current.Response.Write("Not all mandatory fields are field out. Please use the \"back\"-button to return to the page and fill out all mandatory fields<br/>Ikke alle nødvendige felter er udfyldt. Brug venligst \"tilbage\"-knappen i browsere til at gå tilbage til siden og udfyld alle nødvendige felter");
            else if (HttpContext.Current.Request["fieldsList"] != null && mailto != "" && HttpContext.Current.Request["returnUrl"] != null && HttpContext.Current.Request["fieldsList"].ToString() != "" && HttpContext.Current.Request["mailTo"].ToString() != "" && HttpContext.Current.Request["returnUrl"].ToString() != "")
			{
                domain = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
                if (domain.Split('.').Length == 3)
                    domain = domain.Substring(domain.IndexOf(".") + 1);

                if (mailto.IndexOf('@') == -1)
                    mailto += "@" + domain;

                if (ConfigurationManager.AppSettings["mailFrom"] != null)
                    mailFrom = ConfigurationManager.AppSettings["mailFrom"].ToString();
                else
                    mailFrom = "info@" + domain;
                
                //Get template from data-folder
                FileStream stream = File.OpenRead(Global.publicXmlPath + @"/tipafriend.txt");
                StreamReader sr = new StreamReader(stream, Encoding.GetEncoding("ISO-8859-1"));
                subject = sr.ReadLine();
                body = sr.ReadToEnd();
                sr.Close();
                stream.Close();

				//collect data from form
                foreach (string field in HttpContext.Current.Request["fieldsList"].Split(','))
                    body = body.Replace("##" + field + "##", Server.UrlDecode(HttpContext.Current.Request[field.Trim()]));

                var mailsent = Functions.SendMail(subject, body, mailto, mailFrom);

                // Redirect the user
                returnUrl = HttpContext.Current.Request["ReturnURL"].ToString();
				if(returnUrl.IndexOf('?')>0)
					returnUrl = returnUrl +"&mailstatus=ok";
				else
					returnUrl = returnUrl +"?mailstatus=ok";

				HttpContext.Current.Response.Redirect(returnUrl);
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
