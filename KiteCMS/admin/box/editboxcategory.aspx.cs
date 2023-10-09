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
	public class editboxcategory : System.Web.UI.Page
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
			admin.userHasAccess(1705,page.Pageid);
			header.Text = Functions.localText("editcategory");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "dodelete")
				funcDoDeleteBoxCategory();
			else if (action == "delete")
				funcDeleteBoxCategory();
			else if (action == "edit")
				funcDoEditBoxCategory();
			else if (action == "formedit")
				funcFormEditBoxCategory();
			else
				funcListBoxCategory();
		}

		private void funcDeleteBoxCategory()
		{
			string output;
			string boxcategoryid= "-1";

			if(HttpContext.Current.Request["boxcategoryid"]!= null)
				boxcategoryid = HttpContext.Current.Request["boxcategoryid"].ToString();

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("boxcategoryid","",boxcategoryid);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/form_delete_boxcategory.xsl"),xslArg);

			content.Text += output;
		}

		private void funcDoDeleteBoxCategory()
		{
			int boxcategoryid = -1;
			int useboxcategoryid = -1;

			if(HttpContext.Current.Request["boxcategoryid"] != null)
				boxcategoryid = int.Parse(HttpContext.Current.Request["boxcategoryid"].ToString());
			if(HttpContext.Current.Request["useboxcategoryid"] != null)
				useboxcategoryid = int.Parse(HttpContext.Current.Request["useboxcategoryid"].ToString());

			if (boxcategoryid != -1)
			{
				BoxCategory boxcategory = new BoxCategory(boxcategoryid, true);
				boxcategory.Delete(useboxcategoryid);

                content.Text += Functions.localText("boxcategorydeleted") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new ArgumentException("box object doesn't have all required data","box");
		}

		private void funcDoEditBoxCategory()
		{
			string title = "";
			string htmlstring = "";
			int boxCategoryId = -1;
			int boxType = -1;

			if(HttpContext.Current.Request["boxCategoryId"] != null)
				boxCategoryId = int.Parse(HttpContext.Current.Request["boxCategoryId"].ToString());
			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["htmlstring"] != null)
				htmlstring = HttpContext.Current.Request["htmlstring"].ToString();
			if(HttpContext.Current.Request["boxType"] != null)
				boxType = int.Parse(HttpContext.Current.Request["boxType"].ToString());

			if (boxCategoryId != -1 && boxType != -1 && title != "" && htmlstring != "")
			{
				BoxCategory boxCategory = new BoxCategory(boxCategoryId, true);
				boxCategory.Title = title;
				boxCategory.Htmlstring = htmlstring;
				boxCategory.BoxCategoryType = (BoxCategory.BoxCategoryTypeEnum)boxType;

				boxCategory.Save();

                content.Text += Functions.localText("boxcategorysaved") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new Exception ("Edit content: Invalid parameters");
		}

		private void funcFormEditBoxCategory()
		{
			string output;
			string boxcategoryid= "-1";

			if(HttpContext.Current.Request["boxcategoryid"]!= null)
				boxcategoryid = HttpContext.Current.Request["boxcategoryid"].ToString();

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("boxcategoryid","",boxcategoryid);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/form_edit_boxcategory.xsl"),xslArg);

			content.Text += output;
		}

		private void funcListBoxCategory()
		{
			string output;

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

            output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/list_edit_boxcategory.xsl"), xslArg);

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
