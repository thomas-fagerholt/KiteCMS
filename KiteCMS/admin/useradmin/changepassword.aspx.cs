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
	public class changepassword : System.Web.UI.Page
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

			header.Text = Functions.localText("changepassword");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "save")
				funcSavePassword(user);
			else
				funcFormSavePassword(user);
		}

		private void funcSavePassword(User user)
		{
			string newPassword = "";

			if(HttpContext.Current.Request["newpassword"] != null)
				newPassword = HttpContext.Current.Request["newpassword"].ToString();

			if (user.UserId != -1 && newPassword != "")
			{
				user.Password = newPassword;
				user.Save();

                content.Text += Functions.localText("passwordchanged") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new Exception ("Change password: Invalid parameters");
		}

		private void funcFormSavePassword(User user)
		{
			Functions functions = new Functions();
			XmlDocument oXmlAccess = new XmlDocument();

			XslCompiledTransform xslt = new XslCompiledTransform();

			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);
			xslt.Load(HttpContext.Current.Server.MapPath("/admin/useradmin/form_changepassword.xsl"));

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
