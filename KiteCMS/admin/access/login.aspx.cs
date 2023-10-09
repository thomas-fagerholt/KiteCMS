using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for login.
	/// </summary>
	public class login : System.Web.UI.Page
	{
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			Functions functions = new Functions();

			string strPasswordEncrypt = "";
			string cookieString ="";
			XmlDocument oXmlAccess = new XmlDocument();
			XmlNode oXmlUser = null;
			XmlNode oXmlNode;
			XmlElement oXmlElem;
			XmlCDataSection oXmlCdata;
			int loginpageid = 0;

            //check if logging in
            if (HttpContext.Current.Request["action"] != null && HttpContext.Current.Request["action"] == "login")
            {
                oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");

                //oXmlUser = oXmlAccess.SelectSingleNode("//user[username='" + HttpContext.Current.Request.Form["username"].Replace("'", "").ToLower() + "']");

                var nodes = oXmlAccess.SelectNodes("//user");
                foreach (XmlNode node in nodes)
                {
                    if (node.SelectSingleNode("username").InnerText.ToLower() == HttpContext.Current.Request.Form["username"].Replace("'", "").ToLower())
                    {
                        oXmlUser = node;
                        break;
                    }
                }

                if (oXmlUser != null)
                {
                    // Account is inactive
                    if (oXmlUser.Attributes["active"].Value != "1")
                        HttpContext.Current.Response.Redirect("/admin/access/login.aspx?msg=loginclosed");

                    // Check password
                    strPasswordEncrypt = Functions.Encrypt(HttpContext.Current.Request["password"]);
                    if (oXmlUser.SelectSingleNode("password").InnerText == strPasswordEncrypt || oXmlUser.SelectSingleNode("password").InnerText.StartsWith("***"))
                    {
                        // Save new encrypted password if requested
                        if (oXmlUser.SelectSingleNode("password").InnerText.StartsWith("***"))
                        {
                            strPasswordEncrypt = Functions.Encrypt(oXmlUser.SelectSingleNode("password").InnerText.Substring(3));
                            oXmlUser.SelectSingleNode("password").FirstChild.InnerText = strPasswordEncrypt;
                        }

                        HttpContext.Current.Session["userid"] = oXmlUser.Attributes["id"].InnerText;

                        // Set cookiestring
                        if (oXmlUser.SelectSingleNode("cookiestring") != null)
                            cookieString = oXmlUser.SelectSingleNode("cookiestring").InnerText;
                        else
                        {
                            Random r = new Random();
                            for (int i = 1; i <= 64; i++)
                                cookieString += Convert.ToChar(Convert.ToInt16(r.NextDouble() * 60 + 65));
                            oXmlElem = oXmlAccess.CreateElement("cookiestring", "");
                            oXmlCdata = oXmlAccess.CreateCDataSection(cookieString);
                            oXmlElem.AppendChild(oXmlCdata);
                            oXmlUser.AppendChild(oXmlElem);
                        }
                        HttpContext.Current.Response.Cookies["user"].Value = cookieString;

                        //Update faildlogins i XML
                        oXmlUser.Attributes["failedlogins"].Value = "0";

                        // Save XML
                        try
                        {
                            oXmlAccess.Save(Global.adminXmlPath + "/access.webinfo");
                        }
                        catch (System.IO.IOException ex)
                        {
                            throw new Exception(ex.InnerException + "<br/>" + "access.webinfo content:" + ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.OuterXml);
                        }


                        // Find first page the user has access to
                        if (oXmlUser.SelectSingleNode("menuid") != null)
                            loginpageid = int.Parse(oXmlUser.SelectSingleNode("menuid").InnerText);
                        if (loginpageid != 0)
                        {
                            try
                            {
                                // Cannot redirect to nonpublic page
                                Page adminpage = new KiteCMS.Admin.Page(loginpageid);
                                Template template = new Template(adminpage.TemplateId);
                                HttpContext.Current.Response.Redirect(template.Publicurl + "?pageid=" + loginpageid);
                            }
                            catch (ThreadAbortException)
                            {
                            }
                            catch
                            {
                                // default - redirect to frontpage
                                HttpContext.Current.Response.Redirect("/");
                            }
                        }
                        else
                        {
                            // No pageId in request. Take first visible in the menu
                            oXmlNode = Global.oMenuXml.SelectSingleNode("//page/usetemplate");
                            if (oXmlNode != null)
                            {
                                string templateid = oXmlNode.InnerText;
                                oXmlNode = Global.oMenuXml.SelectSingleNode("//template[@id='" + templateid + "']");
                                if (oXmlNode != null)
                                    HttpContext.Current.Response.Redirect(oXmlNode.SelectSingleNode("publicurl").InnerText);
                            }
                            HttpContext.Current.Response.Redirect("/");
                        }
                    }
                    else
                    {
                        // password wrong
                        oXmlUser.Attributes["failedlogins"].Value = Convert.ToString(int.Parse(oXmlUser.Attributes["failedlogins"].Value) + 1);

                        // Check if the is to many wrong logins. If yes, then deactivate
                        if (int.Parse(oXmlUser.Attributes["failedlogins"].Value) > int.Parse(ConfigurationManager.AppSettings["maxFailedLogins"]))
                            oXmlUser.Attributes["active"].Value = "0";

                        // Save XML
                        try
                        {
                            oXmlAccess.Save(Global.adminXmlPath + "/access.webinfo");
                        }
                        catch (System.IO.IOException ex)
                        {
                            throw new Exception(ex.InnerException + "<br/>" + "access.webinfo content:" + oXmlAccess.OuterXml);
                        }

                        HttpContext.Current.Response.Redirect("/admin/access/login.aspx?msg=wronglogin");
                    }
                }
                else
                    HttpContext.Current.Response.Redirect("/admin/access/login.aspx?msg=wronglogin");
            }

            //Set the wished language for the administration pages
            if (HttpContext.Current.Request.QueryString["language"] != null)
            {
                HttpContext.Current.Session["languageCode"] = HttpContext.Current.Request.QueryString["language"].ToString();
            }
            else
            {
                HttpContext.Current.Session["languageCode"] = ConfigurationManager.AppSettings["defaultAdminLanguage"];
            }
            // Save the wished language in cookie for if the user times out
            HttpCookie cookie = new HttpCookie("languageCode");
            cookie.Value = HttpContext.Current.Session["languageCode"].ToString();
            HttpContext.Current.Response.Cookies.Add(cookie);

            // Log out if already logged in
            HttpContext.Current.Session["userid"] = null;

            XslCompiledTransform xslt = new XslCompiledTransform();

            XsltArgumentList xslArg = new XsltArgumentList();
            xslArg.AddExtensionObject("urn:localText", functions);

            xslt.Load(HttpContext.Current.Server.MapPath("/admin/access/form_login.xsl"));

            StringWriter writer = new StringWriter();

            xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, xslArg, writer);

            content.Text += writer.ToString();

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
