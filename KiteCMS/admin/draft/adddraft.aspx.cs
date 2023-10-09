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
	/// Summary description for edittemplate.
	/// </summary>
	public class adddraft : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1301,pageId);
			header.Text = Functions.localText("adddraft");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "add")
				funcDoAddDraft(pageId);
			else
				funcFormAddDraft();

		}

		private void funcDoAddDraft(int pageId)
		{
			Page page = new Page(pageId,true);

            page.HasDraft = 1;
			if(HttpContext.Current.Request["copycontent"] != null)
				page.DraftContentHolders = page.ContentHolders;

			page.Save();

			content.Text += Functions.localText("draftadded") + Functions.publicUrl();
		}

		private void funcFormAddDraft()
		{
			string output;
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			output = Functions.transformMenuXml(Server.MapPath("/admin/draft/form_add_draft.xsl"),xslArg);

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
