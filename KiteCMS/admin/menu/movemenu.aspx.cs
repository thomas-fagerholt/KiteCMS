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
using KiteCMS.Admin.core;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for movemenu.
	/// </summary>
	public class movemenu : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			string action ="";

			// Check for permissions to this module
			admin.userHasAccess(1102,pageId);
			header.Text = Functions.localText("movemenu");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "move")
				funcDoMoveMenuItem(pageId);
			else
				funcChooseLocation(pageId);
		}

		private void funcDoMoveMenuItem(int pageId)
		{
			int newParentPageId = -1;
			int newFollowinSiblingPageId = -1;

			if(HttpContext.Current.Request["parent"] != null && HttpContext.Current.Request["parent"].ToString() != "")
				newParentPageId = int.Parse(HttpContext.Current.Request["parent"]);
			if(HttpContext.Current.Request["child"] != null && HttpContext.Current.Request["child"].ToString() != "")
				newFollowinSiblingPageId = int.Parse(HttpContext.Current.Request["child"]);

			// Check for permissions to move to new location
			Admin admin = new Admin();
			if (admin.userHasAccess(1102,newParentPageId, false, true) || admin.userHasAccess(1102,newFollowinSiblingPageId, false, true))
			{
				try
				{
					Page newpage = new Page(pageId,true);
					newpage.Move(newParentPageId, newFollowinSiblingPageId);

                    content.Text += Functions.localText("menumoved") + Functions.localText("goto") + Functions.publicUrl();
				}
				catch
				{
					throw new Exception ("Move Menuitem: Invalid parameters");
				}
			}
		}

		private void funcChooseLocation(int pageId)
		{
			Functions functions = new Functions();
			Admin admin = new Admin();
			XmlDocument oXmlAccess = new XmlDocument();
			XmlNode oXmlNodeUser;
			XmlNode oXmlNodeAccess;
			XsltArgumentList xslArg = new XsltArgumentList();

			// Insert permissions in XSL to create menuitems
            oXmlAccess.Load(Global.adminXmlPath + "/access.webinfo");
			oXmlNodeUser = oXmlAccess.SelectSingleNode("//user[@id="+ HttpContext.Current.Session["userid"] +" and @active=1]");

			// Does the user have access to root?
			oXmlNodeAccess = oXmlNodeUser.SelectSingleNode("menuid[.=0]");
			if (oXmlNodeAccess != null)
			{
				xslArg.AddParam("hasRoot","", "true");
			}

            XslCompiledTransform xslt = new XslCompiledTransform();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddExtensionObject("urn:userHasAccess", admin);
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/menu/move_menu.xsl"));

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
