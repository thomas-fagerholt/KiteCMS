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
	public class deletetemplate : System.Web.UI.Page
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
			admin.userHasAccess(1403,page.Pageid);
			header.Text = Functions.localText("deletetemplate");

			if (HttpContext.Current.Request["action"] != null)
				action = HttpContext.Current.Request["action"].ToString().ToLower();

			if (action == "delete")
				funcDoDeleteTemplate();
			else if (action == "choose")
				funcFormDeleteTemplate();
			else
				funcDeleteTemplate();

		}

		private void funcDoDeleteTemplate()
		{
			int templateId = -1;
			int useTemplateId = -1;

			if(HttpContext.Current.Request["templateid"] != null)
				templateId = int.Parse(HttpContext.Current.Request["templateid"].ToString());
			if(HttpContext.Current.Request["usetemplateid"] != null)
				useTemplateId = int.Parse(HttpContext.Current.Request["usetemplateid"].ToString());

			if (templateId != -1)
			{
				Template template = new Template(templateId,true);

				template.Delete(useTemplateId);

                content.Text += Functions.localText("templatedeleted") + Functions.localText("goto") + Functions.publicUrl();

			}
			else
				throw new ArgumentException("template object doesn't have all required data","template");
		}

		private void funcFormDeleteTemplate()
		{
			int templateId = -1;

			if(HttpContext.Current.Request["templateid"] != null)
				templateId = int.Parse(HttpContext.Current.Request["templateid"].ToString());

			if (templateId != -1)
			{
				Functions functions = new Functions();
				XsltArgumentList xslArg = new XsltArgumentList();
				xslArg.AddParam("templateid","", templateId.ToString());
				xslArg.AddExtensionObject("urn:localText", functions);

				content.Text += Functions.transformMenuXml(Server.MapPath("/admin/template/form_delete_template.xsl"),xslArg);
			}
			else
				throw new ArgumentException("template object doesn't have a templateid","template");

		}

		private void funcDeleteTemplate()
		{

			Functions functions = new Functions();
			XsltArgumentList xslArg = new XsltArgumentList();
			xslArg.AddExtensionObject("urn:localText", functions);

			content.Text += Functions.transformMenuXml(Server.MapPath("/admin/template/delete_template.xsl"),xslArg);

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
