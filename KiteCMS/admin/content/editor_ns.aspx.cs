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

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for editor_ns.
	/// </summary>
	public class editor_ns : System.Web.UI.Page
	{

        protected string contentid = "";
		protected System.Web.UI.WebControls.Label editor;

		private void Page_Load(object sender, System.EventArgs e)
		{
            if (HttpContext.Current.Request.QueryString["contentid"] != null)
                contentid = HttpContext.Current.Request.QueryString["contentid"];
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
