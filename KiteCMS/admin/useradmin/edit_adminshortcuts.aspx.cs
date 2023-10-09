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
	/// Summary description for addmenu.
	/// </summary>
    public class Adminshortcuts : System.Web.UI.Page
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
			User user = new User(int.Parse(HttpContext.Current.Session["userid"].ToString()));
			string action ="";

			// Check for permissions to this module
			if (user.Changepassword != 1)
				HttpContext.Current.Response.Redirect("/admin/default.aspx?pageId="+ pageId);

            header.Text = Functions.localText("adminshortcuts");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "save")
				funcSaveShortcut(user);
			else
				funcFormShortcut(user);
		}

        private void funcSaveShortcut(User user)
		{
            string[] shortcuts = new string[5];
            for (int index = 0; index <= 4; index++)
            {
                if (HttpContext.Current.Request.Form["text" + index] != null && HttpContext.Current.Request.Form["link" + index] != null)
                {
                    shortcuts[index] = HttpContext.Current.Request.Form["text" + index].Replace("¤", "@") + "¤" + HttpContext.Current.Request.Form["link" + index].Replace("¤", "@");
                }
            }
			if (user.UserId != -1)
			{
				user.Shortcuts = shortcuts;
				user.Save();

                content.Text += Functions.localText("usersaved") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new Exception ("Edit adminshortcuts: Invalid parameters");
		}

        private void funcFormShortcut(User user)
		{
			Functions functions = new Functions();
			XmlDocument oXmlAccess = new XmlDocument();

			XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
            xslArg.AddParam("user", "", user.UserId);
            xslt.Load(HttpContext.Current.Server.MapPath("/admin/useradmin/form_edit_shortcuts.xsl"));

			StringWriter writer = new StringWriter();

            xslt.Transform(Global.adminXmlPath + "/access.webinfo", xslArg, writer);
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
