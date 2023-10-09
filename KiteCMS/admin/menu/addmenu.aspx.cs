using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Configuration;
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for addmenu.
	/// </summary>
	public class addmenu : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action ="";

			// Check for permissions to this module
			admin.userHasAccess(1101,page.Pageid);
			header.Text = Functions.localText("addmenu");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "doadd")
				funcDoAddMenuItem(page.Pageid);
			else if (action == "addinfo")
				funcAddinfo(page.Pageid);
			else
				funcChooseLocation(page.Pageid);
		}

		private void funcDoAddMenuItem(int pageId)
		{
			string title = "";
			string isPublic = "";
			int templateId = -1;
			int parentPageId = -1;
			int followinSiblingPageId = -1;
			string friendlyUrl = "";
			ParameterCollection parameters = new ParameterCollection();

			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["public"] != null)
				isPublic = HttpContext.Current.Request["public"].ToString();
			if(HttpContext.Current.Request["template"] != null)
				templateId = int.Parse(HttpContext.Current.Request["template"]);
			if(HttpContext.Current.Request["parent"] != null && HttpContext.Current.Request["parent"].ToString() != "")
				parentPageId = int.Parse(HttpContext.Current.Request["parent"]);
			if(HttpContext.Current.Request["child"] != null && HttpContext.Current.Request["child"].ToString() != "")
				followinSiblingPageId = int.Parse(HttpContext.Current.Request["child"]);
			if(HttpContext.Current.Request["friendlyurl"] != null && HttpContext.Current.Request["friendlyurl"].ToString() != "")
				friendlyUrl = HttpContext.Current.Request["friendlyurl"];

            if (title != "" && isPublic != "" && templateId != -1)
			{
                try
                {
                    Page newpage = new Page(true);
                    newpage.Title = title;
                    newpage.IsPublic = int.Parse(isPublic);
                    newpage.TemplateId = templateId;
                    newpage.ParentPageid = parentPageId;
                    newpage.FollowinSiblingPageId = followinSiblingPageId;
                    newpage.FriendlyUrl = friendlyUrl;

                    // find extra parameters
                    foreach (string key in HttpContext.Current.Request.Form)
                    {
                        if (key != "friendlyurl" && key != "template" && key != "public" && key != "title" && key != "pageid" && key != "action" && key != "child" && key != "parent")
                        {
                            if (HttpContext.Current.Request.Form[key] != null && HttpContext.Current.Request.Form[key].ToString() != "")
                            {
                                Parameter parameter = new Parameter(key, HttpContext.Current.Request.Form[key]);
                                parameters.Add(parameter);
                            }
                        }
                    }
                    newpage.Parameters = parameters;

                    newpage.Save();
                    Functions.publicUrlRedirect(newpage.Pageid, Functions.localText("menuadded"));
                }
                catch (ArgumentOutOfRangeException)
                {
                    content.Text += Functions.localText("userfriendlyexists") + Functions.publicUrl();
                }

			}
			else
				throw new Exception ("Add Menuitem: Invalid parameters");
		}

		private void funcAddinfo(int pageId)
		{
			Functions functions = new Functions();
			string child = "";
			string parent = "";
		
			if(HttpContext.Current.Request["child"] != null)
				child = HttpContext.Current.Request["child"].ToString();
			if(HttpContext.Current.Request["parent"] != null)
				parent = HttpContext.Current.Request["parent"].ToString();
			if (child != "" || parent != "")
			{
                XslCompiledTransform xslt = new XslCompiledTransform();

				XsltArgumentList xslArg = new XsltArgumentList();
				xslArg.AddExtensionObject("urn:localText", functions);
				xslArg.AddParam("pageid","",pageId);
				xslArg.AddParam("parent","",parent);
				xslArg.AddParam("child","",child);
                if (ConfigurationManager.AppSettings["useUserfriendlyUrl"] != null && bool.Parse(ConfigurationManager.AppSettings["useUserfriendlyUrl"]))
					xslArg.AddParam("useUserfriendlyUrl","","true");
				xslt.Load(HttpContext.Current.Server.MapPath("/admin/menu/add_info.xsl"));

				StringWriter writer = new StringWriter();

				xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,xslArg,writer);
				content.Text += writer.ToString();
			}
		}

		private void funcChooseLocation(int pageId)
		{
			Functions functions = new Functions();
			XmlDocument oXmlAccess = new XmlDocument();
			XmlNode oXmlNodeUser;
			XmlNode oXmlNodeAccess;
			XmlNode oXmlNodeTemp;
			XmlNode oXmlNodeNew;
			XmlNodeList oXmlNodeRange;

			// Insert permissions in XSL to create menu items
            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");
			oXmlNodeUser = oXmlAccess.SelectSingleNode("//user[@id="+ HttpContext.Current.Session["userid"] +" and @active=1]");
			oXmlNodeRange = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page");

			// Does the user have access to root?
			oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("menuid[.=0]");
			if (oXmlNodeAccess != null)
			{
				oXmlNodeTemp = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("element","canAddMenuRoot","");
				oXmlNodeNew = oXmlNodeRange.Item(0).AppendChild(oXmlNodeTemp);
			}

			// Does the user have acces to the specific page?
			foreach(XmlNode objNode in oXmlNodeRange)
			{
				oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("menuid[.="+ objNode.Attributes["id"].Value +"]");
				if(oXmlNodeAccess != null)
				{
					oXmlNodeTemp = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.CreateElement("element","canAddMenu","");
					oXmlNodeNew = objNode.AppendChild(oXmlNodeTemp);
				}
			}

            XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/menu/choose_location.xsl"));

			StringWriter writer = new StringWriter();

			xslt.Transform(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,xslArg,writer);
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
