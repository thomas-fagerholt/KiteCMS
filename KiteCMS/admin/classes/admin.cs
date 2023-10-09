using System;
using System.Web;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Diagnostics;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for access.
	/// </summary>
	public class Admin
	{

		public Admin()
		{
			XmlNode oXmlNode;

			XmlDocument oXmlAccess = new XmlDocument();

            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");

			// Get userinfo from cookie if there is no in session
			if(HttpContext.Current.Session["userid"]== null && HttpContext.Current.Request.Cookies["user"] != null && HttpContext.Current.Request.Cookies["user"].ToString()!="")
			{
				oXmlNode = oXmlAccess.SelectSingleNode("//user[cookiestring='"+ HttpContext.Current.Request.Cookies["user"].Value +"']");
				if (oXmlNode!= null && oXmlNode.InnerText != "")
					HttpContext.Current.Session["userid"]=oXmlNode.Attributes["id"].InnerText;
			}
			// Check for login and that we are not at the login page
			if (HttpContext.Current.Session["userid"] == null && HttpContext.Current.Request.ServerVariables["url"].Replace("\\","/").ToLower() != "/admin/access/login.aspx")
				HttpContext.Current.Response.Redirect("/admin/access/login.aspx");
		}

		public int loadLocalMenuXml()
		{
			XmlNode oXmlNode;
			int pageId = -1;

			// Take a local copy of global menuXML
			((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml = (XmlDocument)Global.oMenuXml.Clone();

			// Insert chosen pageId in xml
			if (HttpContext.Current.Request["pageId"] != null && 
				int.TryParse(HttpContext.Current.Request["pageId"].ToString(), out pageId))
			{ 
				// Get pageId from request og see if it exists
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[@id="+ pageId +"]");
				try
				{
					pageId = int.Parse(oXmlNode.Attributes["id"].Value);
				}
				catch
				{
					HttpContext.Current.Response.Redirect("/modules/errorpages/404.aspx");
				}
			}
			else
			{
				// No pageId in request, take first "visible in menu"
				oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//page[public=1]");
				pageId = int.Parse(oXmlNode.Attributes["id"].InnerText);
			}

			// Save pageId in xml
			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = pageId.ToString();

			return pageId;
		}

		public void userHasAccess(int intMethodId, int pageId)
		{
			userHasAccess(intMethodId, pageId, true);
		}

		public bool userHasAccess(int intMethodId, int pageId, bool redirect)
		{
			return userHasAccess(intMethodId, pageId, redirect, false);
		}

		public bool userHasAccess(int intMethodId, int pageId, bool redirect, bool accessOnAncestorPageOnly)
		{
			Boolean blnMethodAccess = false;
			Boolean blnMenuAccess = false;
			XmlDocument oXmlAccess = new XmlDocument();
			XmlNode oXmlNodeUser;
			XmlNode oXmlNode;
			XmlNode oXmlNodeAccess;
			XmlNodeList oXmlNodeRange;

            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");

			// Verify permissions
			oXmlNodeUser = oXmlAccess.SelectSingleNode("//user[@id="+ HttpContext.Current.Session["userid"] +" and @active=1]");
			if (oXmlNodeUser!=null)
			{
				//Does the user have access to this method?
				oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("access[.="+ intMethodId +"]");
				if (oXmlNodeAccess!=null)
					blnMethodAccess = true;

				if (blnMethodAccess==false && intMethodId != 0)
				{
					// Does the user have access to a method higher in the heraki?
					oXmlNodeAccess = oXmlAccess.SelectSingleNode("//method[@id="+ intMethodId +"]");
					oXmlNodeRange = oXmlNodeAccess.SelectNodes("ancestor-or-self::module");

					for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
					{
						oXmlNode = oXmlNodeRange.Item(counter).SelectSingleNode("password[@aktiv=1]");
						oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("access[.="+ oXmlNodeRange.Item(counter).Attributes["id"].InnerText +"]");
						if (oXmlNodeAccess!=null)
							blnMethodAccess = true;
					}
				}

				// Does the user have access to this page?
				oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("menuid[.=0]");
				if (oXmlNodeAccess!=null)
					blnMenuAccess = true;

				// Does the user have access to a page higher in the menu heraki?
				if (accessOnAncestorPageOnly)
					oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[@id="+ pageId +"]/ancestor::page");
				else
					oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[@id="+ pageId +"]/ancestor-or-self::page");
				for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				{
					oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("menuid[.="+ oXmlNodeRange.Item(counter).Attributes["id"].InnerText +"]");
					if (oXmlNodeAccess != null)
						blnMenuAccess = true;
				}
			}
			if ((blnMenuAccess && blnMethodAccess)== false && intMethodId >0)
			{
				if (redirect)
					HttpContext.Current.Response.Redirect("/admin/default.aspx?pageId="+ pageId);
				return false;
			}
			else
				return true;
		}

		private string adminPageHtml()
		{
			Admin admin = new Admin();
            Functions functions = new Functions();
            string message = "";

            if (HttpContext.Current.Request["message"] != null)
                message = HttpContext.Current.Request["message"];

            XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:moduleMenu", admin);
            xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("message","",message);

			xslt.Load(HttpContext.Current.Server.MapPath("/admin/data/adminMenu.xsl"));

			StringWriter writer = new StringWriter();

            xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, xslArg, writer);

			return writer.ToString();
		}

		public string moduleMenu(Page page)
		{
			XmlDocument oXmlAccess = new XmlDocument();
			XmlNode oXmlNodeUser;
            XmlNode oXmlNode;
			Boolean blnAccess = false;
			XmlNodeList oXmlNodeRange;

			Functions functions = new Functions();

            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");
            oXmlNode = oXmlAccess.SelectSingleNode("//selectedpage");
            oXmlNode.InnerText = page.Pageid.ToString();

			// Does the user have access to this menu item?
			oXmlNodeUser = oXmlAccess.SelectSingleNode("//user[@id="+ HttpContext.Current.Session["userid"] +"]");
			if(oXmlNodeUser.SelectSingleNode("menuid[.=0]")!= null)
				blnAccess = true;

			// Does the user have access to a menu item higher in the menu heraki?
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[@id="+ page.Pageid +"]/ancestor-or-self::page");
			for (int counter = 0; counter <= oXmlNodeRange.Count-1;counter++)
				if(oXmlNodeUser.SelectSingleNode("menuid[.="+ oXmlNodeRange.Item(counter).Attributes["id"].InnerText +"]")!=null)
					blnAccess = true;

			// Remove module menu if the user does not have access to the page
			if (blnAccess == false)
				oXmlAccess.LoadXml("<website></website>");

            XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("user","",HttpContext.Current.Session["userid"]);
			xslArg.AddParam("blnDraft","",page.HasDraft.ToString());
            
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/includes/modules.xsl"));

			StringWriter writer = new StringWriter();

			xslt.Transform(oXmlAccess,xslArg,writer);

			return writer.ToString().Replace("{//selectedpage}",page.Pageid.ToString());

		}

        public static bool IsLoggedIn()
        {
            XmlNode oXmlNode;


            // Get userinfo from cookie if there is no session
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["userid"] == null && HttpContext.Current.Request.Cookies["user"] != null && HttpContext.Current.Request.Cookies["user"].Value != "")
            {
                if (File.Exists(Global.adminXmlPath + "/access.webinfo"))
                {
                    XmlDocument oXmlAccess = new XmlDocument();
                    oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");

                    oXmlNode = oXmlAccess.SelectSingleNode("//user[cookiestring='" + HttpContext.Current.Request.Cookies["user"].Value + "']");
                    if (oXmlNode != null && oXmlNode.InnerText != "")
                        return true;
                }
                else
                    return false;
            }

            if (HttpContext.Current.Session == null || HttpContext.Current.Session["userid"] == null)
                return false;
            else
                return true;

        }

        public string AdminPageHtml
        {
            get { return adminPageHtml(); }
        }
    
    }
}
