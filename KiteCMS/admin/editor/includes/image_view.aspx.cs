using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using KiteCMS.Admin;

namespace KiteCMS.Admin
{
	/// <summary>
	/// Summary description for image_view.
	/// </summary>
	public class image_view : System.Web.UI.Page
	{
		public string strFolder = "";
		public string strFile = "";
        public string popup = "false";

		protected Literal content;

		private void Page_Load(object sender, System.EventArgs e)
		{
            string strFiletypeGraphic = ConfigurationManager.AppSettings["GraphicFiles"];
            string GraphicRootDirectory = ConfigurationManager.AppSettings["GraphicRootDirectory"];

			string CurrentFolder;
			string CurrentFolderPath;
			string RootFolderPath;
            StringBuilder output = new StringBuilder();

			GraphicRootDirectory = GraphicRootDirectory.Replace(".","");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"\",@"/");
			GraphicRootDirectory = GraphicRootDirectory.Replace(@"//",@"/");

			if(HttpContext.Current.Request["Folder"] != null)
				strFolder = HttpContext.Current.Request["Folder"].ToString();
			if(HttpContext.Current.Request["File"] != null)
				strFile = HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request["File"].ToString());
            if (HttpContext.Current.Request["popup"] != null)
                popup = HttpContext.Current.Request["popup"].ToString().ToLower();

			// Dont show under the root
			strFolder = strFolder.Replace(".","");
			strFolder = strFolder.Replace(@"\",@"/");
			strFolder = strFolder.Replace(@"//",@"/");

			strFile = strFile.Replace(@"\",@"/");

			// If folders is empty, then show the root folder
			if (strFolder.IndexOf(GraphicRootDirectory)!=0)
				strFolder = GraphicRootDirectory;

			// Format the strings
			if (!strFolder.EndsWith("/"))
				strFolder += "/";
			if (strFile.IndexOf("/")>-1)
				strFile = strFile.Substring(strFile.LastIndexOf("/"));

			RootFolderPath = HttpContext.Current.Server.MapPath("/");
			CurrentFolderPath = HttpContext.Current.Server.MapPath(strFolder);
			CurrentFolder = CurrentFolderPath.Replace(RootFolderPath,"");
			FileInfo fileinfo = new FileInfo(CurrentFolderPath + strFile);
			if (strFile != "")
			{
				output.Append("<div id='divImg'>&nbsp;</div>"+ Environment.NewLine);
				output.Append(""+ Functions.localText("size") +": "+ Environment.NewLine);
				output.Append(int.Parse(((fileinfo.Length/1024)+1).ToString()) +"kb<br/>"+ Environment.NewLine);
                output.Append("<img id='picImg' src='" + strFolder + strFile + "' border='1' alt='" + strFile + "' title='" + strFile + "'/><br/>&nbsp;<br/>" + Environment.NewLine);
                output.Append("<input type=\"button\" onclick='funcImage" + (popup == "true" ? "Popup" : "") + "Chosen();' value=\"" + Functions.localText("insertpicture") + "\" class=\"almknap\"/>" + Environment.NewLine);
                output.Append("&nbsp;&nbsp;<input type=\"button\" onclick='funcImageCancel();' value=\"" + Functions.localText("cancel") + "\" class=\"almknap\"/>" + Environment.NewLine);

				content.Text = output.ToString();
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
