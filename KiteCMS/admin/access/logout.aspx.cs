using System;
using System.Web;

namespace KiteCMS.Admin
{
    /// <summary>
    /// Summary description for login.
    /// </summary>
    public class logout : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
            Admin admin = new Admin();
            int pageId = admin.loadLocalMenuXml();

            Page page = new Page(pageId);
            Template template = new Template(page.TemplateId);

			// Log out if already logged in
			HttpContext.Current.Session["userid"] = null;
            HttpContext.Current.Response.Cookies["user"].Value = "";

            HttpContext.Current.Response.Redirect(template.Publicurl + "?pageid=" + pageId);
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
