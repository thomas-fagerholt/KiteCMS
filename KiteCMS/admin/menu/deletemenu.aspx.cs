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
	/// Summary description for deletemenu.
	/// </summary>
	public class deletemenu : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;
		protected Literal modulemenu;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();
            string action="";

			// Check for permissions to this module
			admin.userHasAccess(1104,pageId);
			header.Text = Functions.localText("deletemenu");

			Page page = new Page(pageId,true);

            if (HttpContext.Current.Request.QueryString["action"]!=null)
                action = HttpContext.Current.Request.QueryString["action"];

			// Get parentPid and set selectedpage=parentPid so the user goes to this page after the receiption page
			XmlNode oXmlNode;
			oXmlNode = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectSingleNode("//selectedpage");
			oXmlNode.InnerText = page.ParentPageid.ToString();

			if (pageId != -1)
			{
                XmlNodeList nodes = ((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml.SelectNodes("//page[@id='" + pageId + "']/descendant-or-self::page");
                if (nodes.Count == 1 || action == "dodelete")
                {
                    page.Delete();
                    content.Text += Functions.localText("menudeleted") + Functions.localText("goto") + Functions.publicUrl();
                }
                else
                {
                    content.Text += string.Format(Functions.localText("deletemenualertmany"),nodes.Count.ToString()).Replace("##pageid##",pageId.ToString());
                }
            }
			else
				throw new Exception ("Delete Menuitem: Invalid parameters");

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
