using System;
using System.IO;
using System.Text;
using System.Data;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Diagnostics;
using System.Reflection;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class moduleDefault : System.Web.UI.Page
	{
		protected Literal content;
        protected Literal adminFooter;
        protected Literal toplogobar;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			int pageId = admin.loadLocalMenuXml();
			Page page = new Page(pageId);

			Template template = new Template(page.TemplateId);

			bool showMenu= true;
			bool showModuleMenu= true;
		
            if (template.ModuleClassAdmin.Split('@').Length != 2)
                throw new Exception("ModuleClassAdmin has wrong length (pageid=" + page.Pageid + ")");
		
            string[] moduleclass = template.ModuleClassAdmin.Split('@');

			Assembly module = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("/bin/"+ moduleclass[1]));

			Type type = module.GetType(moduleclass[0], true, true);

			MethodInfo method = type.GetMethod("Admin");

			Object[] parameters = new object[4];
			parameters[0] = admin;
			parameters[1] = page;
			parameters[2] = showMenu;
			parameters[3] = showModuleMenu;

			Object Agent = Activator.CreateInstance(type);

			string output = (string)method.Invoke(Agent,parameters).ToString();
			showMenu = (bool)parameters[2];
			showModuleMenu = (bool)parameters[3];

			content.Text = output;

            if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit || ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                adminFooter.Text = @"<script type=""text/javascript"" src='/admin/editor/includes/functions.js'></script>";
            else
                toplogobar.Text = @"<td colspan=""2""><div id=""KiteCMSAdminBarInner""></div></td></tr><tr><td valign=""top"" width=""150""></td>";

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
