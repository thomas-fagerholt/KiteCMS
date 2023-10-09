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
	public class addbox : System.Web.UI.Page
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
			admin.userHasAccess(1702,page.Pageid);
			header.Text = Functions.localText("addbox");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "add")
				funcDoAddBox();
			else
				funcFormAddBox();

		}

		private void funcDoAddBox()
		{
			string title = "";
			string boxcontent = "";
			string xmluri = "";
			int boxCategoryId = -1;
			bool cascade = false;

			if(HttpContext.Current.Request["boxcategoryid"] != null)
				boxCategoryId = int.Parse(HttpContext.Current.Request["boxcategoryid"].ToString());
			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["content"] != null)
				boxcontent = HttpContext.Current.Request["content"].ToString();
			if(HttpContext.Current.Request["xmluri"] != null)
				xmluri = HttpContext.Current.Request["xmluri"].ToString();
			if(HttpContext.Current.Request["cascade"] != null)
				cascade = bool.Parse(HttpContext.Current.Request["cascade"].ToString());

			Box box = new Box(true);
			box.BoxCategory = new BoxCategory(boxCategoryId);
			box.Title = title;
			box.Content = boxcontent;
			box.XmlUri = xmluri;
			box.Cascade = cascade;

			box.Save();

            content.Text += Functions.localText("boxsaved") + Functions.localText("goto") + Functions.publicUrl();

		}

		private void funcFormAddBox()
		{
			string output;

			if (BoxCategory.getAllBoxCategories() != null && BoxCategory.getAllBoxCategories().Count != 0)
			{
				Functions functions = new Functions();
				XsltArgumentList xslArg = new XsltArgumentList();

				xslArg.AddExtensionObject("urn:localText", functions);

				output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/form_add_box.xsl"),xslArg);

				content.Text += output;
			}
			else
                content.Text += Functions.localText("noboxcategoryexists") + Functions.localText("goto") + Functions.publicUrl();
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
