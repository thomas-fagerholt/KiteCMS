namespace KiteCMS.Admin
{
	using System;
	using System.Data;
	using System.Drawing;
    using System.Configuration;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using KiteCMS.Admin.core;

	/// <summary>
	///		Summary description for NSeditor.
	/// </summary>
	public class NSeditor : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label editor;
		protected Literal options;
		public string pageId;
		public bool blnEdit;

		private void Page_Load(object sender, System.EventArgs e)
		{
			Admin admin = new Admin();
			pageId = admin.loadLocalMenuXml().ToString();

            editor.Text = "<iframe width=" + ConfigurationManager.AppSettings["EditorWidth"] + " height=400 id=tbContent src=/admin/content/editor_ns.aspx style='position:absolute;left:100px;top:150px'></iframe>";

			// find styles to add to dropdown from stylesheet
            if (ConfigurationManager.AppSettings["EditorStyles"] != null)
			{
                string[] styles = ConfigurationManager.AppSettings["EditorStyles"].Split(',');
				for (int i = 0; i < styles.Length; i++)
					options.Text += "<option value='"+ styles[i].Trim() +"'>"+ styles[i].Trim() +"</option>";
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
