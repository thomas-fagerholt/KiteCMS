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
	/// Summary description for editmenu.
	/// </summary>
	public class CopyMenu : System.Web.UI.Page
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
			admin.userHasAccess(1105,page.Pageid);
			header.Text = Functions.localText("copymenu");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "docopy")
				funcDoCopyMenuItem(page.Pageid, admin);
			else
				funcCopyMenuItem();
		}

		private void funcDoCopyMenuItem(int pageId, Admin admin)
		{
			bool includeSubpages = false;
			bool includeContent = false;
			int newParentPageId = -1;
			int newFollowinSiblingPageId = -1;

			if(HttpContext.Current.Request["includeSubpages"] != null)
				includeSubpages = bool.Parse(HttpContext.Current.Request["includeSubpages"].ToString());
			if(HttpContext.Current.Request["includeContent"] != null)
				includeContent = bool.Parse(HttpContext.Current.Request["includeContent"].ToString());
			if(HttpContext.Current.Request["parent"] != null && HttpContext.Current.Request["parent"].ToString() != "")
				newParentPageId = int.Parse(HttpContext.Current.Request["parent"]);
			if(HttpContext.Current.Request["child"] != null && HttpContext.Current.Request["child"].ToString() != "")
				newFollowinSiblingPageId = int.Parse(HttpContext.Current.Request["child"]);

			// Save new data
			if (admin.userHasAccess(1105,newParentPageId, false, true) || admin.userHasAccess(1105,newFollowinSiblingPageId, false, true))
			{
				if (pageId != -1)
				{
					Page page = new Page(pageId, true);
                    page.Copy(newParentPageId, newFollowinSiblingPageId, includeSubpages, includeContent);
                    content.Text += Functions.localText("menusaved") + Functions.localText("goto") + Functions.publicUrl();
				}
				else
					throw new Exception ("Copy Menuitem: Invalid parameters");
			}
		}

		private void funcCopyMenuItem()
		{
			string output;
			Functions functions = new Functions();
			Admin admin = new Admin();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddExtensionObject("urn:userHasAccess", admin);

            XmlDocument xml = (XmlDocument)((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.Clone();
            XmlNodeList nodes = xml.SelectNodes("//page");
            foreach (XmlNode node in nodes)
            {
                bool userHasAccess = admin.userHasAccess(1105, int.Parse(node.Attributes["id"].Value), false, true);
                XmlAttribute attr = ((XmlElement)node).SetAttributeNode("userhasaccess","");
                attr.Value = userHasAccess.ToString().ToLower();
            }

			output = Functions.transformXml(xml,Server.MapPath("/admin/menu/copy_menu.xsl"),xslArg);

			content.Text += output;

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
