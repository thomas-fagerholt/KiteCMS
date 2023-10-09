using System;
using System.Configuration;
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
    public class admin_default : System.Web.UI.Page
    {
        protected Literal lbContent;

        private void Page_Load(object sender, System.EventArgs e)
        {
            Admin admin = new Admin();
            int pageId = admin.loadLocalMenuXml();
            Page page = new Page(pageId);

            Template template = new Template(page.TemplateId);

            string adminFooter = "";
            string toplogobar = "";
            bool showMenu = true;
            bool showModuleMenu = true;

            if (template.ModuleClassAdmin != "")
            {
                string html = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/transitional.dtd\">\n" +
                    "<html>\n" +
                    "	<head>\n" +
                    "	    <title>KiteCMS</title>\n" +
                    "		<link href=\"/admin/default.css\" type=\"text/css\" rel=\"stylesheet\"/>\n" +
                    "		<link href=\"/admin/adminpages.css\" type=\"text/css\" rel=\"stylesheet\"/>\n" +
                    "	</head>\n" +
                    "	<body>\n" +
                    "		<table cellspacing=\"0\" cellpadding=\"0\" width=\"100%\" border=\"0\">\n" +
                    "		<tr>\n" +
                    "   		{0}<!--toplogobar-->\n" +
                    "				<td valign=\"top\">\n" +
                    "					<h2>{1}<!--header--></h2>\n" +
                    "					{2}<!--content--></td>\n" +
                    "			</tr>\n" +
                    "		</table>\n" +
                    "		{3}<!--adminFooter-->\n" +
                    "	</body>\n" +
                    "</html>\n";

                if (template.ModuleClassAdmin.Split('@').Length != 2)
                    throw new Exception("ModuleClassAdmin has wrong length (pageid=" + page.Pageid + ")");

                string[] moduleclass = template.ModuleClassAdmin.Split('@');

                Assembly module = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("/bin/" + moduleclass[1]));

                Type type = module.GetType(moduleclass[0], true, true);

                MethodInfo method = type.GetMethod("Admin");

                Object[] parameters = new object[4];
                parameters[0] = admin;
                parameters[1] = page;
                parameters[2] = showMenu;
                parameters[3] = showModuleMenu;

                Object Agent = Activator.CreateInstance(type);

                string content = (string)method.Invoke(Agent, parameters).ToString();
                showMenu = (bool)parameters[2];
                showModuleMenu = (bool)parameters[3];


                if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEdit || ((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.AdminEditDraft)
                    adminFooter = @"<script type=""text/javascript"" src='/admin/editor/includes/functions.js'></script>";
                else
                    toplogobar = @"<td colspan=""2""><div id=""KiteCMSAdminBarInner""></div></td></tr><tr><td valign=""top"" width=""150""></td>";

                lbContent.Text = string.Format(html, toplogobar, "", content, adminFooter);

            }
            else
            {
                Menu menu = new Menu(ConfigurationManager.AppSettings["blnTemplateCheck"]);
                lbContent.Text += menu.PageHtml;

            }
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
