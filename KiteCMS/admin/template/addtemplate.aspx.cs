using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml.Xsl;

namespace KiteCMS.Admin
{
    /// <summary>
    /// Summary description for edittemplate.
    /// </summary>
    public class addtemplate : System.Web.UI.Page
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
			admin.userHasAccess(1401,page.Pageid);
			header.Text = Functions.localText("addtemplate");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "save")
				funcDoAddTemplate();
			else
				funcFormAddTemplate();

		}

		private void funcDoAddTemplate()
		{
			int templateId = -1;
			string title = "";
			string publicurl = "";
			string xslurl = "";
			string adminurl = "";
			string moduleclassadmin = "";
			string moduleclasspublic = "";
			string templatecolor = "";

			if(HttpContext.Current.Request["templateid"] != null)
				templateId = int.Parse(HttpContext.Current.Request["templateid"].ToString());
			if(HttpContext.Current.Request["title"] != null)
				title = HttpContext.Current.Request["title"].ToString();
			if(HttpContext.Current.Request["publicurl"] != null)
				publicurl = HttpContext.Current.Request["publicurl"].ToString();
			if(HttpContext.Current.Request["xslurl"] != null)
				xslurl = HttpContext.Current.Request["xslurl"].ToString();
			if(HttpContext.Current.Request["adminurl"] != null)
				adminurl = HttpContext.Current.Request["adminurl"].ToString();
			if(HttpContext.Current.Request["moduleclassadmin"] != null)
				moduleclassadmin = HttpContext.Current.Request["moduleclassadmin"].ToString();
			if(HttpContext.Current.Request["moduleclasspublic"] != null)
				moduleclasspublic = HttpContext.Current.Request["moduleclasspublic"].ToString();
			if(HttpContext.Current.Request["templatecolor"] != null)
				templatecolor = HttpContext.Current.Request["templatecolor"].ToString();

			if (templateId == -1 && title != "" && publicurl != "" && xslurl != "" && adminurl != "")
			{
				Template template = new Template(true);

				template.Title = title;
				template.Publicurl = publicurl;
				template.Xslurl = xslurl;
				template.Adminurl = adminurl;
				template.ModuleClassAdmin = moduleclassadmin;
				template.ModuleClassPublic = moduleclasspublic;
				template.Templatecolor = templatecolor;

				template.Save();

                content.Text += Functions.localText("templatesaved") + Functions.localText("goto") + Functions.publicUrl();

			}
			else
				throw new ArgumentException("template object doesn't have all required data","template");
		}

		private void funcFormAddTemplate()
		{
			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);

			content.Text += Functions.transformMenuXml(Server.MapPath("/admin/template/form_edit_template.xsl"),xslArg);
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
