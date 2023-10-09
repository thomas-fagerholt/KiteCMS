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
	public class addboxcategpry : System.Web.UI.Page
	{
		protected Literal menu;
		protected Literal header;
		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();

			Page page = new Page(pageId);

			string action = "";

			// Check for permissions to this module
			admin.userHasAccess(1704,page.Pageid);
			header.Text = Functions.localText("addcategory");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "add")
				funcDoAddBoxCategory();
			else
				funcFormAddBoxCategory();

		}

		private void funcDoAddBoxCategory()
		{
			string title = "";
			string htmlstring = "";
			int boxType = -1;

			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["htmlstring"] != null)
				htmlstring = HttpContext.Current.Request["htmlstring"].ToString();
			if(HttpContext.Current.Request["type"] != null)
				boxType = int.Parse(HttpContext.Current.Request["type"].ToString());

			BoxCategory boxCategory = new BoxCategory(true);
			boxCategory.Title = title;
			boxCategory.Htmlstring = htmlstring;
			boxCategory.BoxCategoryType = (BoxCategory.BoxCategoryTypeEnum)boxType;

			boxCategory.Save();

            content.Text += Functions.localText("boxcategorysaved") + Functions.localText("goto") + Functions.publicUrl();

		}

		private void funcFormAddBoxCategory()
		{
			string output;

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/form_add_boxCategory.xsl"),xslArg);

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
