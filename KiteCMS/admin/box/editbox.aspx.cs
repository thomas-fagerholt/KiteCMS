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
	public class editbox : System.Web.UI.Page
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
			admin.userHasAccess(1703,page.Pageid);
			header.Text = Functions.localText("editbox");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "delete")
				funcDeleteBox();
			else if (action == "edit")
				funcDoEditBox();
			else if (action == "formedit")
				funcFormEditBox();
			else
				funcListBox();

		}

		private void funcDeleteBox()
		{
			int boxId = -1;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());

			if (boxId != -1)
			{
				Box box = new Box(boxId, true);
				box.Delete();

                content.Text += Functions.localText("boxdeleted") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new ArgumentException("box object doesn't have all required data","box");
		}

		private void funcDoEditBox()
		{
			string title = "";
			string boxcontent = "";
			string xmluri = "";
			int boxId = -1;
			int boxCategoryId = -1;
			bool cascade = false;

			if(HttpContext.Current.Request["boxid"] != null)
				boxId = int.Parse(HttpContext.Current.Request["boxid"].ToString());
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

			if (boxId != -1)
			{
                Box box = new Box(boxId, true);
				box.BoxCategory = new BoxCategory(boxCategoryId);
				box.Title = title;
				box.Content = boxcontent;
				box.XmlUri = xmluri;
				box.Cascade = cascade;

				box.Save();

                content.Text += Functions.localText("boxsaved") + Functions.localText("goto") + Functions.publicUrl();
			}
			else
				throw new ArgumentException("box object doesn't have all required data","box");
		}

		private void funcFormEditBox()
		{
			string output;
			string boxid= "-1";

			if(HttpContext.Current.Request["boxid"]!= null)
				boxid = HttpContext.Current.Request["boxid"].ToString();

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);
			xslArg.AddParam("boxid","",boxid);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml, Server.MapPath("/admin/box/form_edit_box.xsl"),xslArg);

			content.Text += output;
		}

		private void funcListBox()
		{
			string output;

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();

			xslArg.AddExtensionObject("urn:localText", functions);

			output = Functions.transformXml(((Global)HttpContext.Current.ApplicationInstance).oLocalMenuXml,Server.MapPath("/admin/box/list_edit_box.xsl"),xslArg);

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
