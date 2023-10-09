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
using System.Reflection;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for WebForm1.
	/// </summary>
	public class ModulePage : System.Web.UI.Page
	{
		protected Literal lbContent;
		
		private void Page_Load(object sender, System.EventArgs e)
		{
            if (HttpContext.Current.Request.QueryString["format"] != null && HttpContext.Current.Request.QueryString["format"].ToLower() == "rss")
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ContentType = "text/xml";
            }

            Menu menu = new Menu(ConfigurationManager.AppSettings["blnTemplateCheck"]);

			Page page = new Page(menu.Pageid);
			Template template = new Template(page.TemplateId);

            if (template.ModuleClassPublic != "")
            {
                if (template.ModuleClassPublic.Split('@').Length != 2)
                    throw new Exception("ModuleClassPublic has wrong length (pageid=" + page.Pageid + ")");

                string[] moduleclass = template.ModuleClassPublic.Split('@');

                Assembly module = Assembly.LoadFrom(HttpContext.Current.Server.MapPath("/bin/" + moduleclass[1]));

                Type type = module.GetType(moduleclass[0], true, true);

                MethodInfo method = type.GetMethod("Public");

                Object[] parameters = new object[1];
                parameters[0] = menu.Pageid;

                Object Agent = Activator.CreateInstance(type);

                string output = (string)method.Invoke(Agent, parameters);

                if (HttpContext.Current.Request.QueryString["format"] != null && HttpContext.Current.Request.QueryString["format"].ToLower() == "rss")
                {
                    HttpContext.Current.Response.Write(@"<?xml version=""1.0"" encoding=""ISO-8859-1"" ?>");
                    HttpContext.Current.Response.Write(output);
                    HttpContext.Current.Response.End();
                }
                else
                    lbContent.Text += menu.PageHtml.Replace("<!--modulecontent-->", output);
            }
            else if (template.UserControl != "")
            {
                // Insert usercontrol in code
                Control form = (Control)this.FindControl("theform");
                form.Visible = true;
                lbContent.Text = menu.PageHtml.Substring(0, menu.PageHtml.IndexOf("<!--modulecontent-->"));

                PlaceHolder phContent = (PlaceHolder)this.FindControl("phContent");
                Control userControl = LoadControl("/modules/controls/" + template.UserControl);
                phContent.Controls.Add(userControl);

                Literal lit = (Literal)this.FindControl("lbAfterForm");
                lit.Text = menu.PageHtml.Substring(menu.PageHtml.IndexOf("<!--modulecontent-->") + 20);
            }
            else
                lbContent.Text += menu.PageHtml;
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
