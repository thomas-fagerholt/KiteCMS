using System;
using System.Collections;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace KiteCMS
{
	/// <summary>
	/// Summary description for redirect.
	/// </summary>
	public class redirect : System.Web.UI.Page
	{
        protected Literal message;

        private void Page_Load(object sender, System.EventArgs e)
		{
            Menu menu = new Menu(ConfigurationManager.AppSettings["blnTemplateCheck"]);
            Page page = new Page(menu.Pageid);
			Functions functions = new Functions();
			string url="";

			url = Functions.funcGetParameter("url",menu.Pageid);
            if (page.ContentHolders["redirecturl"] != null && page.ContentHolders["redirecturl"] != "")
                url = page.ContentHolders["redirecturl"];
            if (((Global)HttpContext.Current.ApplicationInstance).EditMode == Global.EditModeEnum.Public)
            {
                if (url != "")
                    HttpContext.Current.Response.Redirect(url);
            }
            else
            {
                string linkText = Functions.localText("redirectinpublic");
                message.Text = menu.PageHtml.Replace("##linktext##",linkText);
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
